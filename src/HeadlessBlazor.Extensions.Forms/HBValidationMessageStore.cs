using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;

namespace HeadlessBlazor;

public class HBValidationMessageStore<TModel> where TModel : notnull
{
    private readonly EditContext _editContext;

    public ValidationMessageStore Store => field ??= new ValidationMessageStore(_editContext);

    public HBValidationMessageStore(EditContext editContext)
    {
        _editContext = editContext;
    }

    public void Add(Expression<Func<TModel, object>> fieldExpression, string errorMessage)
    {
        var fieldName = GetFieldName(fieldExpression);
        Add(fieldName, errorMessage);
    }

    public void Add(string fieldName, string errorMessage)
    {
        var fieldId = _editContext.Field(fieldName);
        Add(fieldId, errorMessage);
    }

    public void Add(FieldIdentifier fieldIdentifier, string errorMessage)
    {
        Store.Add(fieldIdentifier, errorMessage);
    }

    public void AddRange(Expression<Func<TModel, object>> fieldExpression, IEnumerable<string> errorMessages)
    {
        var fieldName = GetFieldName(fieldExpression);
        AddRange(fieldName, errorMessages);
    }

    public void AddRange(string fieldName, IEnumerable<string> errorMessages)
    {
        var fieldId = _editContext.Field(fieldName);
        AddRange(fieldId, errorMessages);
    }

    public void AddRange(FieldIdentifier fieldIdentifier, IEnumerable<string> errorMessages)
    {
        Store.Add(fieldIdentifier, errorMessages);
    }

    public void Clear()
    {
        Store.Clear();
    }

    public void Clear(Expression<Func<TModel, object>> fieldExpression)
    {
        var fieldName = GetFieldName(fieldExpression);
        Clear(fieldName);
    }

    public void Clear(string fieldName)
    {
        var fieldId = _editContext.Field(fieldName);
        Clear(fieldId);
    }

    public void Clear(FieldIdentifier fieldIdentifier)
    {
        Store.Clear(fieldIdentifier);
    }

    private static string GetFieldName(Expression<Func<TModel, object>> fieldExpression)
    {
        if (fieldExpression.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }
        else if (fieldExpression.Body is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression memberOperand)
        {
            return memberOperand.Member.Name;
        }
        else
        {
            throw new ArgumentException("Invalid field expression", nameof(fieldExpression));
        }
    }
}
