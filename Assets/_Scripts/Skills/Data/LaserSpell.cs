using UnityEngine;

public class LaserSpell : MonoBehaviour, ISpell
{
    [SerializeField] private SpellData skillData;
    public bool CanUseSkill(bool useSkill = true)
    {
        return SkillManager.Instance.laser.CanUseSkill();
    }

    public float GetCooldown()
    {
        return SkillCooldowns.Instance.laser;
    }

    public SpellData GetSkillData()
    {
        return skillData;
    }
}