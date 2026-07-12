namespace HeadlessBlazor;

/// <summary>
/// The transition phase of a shown toast, used to drive its rendered <c>data-state</c> attribute
/// so CSS can animate the toast in and out.
/// </summary>
internal enum ToastPhase
{
    /// <summary>
    /// The toast has just been mounted and is rendered in its closed state, so the browser can
    /// paint that state before it flips to <see cref="Entered"/> and the enter transition runs.
    /// </summary>
    Entering,

    /// <summary>
    /// The toast is visible. The enter transition (if any) plays as it moves into this phase.
    /// </summary>
    Entered,

    /// <summary>
    /// The toast has been dismissed and is rendered back in its closed state so the exit
    /// transition can play; it is removed from the DOM once the transition duration elapses.
    /// </summary>
    Leaving
}
