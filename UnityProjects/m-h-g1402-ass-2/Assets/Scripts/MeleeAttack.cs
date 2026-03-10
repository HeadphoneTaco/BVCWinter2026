using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private InputAction meleeInput;
    [SerializeField] private Animator animator;
    [SerializeField] private MeleeHitbox hitbox;

    private static readonly int MeleeParam = Animator.StringToHash("Melee");
    private static readonly int MeleeStateHash = Animator.StringToHash("Melee_Attack");

    private bool _isAttacking;

    private void OnEnable()
    {
        meleeInput.Enable();
        meleeInput.performed += OnMeleePerformed;
    }

    private void OnDisable()
    {
        meleeInput.performed -= OnMeleePerformed;
    }

    private void OnMeleePerformed(InputAction.CallbackContext context)
    {
        animator.SetTrigger(MeleeParam);
    }

    private void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool inMeleeState = stateInfo.shortNameHash == MeleeStateHash;

        if (inMeleeState && !_isAttacking)
        {
            _isAttacking = true;
            hitbox.EnableHitbox();
        }
        else if (!inMeleeState && _isAttacking)
        {
            _isAttacking = false;
            hitbox.DisableHitbox();
        }
    }
}