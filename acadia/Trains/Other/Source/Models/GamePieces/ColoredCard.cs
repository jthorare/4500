namespace Trains.Models.GamePieces
{
    public class ColoredCard
    {
        public GamePieceColor Color { get; }
        public ColoredCard(GamePieceColor cardColor)
        {
            Color = cardColor;
        }

        public override string ToString()
        {
            return Color.ToString();
        }

        public override bool Equals(object? obj)
        {
            if (obj.GetType() != typeof(ColoredCard))
            {
                return false;
            }
            else
            {
                ColoredCard other = (ColoredCard)obj;
                return this.Color == other.Color;
            }
        }

        public override int GetHashCode()
        {
            return Color.GetHashCode();
        }
    }
}
