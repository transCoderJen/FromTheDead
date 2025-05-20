using UnityEngine;

public class FlashSkill : Skill
{
    public override bool CanUseSkill(bool _useSkill = true)
    {
        return base.CanUseSkill(_useSkill);
    }

    public override void UseSkill()
    {
        base.UseSkill();

        player.stateMachine.ChangeState(player.flashState);
    }
}