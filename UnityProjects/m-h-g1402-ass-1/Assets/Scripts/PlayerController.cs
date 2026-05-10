using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpforce = 15f;
   
    [SerializeField] private InputManager inputManager;
   
    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Vector2 startPointOffset;
    [SerializeField] private float groundCheckDistance;
    
    
    private float _horizontalInput;
    private Rigidbody2D _playerRb;
    private bool _isOnGround;

    private void Awake()
    {
        _playerRb = GetComponent<Rigidbody2D>(); //get the Rigidbody2D component
    }
    void OnEnable()
    {
        inputManager.OnJump += HandleJumpInput; //subscribe to jump action
        inputManager.OnMove += HandleMoveInput; //subscribe to horizontal movement action

    }
    void OnDisable()
    {
        inputManager.OnJump -= HandleJumpInput; //unsubscribe to jump action
        inputManager.OnMove -= HandleMoveInput; //unsubscribe to horizontal movement action
    }

    void HandleJumpInput()
    {
        // apply the jump force
        if (_playerRb == null) return;
        if (_isOnGround)
        {
            _playerRb.AddForceY(jumpforce, ForceMode2D.Impulse);
        }
    }

    void HandleMoveInput(float value)
    {
        _horizontalInput = value; //store the horizontal input value
    }

    void FixedUpdate()
    {
        HandleMovement(); //handle movement in FixedUpdate for consistent physics updates
        GroundCheck();
    }
    
    void HandleMovement()
    {
        if (_playerRb == null) return;
        _playerRb.linearVelocityX = moveSpeed * _horizontalInput; //set the horizontal velocity based on input
    }
    
    void GroundCheck()
    {
        _isOnGround= Physics2D.Raycast((Vector2)transform.position + startPointOffset, Vector2.down, groundCheckDistance, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine((Vector2)transform.position + startPointOffset,
            (Vector2)transform.position + startPointOffset + Vector2.down * groundCheckDistance,
            _isOnGround ? Color.green : Color.red);
    }
}
