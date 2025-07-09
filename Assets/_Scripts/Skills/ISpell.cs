public interface ISpell
{
    bool CanUseSkill(bool useSkill = true);
    SpellData GetSkillData();
    float GetCooldown();
}
