using Trains.Common.Map;

namespace Trains.Player
{
    /// <summary>
    /// The enumeration of different types of player responses
    /// </summary>
    public enum ResponseType
    {
        ClaimConnection,
        DrawCards
    }

    /// <summary>
    /// A Response that a player can give to how they may want to make a move. 
    /// </summary>
    public class PlayerResponse
    {
        /// <summary>
        /// Return a <see cref="PlayerResponse"/> that represent that a user would like to claim the given connection.
        /// </summary>
        /// <param name="connection">The connection that the player would like to claim.</param>
        /// <returns>A representative <see cref="PlayerResponse"/>.</returns>
        public static PlayerResponse ClaimConnection(Connection connection) =>
            new(ResponseType.ClaimConnection, connection);

        /// <summary>
        /// Return a <see cref="PlayerResponse"/> that represent that a user would like to request more cards.
        /// </summary>
        /// <returns>A representative <see cref="PlayerResponse"/>.</returns>
        public static PlayerResponse DrawCard() => new(ResponseType.DrawCards);

        /// <summary>
        /// The type of response that a player is making.
        /// </summary>
        public ResponseType ResponseType { get; }

        /// <summary>
        /// Get the requested connection that a user wants to claim.
        /// </summary>
        /// <remarks>
        /// User should validate <see cref="ResponseType"/> before getting this property. Property will be null if
        /// <see cref="ResponseType"/> is incorrect.
        /// </remarks>
        public Connection? RequestedConnectionClaim { get; }

        /// <summary>
        /// Private constructor sets required fields and limits construction to static builders only.
        /// </summary>
        /// <param name="responseType"></param>
        /// <param name="connection"></param>
        private PlayerResponse(ResponseType responseType, Connection? connection = null)
        {
            ResponseType = responseType;
            RequestedConnectionClaim = connection;
        }

        public override bool Equals(object? obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                PlayerResponse p = (PlayerResponse)obj;
                bool rv = p.ResponseType == this.ResponseType;
                if (rv && p.ResponseType == ResponseType.ClaimConnection)
                {
                    rv = p.RequestedConnectionClaim == this.RequestedConnectionClaim;
                }
                return rv;
            }
        }
    }
}