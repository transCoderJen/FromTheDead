public interface ISpell
{
    bool CanUseSkill(bool useSkill = true);
    SkillData GetSkillData();
    float GetCooldown();
}
