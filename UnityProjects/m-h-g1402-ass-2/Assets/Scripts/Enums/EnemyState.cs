namespace Enums
{
    /// <summary>
    /// Represents the high-level behavior states an enemy can be in.
    /// </summary>
    public enum EnemyState
    {
        /// <summary>
        /// Enemy is inactive or waiting.
        /// </summary>
        IDLE,

        /// <summary>
        /// Enemy is moving between patrol points.
        /// </summary>
        PATROL,

        /// <summary>
        /// Enemy is pursuing a detected target.
        /// </summary>
        CHASE,

        /// <summary>
        /// Enemy is actively performing an attack.
        /// </summary>
        ATTACK
    }
}
