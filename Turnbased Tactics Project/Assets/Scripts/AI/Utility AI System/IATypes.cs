namespace TurnbasedGame.AI
{
    /// <summary>
    /// Tag for describing what a <see cref="Consideration"/> does.
    /// </summary>
    public enum IATypes
    {
        DistanceBetweenUnits,
        ApproachTarget,
        AvoidTarget,
        GetHealthOfTarget,
        AttackTarget,
        ConsiderHealingOther,
        ConsiderHealingSelf,
        ConsiderUsingBuff,
        ConsiderUsingDebuff,
        ConsiderUsingDamageAbility
    }
}
