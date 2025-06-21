using UnityEngine;

public class LaserSpell : MonoBehaviour, ISpell
{
    [SerializeField] private SkillData skillData;
    public bool CanUseSkill(bool useSkill = true)
    {
        return SkillManager.Instance.laser.CanUseSkill();
    }

    public float GetCooldown()
    {
        return SkillCooldowns.Instance.laser;
    }

    public SkillData GetSkillData()
    {
        return skillData;
    }
}