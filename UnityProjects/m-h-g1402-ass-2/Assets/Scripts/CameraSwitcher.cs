using Enums;
using Unity.Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineCamera explorecamera;
    [SerializeField] private CinemachineCamera aimCamera;
    [SerializeField] private PlayerController playerController;

  #region Unity Functions
    void OnEnable()
    {
        playerController.OnStateUpdated += SwitchCamera;
    }

    void OnDisable()
    {
        playerController.OnStateUpdated -= SwitchCamera;
    }
  #endregion  

    private void SwitchCamera(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.EXPLORE:
                //Do Something
                explorecamera.Prioritize();
                break;
            
            case PlayerState.AIM:
                //Do something else
                aimCamera.Prioritize();
                break;
            
            default:
                //Nothing to  do here
                break;
        }
    }
}