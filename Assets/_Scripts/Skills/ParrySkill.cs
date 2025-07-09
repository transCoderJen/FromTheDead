public class ParrySkill : Skill
{
    public bool parryEquipped;

    public override bool CanUseSkill(bool _useSkill = true)
    {
        return base.CanUseSkill(_useSkill);
    }
    
}