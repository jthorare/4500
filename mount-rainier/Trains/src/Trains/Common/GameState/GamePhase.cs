namespace Trains.Common.GameState
{
    /// <summary>
    /// Represents a phase of the game for an <see cref="AbstractGameState"/>
    /// </summary>
    public enum GamePhase
    {
        /// <summary>
        /// The game is being setup as destinations are assigned and chosen.
        /// </summary>
        SetUp,

        /// <summary>
        /// The game is in progress and moves can be made.
        /// </summary>
        InProgress,

        // <summary>
        /// Someone has gone below the minimal amount of cards, giving everyone
        /// else one more turn before the end of the game.
        /// </summary>
        FinalRound,

        /// <summary>
        /// The game is over and this is the final state of the game.
        /// </summary>
        Finished,
    }
}