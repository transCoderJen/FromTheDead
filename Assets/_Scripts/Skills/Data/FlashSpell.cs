using UnityEditor;
using UnityEngine;

public class FlashSpell : MonoBehaviour, ISpell
{
    [SerializeField] private SpellData skillData;
    public bool CanUseSkill(bool useSkill = true)
    {
        return SkillManager.Instance.flash.CanUseSkill();
    }

    public float GetCooldown()
    {
        return SkillCooldowns.Instance.flash;
    }

    public SpellData GetSkillData()
    {
        return skillData;
    }
}