using Enums;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Controls crosshair visibility based on the player's current state.
    /// </summary>
    public class CrossHair : MonoBehaviour
    {
        /// <summary>
        /// Reference to the player controller that emits state-change events.
        /// </summary>
        [SerializeField] private PlayerController player;

        /// <summary>
        /// Canvas that renders the crosshair UI.
        /// </summary>
        [SerializeField] private Canvas crossHairCanvas;

        /// <summary>
        /// Subscribes to player state updates when this component becomes active.
        /// </summary>
        private void OnEnable()
        {
            player.OnStateUpdated += StateUpdate;
        }

        /// <summary>
        /// Unsubscribes from player state updates when this component becomes inactive.
        /// </summary>
        private void OnDisable()
        {
            player.OnStateUpdated -= StateUpdate;
        }

        /// <summary>
        /// Initializes the crosshair as hidden at startup.
        /// </summary>
        private void Start()
        {
            crossHairCanvas.enabled = false;
        }

        /// <summary>
        /// Updates crosshair visibility based on the supplied player state.
        /// </summary>
        /// <param name="state">The player's new state.</param>
        private void StateUpdate(PlayerState state)
        {
            crossHairCanvas.enabled = state == PlayerState.Aim;
        }
    }
}