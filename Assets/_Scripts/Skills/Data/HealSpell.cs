using UnityEngine;

public class HealSpell : MonoBehaviour, ISpell
{
    [SerializeField] private SpellData skillData;
    public bool CanUseSkill(bool useSkill = true)
    {
        return SkillManager.Instance.heal.CanUseSkill();
    }

    public float GetCooldown()
    {
        return SkillCooldowns.Instance.heal;
    }   

    public SpellData GetSkillData()
    {
        return skillData;
    }
}