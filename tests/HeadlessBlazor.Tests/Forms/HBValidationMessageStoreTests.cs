using Microsoft.AspNetCore.Components.Forms;

namespace HeadlessBlazor.Tests.Forms;

/// <summary>
/// Layer 1 (pure logic) tests for <see cref="HBValidationMessageStore{TModel}"/>: adding and
/// clearing messages by member-access expression, by field name, and for nested members.
/// </summary>
public class HBValidationMessageStoreTests
{
    private sealed class Address
    {
        public string? City { get; set; }
    }

    private sealed class Person
    {
        public string? Name { get; set; }
        public Address Address { get; set; } = new();
    }

    [Fact]
    public void Add_ByExpression_AddsAMessageForThatField()
    {
        var editContext = new HBEditContext<Person>(new Person());

        editContext.ValidationErrors.Add(x => x.Name, "Name is required.");

        Assert.Contains("Name is required.", editContext.Context.GetValidationMessages());
    }

    [Fact]
    public void Add_ByFieldName_AddsAMessageForThatField()
    {
        var editContext = new HBEditContext<Person>(new Person());

        editContext.ValidationErrors.Add(nameof(Person.Name), "Name is required.");

        Assert.Contains("Name is required.", editContext.Context.GetValidationMessages());
    }

    [Fact]
    public void Add_ForNestedMember_ResolvesTheNestedOwner()
    {
        var person = new Person();
        var editContext = new HBEditContext<Person>(person);

        editContext.ValidationErrors.Add(x => x.Address.City, "City is required.");

        var fieldMessages = editContext.Context.GetValidationMessages(new FieldIdentifier(person.Address, nameof(Address.City)));
        Assert.Contains("City is required.", fieldMessages);
    }

    [Fact]
    public void AddRange_AddsAllMessages()
    {
        var editContext = new HBEditContext<Person>(new Person());

        editContext.ValidationErrors.AddRange(x => x.Name, ["Too short.", "Must not contain digits."]);

        Assert.Equal(2, editContext.Context.GetValidationMessages().Count());
    }

    [Fact]
    public void Clear_RemovesPreviouslyAddedMessages()
    {
        var editContext = new HBEditContext<Person>(new Person());
        editContext.ValidationErrors.Add(x => x.Name, "Name is required.");

        editContext.ValidationErrors.Clear();

        Assert.Empty(editContext.Context.GetValidationMessages());
    }

    [Fact]
    public void Clear_ByExpression_OnlyRemovesMessagesForThatField()
    {
        var editContext = new HBEditContext<Person>(new Person());
        editContext.ValidationErrors.Add(x => x.Name, "Name is required.");
        editContext.ValidationErrors.Add(x => x.Address.City, "City is required.");

        editContext.ValidationErrors.Clear(x => x.Name);

        Assert.Single(editContext.Context.GetValidationMessages());
    }

    [Fact]
    public void Add_WithUnsupportedExpression_ThrowsArgumentException()
    {
        var editContext = new HBEditContext<Person>(new Person());

        // A method call (rather than a member access) is not a supported field expression shape.
        Assert.Throws<ArgumentException>(() => editContext.ValidationErrors.Add(x => x.ToString()!, "Invalid."));
    }
}
