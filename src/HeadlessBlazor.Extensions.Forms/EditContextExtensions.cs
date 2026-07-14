using Microsoft.AspNetCore.Components.Forms;

namespace HeadlessBlazor;

/// <summary>
/// Wraps an <see cref="EditContext"/> together with a <see cref="ValidationMessageStore"/> for the
/// model being edited, and exposes helpers for adding custom validation errors and re-running
/// validation. Instances are created and owned by <see cref="HBEditForm{TModel}"/> and passed to its
/// <c>Validation</c> delegate.
/// </summary>
/// <typeparam name="TModel">The type of the model being edited.</typeparam>
public class HBEditContext<TModel> where TModel : notnull
{
    /// <summary>
    /// The model this context validates and tracks edits for.
    /// </summary>
    public TModel Model { get; }

    /// <summary>
    /// The underlying <see cref="EditContext"/> that tracks field state and validation for
    /// <see cref="Model"/>. Created on first access.
    /// </summary>
    public EditContext Context => field ??= new EditContext(Model);

    /// <summary>
    /// The store that holds the custom validation messages added through <see cref="AddError"/> and
    /// <see cref="AddErrors"/>, backed by <see cref="Context"/>. Created on first access.
    /// </summary>
    public ValidationMessageStore ValidationMessageStore => field ??= new ValidationMessageStore(Context);

    /// <summary>
    /// Initializes a new <see cref="HBEditContext{TModel}"/> for the given model.
    /// </summary>
    /// <param name="model">The model to validate and track edits for.</param>
    public HBEditContext(TModel model)
    {
        Model = model;
    }

    /// <summary>
    /// Adds a single validation error message for the specified field. The message is surfaced through
    /// the usual <see cref="ValidationMessage{TValue}"/> / <c>ValidationSummary</c> components.
    /// </summary>
    /// <param name="fieldName">The name of the field the error applies to (typically <c>nameof(model.Property)</c>).</param>
    /// <param name="errorMessage">The error message to display.</param>
    public void AddError(string fieldName, string errorMessage)
    {
        var fieldId = Context.Field(fieldName);
        ValidationMessageStore.Add(fieldId, errorMessage);
    }

    /// <summary>
    /// Adds multiple validation error messages for the specified field.
    /// </summary>
    /// <param name="fieldName">The name of the field the errors apply to (typically <c>nameof(model.Property)</c>).</param>
    /// <param name="errorMessages">The error messages to display.</param>
    public void AddErrors(string fieldName, IEnumerable<string> errorMessages)
    {
        var fieldId = Context.Field(fieldName);
        ValidationMessageStore.Add(fieldId, errorMessages);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the context currently holds no validation messages. This
    /// inspects the existing messages only; it does not re-run validation (use <see cref="Revalidate"/>
    /// for that).
    /// </summary>
    public bool IsValid()
    {
        return !Context.GetValidationMessages().Any();
    }

    /// <summary>
    /// Clears any custom messages previously added to <see cref="ValidationMessageStore"/> and runs
    /// validation against the context.
    /// </summary>
    /// <returns><see langword="true"/> if the model is valid after validation; otherwise <see langword="false"/>.</returns>
    public bool Revalidate()
    {
        ValidationMessageStore.Clear();
        return Context.Validate();
    }

    /// <summary>
    /// Raises the context's validation state changed event so that bound components refresh to reflect
    /// the current messages. Call this after adding custom errors outside of a validation request.
    /// </summary>
    public void NotifyValidationStateChanged()
    {
        Context.NotifyValidationStateChanged();
    }
}
