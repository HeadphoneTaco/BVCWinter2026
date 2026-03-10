using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private MeleeHitbox hitbox;

    private static readonly int MeleeParam = Animator.StringToHash("Melee");

    private void OnMeleeAttack(InputValue value)
    {
        if (value.isPressed)
        {
            animator.SetTrigger(MeleeParam);
        }
    }
}