namespace TurnbasedGame.AI
{
    /// <summary>
    /// Stores a <see cref="AIContext"/> alongside a score.
    /// </summary>
    public struct ContextScore
    {
        public AIContext StoredContext { get; set; }
        public float Score { get; set; }
    }
}
