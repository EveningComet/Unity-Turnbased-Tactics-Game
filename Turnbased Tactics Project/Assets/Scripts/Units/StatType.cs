namespace TurnbasedGame.Units
{
    /// <summary>
    /// The various stats that can exist on a <see cref="Unit"/>.
    /// </summary>
    /// <remarks>
    /// We're using this so that we can store a <see cref="Unit"/>'s stats inside something like an array,
    /// so that we can easily index the stats based on the corresponding enum position. This way, we don't have to
    /// worry about keeping track of where all the stat variables are.
    /// </remarks>
    public enum StatTypes
    {
        MAXHP,
        CURRENTHP,
        MAXMOVEMENTPOINTS,
        CURRENTMOVEMENTPOINTSREMAINING,
        STRENGTH,
        DEFENSE,
        Count // How many stats there are. SHOULD ALWAYS BE LAST.
    }
}
