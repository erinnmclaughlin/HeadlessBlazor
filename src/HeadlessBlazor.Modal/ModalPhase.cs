namespace HeadlessBlazor;

/// <summary>
/// The transition phase of an open modal, used to drive its rendered <c>data-state</c> attribute
/// so CSS can animate the modal in and out.
/// </summary>
internal enum ModalPhase
{
    /// <summary>
    /// The modal has just been mounted and is rendered in its closed state, so the browser can
    /// paint that state before it flips to <see cref="Entered"/> and the enter transition runs.
    /// </summary>
    Entering,

    /// <summary>
    /// The modal is open. The enter transition (if any) plays as it moves into this phase.
    /// </summary>
    Entered,

    /// <summary>
    /// The modal has been closed and is rendered back in its closed state so the exit transition
    /// can play; it is removed from the DOM once the transition duration elapses.
    /// </summary>
    Leaving
}
