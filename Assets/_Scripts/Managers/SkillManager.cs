using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    public DashSkill dash {get; private set; }
    public HealSkill heal { get; private set; }
    public FlashSkill flash { get; private set; }
    // public CloneSkill clone {get; private set; }
    // public SwordSkill sword { get; private set; }

    // public BlackholeSkill blackhole { get; private set; }
    // public CrystalSkill crystal { get; private set; }
    public ParrySkill parry { get; private set; }
    // public DodgeSkill dodge { get; private set; }

    private void Start()
    {
        dash = GetComponent<DashSkill>();
        heal = GetComponent<HealSkill>();
        flash = GetComponent<FlashSkill>();
        // clone = GetComponent<CloneSkill>();
        // sword = GetComponent<SwordSkill>();
        // blackhole = GetComponent<BlackholeSkill>();
        // crystal = GetComponent<CrystalSkill>();
        parry = GetComponent<ParrySkill>();
        // dodge = GetComponent<DodgeSkill>();
    }

}
