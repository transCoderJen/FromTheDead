using UnityEngine;

public class HealSkill : Skill
{
    public int healAmount;

    public override bool CanUseSkill(bool _useSkill = true)
    {
        return base.CanUseSkill(_useSkill);
    }

    public override void UseSkill()
    {
        base.UseSkill();

        player.stateMachine.ChangeState(player.healState);
        player.stats.IncreaseHealthBy(healAmount);

    }

    
}