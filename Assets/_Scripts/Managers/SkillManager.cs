using UnityEngine;
public enum SkillSlot {
    Slot1,
    Slot2
}

public class SkillManager : Singleton<SkillManager>
{
    public DashSkill dash { get; private set; }
    public HealSkill heal { get; private set; }
    public FlashSkill flash { get; private set; }
    public CloneSkill clone {get; private set; }
    public SwordSkill sword { get; private set; }

    // public BlackholeSkill blackhole { get; private set; }
    // public CrystalSkill crystal { get; private set; }
    public ParrySkill parry { get; private set; }
    public LaserSkill laser { get; private set; }
    // public DodgeSkill dodge { get; private set; }
    public SpellData skill1;
    public SpellData skill2;

    [Header("Choose Right Click Action")]
    public bool canUseSword;
    public bool canUseParry;

    private void Update()
    {
        if (canUseSword)
        {
            canUseParry = false;
        }
        if (canUseParry)
        {
            canUseSword = false;
        }
    }
    
    private void Start()
    {
        dash = GetComponent<DashSkill>();
        heal = GetComponent<HealSkill>();
        flash = GetComponent<FlashSkill>();
        clone = GetComponent<CloneSkill>();
        sword = GetComponent<SwordSkill>();
        // blackhole = GetComponent<BlackholeSkill>();
        // crystal = GetComponent<CrystalSkill>();
        parry = GetComponent<ParrySkill>();
        laser = GetComponent<LaserSkill>();


        // dodge = GetComponent<DodgeSkill>();
    }

    public void AssignSkill(SpellData skillData, SkillSlot slot)
    {
        if (slot == SkillSlot.Slot1)
        {
            skill1 = skillData;
        }
        else
        {
            skill2 = skillData;
        }
    }
}
