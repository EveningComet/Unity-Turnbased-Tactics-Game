namespace TurnbasedGame.Tiles
{
    /// <summary>
    /// The offset coordinate of a hex.
    /// </summary>
    public struct OffsetCoord
    {
        public readonly int Column;
        public readonly int Row;

        public OffsetCoord(int column, int row)
        {
            this.Column = column;
            this.Row = row;
        }
    }
}