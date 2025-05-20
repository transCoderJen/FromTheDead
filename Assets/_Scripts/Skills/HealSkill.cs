using UnityEngine;

public class HealSkill : Skill
{
    [Range(0, 1)]
    public float healPercent = 0.2f;

    public override bool CanUseSkill(bool _useSkill = true)
    {
        return base.CanUseSkill(_useSkill);
    }

    public override void UseSkill()
    {
        base.UseSkill();

        player.stateMachine.ChangeState(player.healState);
        player.stats.IncreaseHealthBy((int)(player.stats.maxHealth.GetValue() * healPercent));

    }

    
}