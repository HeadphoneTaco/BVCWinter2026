using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages player interactions with interactable objects in the game world.
/// Detects when the player enters/exits trigger zones and routes interaction input
/// to objects that implement the <see cref="IInteractable"/> interface.
/// </summary>
public class PlayerInteractor : MonoBehaviour
{
    /// <summary>
    /// Input action that triggers interactions with nearby interactable objects.
    /// </summary>
    [SerializeField] private InputAction interactionInput;

    /// <summary>
    /// Reference to the currently active interactable object the player is hovering over.
    /// </summary>
    private IInteractable _interactable;

    /// <summary>
    /// Temporary storage for an interactable object detected during trigger collision.
    /// Used to validate the component exists before assigning to <see cref="_interactable"/>.
    /// </summary>
    private IInteractable _tempInteractable;

    /// <summary>
    /// Enables the interaction input and subscribes to the performed event when this component is enabled.
    /// </summary>
    private void OnEnable()
    {
        interactionInput.Enable();
        interactionInput.performed += Interact;
    }

    /// <summary>
    /// Unsubscribes from the performed event when this component is disabled.
    /// </summary>
    //TODO:Make these enable/disable matching pattern.
    private void OnDisable()
    {
        interactionInput.performed -= Interact;
    }

    /// <summary>
    /// Handles collision when the player enters a trigger zone containing an interactable object.
    /// Caches the interactable and invokes its hover-in callback.
    /// </summary>
    /// <param name="other">The collider that was entered.</param>
    private void OnTriggerEnter(Collider other)
    {
        _tempInteractable = other.GetComponent<IInteractable>();

        if (_tempInteractable != null)
        {
            _interactable = _tempInteractable;
            _interactable?.OnHoverIn();
        }
    }

    /// <summary>
    /// Handles collision when the player exits a trigger zone.
    /// Invokes the hover-off callback on the current interactable and clears the reference.
    /// </summary>
    /// <param name="other">The collider that was exited.</param>
    private void OnTriggerExit(Collider other)
    {
        _interactable?.OnHoverOff();
        _interactable = null;
    }

    /// <summary>
    /// Handles interaction input by invoking the interaction callback on the current interactable object.
    /// </summary>
    /// <param name="context">Input callback context for the performed action. Not used directly.</param>
    private void Interact(InputAction.CallbackContext context)
    {
        _interactable?.OnInteract();
    }
}