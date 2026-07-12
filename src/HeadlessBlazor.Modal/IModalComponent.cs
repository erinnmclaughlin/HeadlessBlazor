namespace HeadlessBlazor;

/// <summary>
/// Implemented by a component used as a modal body to declare the type of value it resolves with
/// when closed. Tying the result type to the component this way lets
/// <see cref="IModalService.ShowAsync{TComponent, TResult}(ModalOptions?)"/> infer and enforce
/// <typeparamref name="TResult"/> at compile time, and lets the body close itself through a
/// strongly-typed <see cref="IModalInstance{TResult}"/> - so a result-type mismatch between the
/// caller and the component is impossible rather than a runtime cast.
/// </summary>
/// <typeparam name="TResult">The type of value produced when the modal is closed with a result.</typeparam>
public interface IModalComponent<TResult> : IComponent;
