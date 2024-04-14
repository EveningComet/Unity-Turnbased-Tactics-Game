namespace TurnbasedGame.Tiles
{
    /// <summary>
    /// A hex's coordinates as a cube.
    /// </summary>
    public struct CubeCoord
    {
        // Q + R + S = 0. Must equal zero.
        // S = -(Q + R)
        public int X;
        public int Y;
        public int Z;

        /// <summary>
        /// Return a <see cref="CubeCoord"/> with coordinates (0, 0, 0).
        /// </summary>
        public static CubeCoord Zero { get{return new CubeCoord(0, 0, 0); } }

        public static CubeCoord[] CubeDirections = {
            new CubeCoord(1, -1, 0), // Right
            new CubeCoord(1, 0, -1), // Top Right
            new CubeCoord(0, 1, -1), // Top Left
            new CubeCoord(-1, 1, 0), // Left
            new CubeCoord(-1, 0, 1), // Bottom Left
            new CubeCoord(0, -1, 1)  // Bottom Right
        };

        public CubeCoord(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Z = -x - y;
        }

        public CubeCoord(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static CubeCoord Add(CubeCoord a, CubeCoord b)
        {
            return new CubeCoord(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
    }
}