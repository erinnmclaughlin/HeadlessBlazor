using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Microsoft.Playwright;

namespace HeadlessBlazor.E2E.Tests;

/// <summary>
/// Boots the E2E harness host once for the whole test run and hands tests a browser plus its base
/// URL.
/// </summary>
/// <remarks>
/// The host runs as a real out-of-process Kestrel server rather than via
/// <c>WebApplicationFactory</c>: that spins up an in-memory <c>TestServer</c> with no listening
/// socket, which a real browser cannot connect to.
/// </remarks>
public sealed class HarnessFixture : IAsyncLifetime
{
#if DEBUG
    private const string Configuration = "Debug";
#else
    private const string Configuration = "Release";
#endif

    private Process? _host;
    private IPlaywright? _playwright;

    public string BaseUrl { get; private set; } = "";

    public IBrowser Browser { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        var port = GetFreePort();
        BaseUrl = $"http://localhost:{port}";

        _host = StartHost(BaseUrl);
        await WaitForHealthyAsync();

        // Idempotent and quick once the browser is present, which keeps a fresh clone's first run
        // from failing on a missing binary.
        Microsoft.Playwright.Program.Main(["install", "chromium"]);

        _playwright = await Playwright.CreateAsync();
        Browser = await _playwright.Chromium.LaunchAsync();
    }

    public async Task DisposeAsync()
    {
        if (Browser is not null)
        {
            await Browser.CloseAsync();
        }

        _playwright?.Dispose();

        if (_host is { HasExited: false })
        {
            // `dotnet run` launches the app as a child process, so killing only the parent would
            // leave Kestrel holding the port.
            _host.Kill(entireProcessTree: true);
            await _host.WaitForExitAsync();
        }

        _host?.Dispose();
    }

    private static Process StartHost(string baseUrl)
    {
        var projectPath = Path.Combine(FindRepoRoot(), "tests", "HeadlessBlazor.E2E.Host");

        var startInfo = new ProcessStartInfo("dotnet")
        {
            ArgumentList =
            {
                "run",
                "--no-build",
                "--configuration", Configuration,
                "--project", projectPath,
                "--urls", baseUrl
            },
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        // Static Web Assets are only wired up automatically in Development when running against
        // build (rather than published) output. Under Production the asset endpoints still get
        // registered from the manifest, but with no content roots behind them - so every request
        // for blazor.web.js returns 200 with an empty body and Blazor silently never boots.
        startInfo.Environment["ASPNETCORE_ENVIRONMENT"] = "Development";

        var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Failed to start the E2E harness host.");

        // Kestrel blocks on a full stdout pipe, so these have to be drained even though nothing
        // reads them.
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        return process;
    }

    private async Task WaitForHealthyAsync()
    {
        using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(90));

        while (!timeout.IsCancellationRequested)
        {
            if (_host is { HasExited: true })
            {
                throw new InvalidOperationException(
                    $"The E2E harness host exited during startup with code {_host.ExitCode}.");
            }

            try
            {
                using var response = await client.GetAsync($"{BaseUrl}/healthz", timeout.Token);

                if (response.IsSuccessStatusCode)
                {
                    return;
                }
            }
            catch (HttpRequestException)
            {
                // Not listening yet.
            }
            catch (TaskCanceledException)
            {
                // Request timed out; fall through to the retry delay.
            }

            await Task.Delay(250);
        }

        throw new TimeoutException($"The E2E harness host at {BaseUrl} never became healthy.");
    }

    private static string FindRepoRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "HeadlessBlazor.sln")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
            ?? throw new InvalidOperationException("Could not locate the repo root (HeadlessBlazor.sln).");
    }

    private static int GetFreePort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();

        try
        {
            return ((IPEndPoint)listener.LocalEndpoint).Port;
        }
        finally
        {
            listener.Stop();
        }
    }
}

[CollectionDefinition(Name)]
public sealed class HarnessCollection : ICollectionFixture<HarnessFixture>
{
    public const string Name = "harness";
}
