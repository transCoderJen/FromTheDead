using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillInventory : Singleton<SkillInventory>
{
    [SerializeField] private GameObject spellSlotParent;
    [SerializeField] private List<SkillSlot_UI> skillSlots;

    private void Start()
    {
        InitializeSkillSlots();
    }

    private void InitializeSkillSlots()
    {
        skillSlots = new List<SkillSlot_UI>(spellSlotParent.GetComponentsInChildren<SkillSlot_UI>(true));
    }

    public void AddSkillToInventory(SkillData skillData)
    {
        Debug.Log("Adding skill to inventory: " + skillData.name);
        foreach (SkillSlot_UI skillSlot in skillSlots)
        {
            if (!skillSlot.gameObject.activeSelf)
            {


                if (skillSlot.transform.parent != null)
                {
                    skillSlot.transform.parent.gameObject.SetActive(true);
                    skillSlot.gameObject.SetActive(true);
                }
                skillSlot.AssignSkill(skillData);
                break;
            }
        }
    }
}
