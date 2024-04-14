namespace TurnbasedGame.Abilities
{
    /// <summary>
    /// Tag that describes what an <see cref="Ability"/> does. This is useful for helping the AI make decisions.
    /// </summary>
    public enum AbilityTypes
    {
        Buff,
        Debuff,
        Damage,
        Heal
    }
}