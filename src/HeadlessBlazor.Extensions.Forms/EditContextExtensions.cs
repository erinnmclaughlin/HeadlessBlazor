using Microsoft.AspNetCore.Components.Forms;

namespace HeadlessBlazor;

public class HBEditContext<TModel> where TModel : notnull
{
    public TModel Model { get; }

    internal EditContext Context => field ??= new EditContext(Model);
    internal ValidationMessageStore ValidationMessageStore => field ??= new ValidationMessageStore(Context);
    
    public HBEditContext(TModel model)
    {
        Model = model;
    }

    public void AddError(string fieldName, string errorMessage)
    {
        var fieldId = Context.Field(fieldName);
        ValidationMessageStore.Add(fieldId, errorMessage);
    }

    public void AddErrors(string fieldName, IEnumerable<string> errorMessages)
    {
        var fieldId = Context.Field(fieldName);
        ValidationMessageStore.Add(fieldId, errorMessages);
    }

    public bool Revalidate()
    {
        ValidationMessageStore.Clear();
        return Context.Validate();
    }

    public void NotifyValidationStateChanged()
    {
        Context.NotifyValidationStateChanged();
    }
}
