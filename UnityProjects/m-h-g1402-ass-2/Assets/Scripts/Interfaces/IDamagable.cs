namespace Interfaces
{
    /// <summary>
    /// Defines behavior for objects that can receive damage.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// Handles the damage event when this object is damaged.
        /// </summary>
        /// <param name="amount">The amount of damage to apply.</param>
        void TakeDamage(int amount);
    }
}