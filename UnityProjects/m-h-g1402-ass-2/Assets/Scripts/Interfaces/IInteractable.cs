namespace Interfaces
{
    /// <summary>
    /// Defines behavior for objects that can be hovered and interacted with.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Called when an interactor starts hovering over this object.
        /// </summary>
        public void OnHoverIn();

        /// <summary>
        /// Called when an interactor performs an interaction with this object.
        /// </summary>
        public void OnInteract();

        /// <summary>
        /// Called when an interactor stops hovering over this object.
        /// </summary>
        public void OnHoverOff();
    }
}