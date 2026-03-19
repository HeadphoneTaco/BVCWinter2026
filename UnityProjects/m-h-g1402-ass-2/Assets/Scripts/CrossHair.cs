using Enums;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
       [SerializeField] private PlayerController player;
       [SerializeField] private Canvas crossHairCanvas;

    private void OnEnable()
    {
           player.OnStateUpdated += StateUpdate;
    }
    
    void OnDisable()
    {
        player.OnStateUpdated -= StateUpdate;
    }

    private void Start()
    {
        crossHairCanvas.enabled = false;
    }

    void StateUpdate(PlayerState state)
    {
        crossHairCanvas.enabled = state == PlayerState.AIM;
    }
}
