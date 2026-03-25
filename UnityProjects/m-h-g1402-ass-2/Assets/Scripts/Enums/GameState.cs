namespace Enums
{
    /// <summary>
    /// Represents the current overall state of the game loop.
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// The game is actively running and accepting normal gameplay input.
        /// </summary>
        Playing,

        /// <summary>
        /// Gameplay is temporarily halted.
        /// </summary>
        Paused,

        /// <summary>
        /// The player has met the win condition.
        /// </summary>
        Win,

        /// <summary>
        /// The player has met the loss condition.
        /// </summary>
        Loss
    }
}