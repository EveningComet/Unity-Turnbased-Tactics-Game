namespace TurnbasedGame.AI
{
    /// <summary>
    /// Describes what an <see cref="AIAction"/> is supposed to do.
    /// The AI uses this to help execute the chosen action.
    /// </summary>
    public enum AIActionTypes
    {
        GenericAttack,
        UseHealingAbility,
        UseDamagingAbility,
        UseBuffAbility,
        UseDebuffAbility
    }
}
