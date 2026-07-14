using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;

namespace HeadlessBlazor;

/// <summary>
/// A strongly-typed wrapper around <see cref="ValidationMessageStore"/> that lets callers identify
/// fields on <typeparamref name="TModel"/> via member-access expressions instead of raw field names.
/// Owned by <see cref="HBEditContext{TModel}"/>.
/// </summary>
/// <typeparam name="TModel">The type of the model being validated.</typeparam>
public class HBValidationMessageStore<TModel> where TModel : notnull
{
    private readonly HBEditContext<TModel> _editContext;

    /// <summary>
    /// The underlying <see cref="ValidationMessageStore"/>. Created on first access.
    /// </summary>
    public ValidationMessageStore Store => field ??= new ValidationMessageStore(_editContext.Context);

    /// <summary>
    /// Initializes a new <see cref="HBValidationMessageStore{TModel}"/> backed by the given edit context.
    /// </summary>
    /// <param name="editContext">The edit context to add and clear validation messages against.</param>
    public HBValidationMessageStore(HBEditContext<TModel> editContext)
    {
        _editContext = editContext;
    }

    /// <summary>
    /// Adds a validation message for the field identified by <paramref name="fieldExpression"/>.
    /// </summary>
    /// <param name="fieldExpression">A member-access expression identifying the field, e.g. <c>m => m.Name</c>.</param>
    /// <param name="errorMessage">The validation message to add.</param>
    public void Add<TProperty>(Expression<Func<TModel, TProperty>> fieldExpression, string errorMessage)
    {
        var fieldIdentifier = GetFieldIdentifier(fieldExpression);
        Add(fieldIdentifier, errorMessage);
    }

    /// <summary>
    /// Adds a validation message for the field with the given name.
    /// </summary>
    /// <param name="fieldName">The name of the field on <typeparamref name="TModel"/>.</param>
    /// <param name="errorMessage">The validation message to add.</param>
    public void Add(string fieldName, string errorMessage)
    {
        var fieldId = _editContext.Context.Field(fieldName);
        Add(fieldId, errorMessage);
    }

    /// <summary>
    /// Adds a validation message for the given field.
    /// </summary>
    /// <param name="fieldIdentifier">The field to add the message for.</param>
    /// <param name="errorMessage">The validation message to add.</param>
    public void Add(FieldIdentifier fieldIdentifier, string errorMessage)
    {
        Store.Add(fieldIdentifier, errorMessage);
    }

    /// <summary>
    /// Adds multiple validation messages for the field identified by <paramref name="fieldExpression"/>.
    /// </summary>
    /// <param name="fieldExpression">A member-access expression identifying the field, e.g. <c>m => m.Name</c>.</param>
    /// <param name="errorMessages">The validation messages to add.</param>
    public void AddRange<TProperty>(Expression<Func<TModel, TProperty>> fieldExpression, IEnumerable<string> errorMessages)
    {
        var fieldIdentifier = GetFieldIdentifier(fieldExpression);
        AddRange(fieldIdentifier, errorMessages);
    }

    /// <summary>
    /// Adds multiple validation messages for the field with the given name.
    /// </summary>
    /// <param name="fieldName">The name of the field on <typeparamref name="TModel"/>.</param>
    /// <param name="errorMessages">The validation messages to add.</param>
    public void AddRange(string fieldName, IEnumerable<string> errorMessages)
    {
        var fieldId = _editContext.Context.Field(fieldName);
        AddRange(fieldId, errorMessages);
    }

    /// <summary>
    /// Adds multiple validation messages for the given field.
    /// </summary>
    /// <param name="fieldIdentifier">The field to add the messages for.</param>
    /// <param name="errorMessages">The validation messages to add.</param>
    public void AddRange(FieldIdentifier fieldIdentifier, IEnumerable<string> errorMessages)
    {
        Store.Add(fieldIdentifier, errorMessages);
    }

    /// <summary>
    /// Clears all validation messages in the store.
    /// </summary>
    public void Clear()
    {
        Store.Clear();
    }

    /// <summary>
    /// Clears validation messages for the field identified by <paramref name="fieldExpression"/>.
    /// </summary>
    /// <param name="fieldExpression">A member-access expression identifying the field, e.g. <c>m => m.Name</c>.</param>
    public void Clear<TProperty>(Expression<Func<TModel, TProperty>> fieldExpression)
    {
        var fieldIdentifier = GetFieldIdentifier(fieldExpression);
        Clear(fieldIdentifier);
    }

    /// <summary>
    /// Clears validation messages for the field with the given name.
    /// </summary>
    /// <param name="fieldName">The name of the field on <typeparamref name="TModel"/>.</param>
    public void Clear(string fieldName)
    {
        var fieldId = _editContext.Context.Field(fieldName);
        Clear(fieldId);
    }

    /// <summary>
    /// Clears validation messages for the given field.
    /// </summary>
    /// <param name="fieldIdentifier">The field to clear messages for.</param>
    public void Clear(FieldIdentifier fieldIdentifier)
    {
        Store.Clear(fieldIdentifier);
    }

    private FieldIdentifier GetFieldIdentifier<TProperty>(Expression<Func<TModel, TProperty>> fieldExpression)
    {
        var fieldName = HBExpressionUtils.GetFieldName(fieldExpression);

        if (fieldName is null)
        {
            throw new ArgumentException(
                $"'{fieldExpression}' is not a supported member-access expression, e.g. 'm => m.Name' or 'm => m.Address.City'.",
                nameof(fieldExpression));
        }

        var model = _editContext.Model;
        var fieldOwner = HBExpressionUtils.GetFieldOwner(model, fieldExpression);

        if (fieldOwner is null)
        {
            throw new InvalidOperationException(
                $"Cannot add a validation message for '{fieldExpression}': an intermediate member evaluated to null before reaching '{fieldName}'.");
        }

        return new FieldIdentifier(fieldOwner, fieldName);
    }
}
