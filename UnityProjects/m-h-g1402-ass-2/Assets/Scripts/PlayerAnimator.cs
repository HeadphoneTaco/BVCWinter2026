using UnityEngine;

/// <summary>
/// Synchronizes player movement state with animator parameters and triggers jump animations.
/// </summary>
public class PlayerAnimator : MonoBehaviour
{
    /// <summary>
    /// Reference to the player controller that provides movement and event data.
    /// </summary>
    [SerializeField] private PlayerController playerController;

    /// <summary>
    /// Animator component used to set animation parameters and triggers.
    /// </summary>
    [SerializeField] private Animator anim;

    /// <summary>
    /// Cached player velocity used to drive blend values in the animator.
    /// </summary>
    private Vector3 _playerVelocity;

    /// <summary>
    /// Updates animator parameters every frame based on the player's current movement state.
    /// </summary>
    private void Update()
    {
        anim.SetBool("IsGrounded", playerController.IsGrounded());

        _playerVelocity = playerController.GetPlayerVelocity();
        _playerVelocity.y = 0;

        anim.SetFloat("Velocity", _playerVelocity.sqrMagnitude);
    }

    /// <summary>
    /// Subscribes to player events when this component becomes enabled.
    /// </summary>
    private void OnEnable()
    {
        playerController.OnJumpEvent += OnJump;
    }

    /// <summary>
    /// Unsubscribes from player events when this component becomes disabled.
    /// </summary>
    private void OnDisable()
    {
        playerController.OnJumpEvent -= OnJump;
    }

    /// <summary>
    /// Handles the player jump event by triggering the jump animation.
    /// </summary>
    private void OnJump()
    {
        anim.SetTrigger("Jump");
    }
}