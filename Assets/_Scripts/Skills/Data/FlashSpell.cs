using UnityEngine;

public class FlashSpell : MonoBehaviour, ISpell
{
    [SerializeField] private SkillData skillData;
    public bool CanUseSkill(bool useSkill = true)
    {
        return SkillManager.Instance.flash.CanUseSkill();
    }

    public float GetCooldown()
    {
        return SkillCooldowns.Instance.flash;
    }

    public SkillData GetSkillData()
    {
        return skillData;
    }
}