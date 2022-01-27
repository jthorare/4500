using System;
namespace Trains.Common.GameState
{
    /// <summary>
    /// Represents a snapshot of the game. 
    /// </summary>
    public abstract class AbstractGameState
    {
        /// <summary>
        /// The current phase of the game.
        /// </summary>
        public GamePhase Phase { get; internal set; }

        /// <summary>
        /// The number of players in the game.
        /// </summary>
        public uint NumberOfPlayers { get; }

        
        public AbstractGameState(GamePhase gamePhase, uint numberOfPlayers)
        {
            Phase = gamePhase;
            NumberOfPlayers = numberOfPlayers;
        }
    }
}