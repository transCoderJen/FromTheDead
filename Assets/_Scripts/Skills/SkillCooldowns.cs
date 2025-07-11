using UnityEngine;

public class SkillCooldowns : Singleton<SkillCooldowns>
{
    [SerializeField] private SpellData flashSkillData;
    [SerializeField] private SpellData laserSkill;
    [SerializeField] private SpellData healSkill;
    [HideInInspector] public float flash;
    [HideInInspector] public float dash = 3f;
    [HideInInspector] public float heal;
    [HideInInspector] public float laser;

    void Start()
    {
        flash = flashSkillData.cooldown;
        laser = laserSkill.cooldown;
        heal = healSkill.cooldown;
    }
}