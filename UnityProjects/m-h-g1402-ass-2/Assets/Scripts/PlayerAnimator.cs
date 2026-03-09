using UnityEngine;
using YeetThePlayer;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator anim;

    Vector3 _playerVelocity;

    void Update()
    {
        anim.SetBool("IsGrounded", playerController.IsGrounded());

        _playerVelocity = playerController.GetPlayerVelocity();
        _playerVelocity.y = 0;
        
        anim.SetFloat("Velocity", _playerVelocity.sqrMagnitude);
    }

    //Subscribe to Events
    void OnEnable()
    {
        playerController.OnJumpEvent += OnJump;
    }
    
    //Unsubscribe to Events
    void OnDisable()
    {
        playerController.OnJumpEvent -= OnJump;
    }

    private void OnJump()
    {
        anim.SetTrigger("Jump");
    }
}