using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class SpellInventory : Singleton<SpellInventory>, ISaveManager
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

    public void AddSkillToInventory(SpellData skillData)
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

    public void LoadData(GameData _data)
    {
        foreach (var loadedSpellId in _data.spells)
        {
            foreach (var spell in Inventory.instance.spellDataBase)
            {
                if (spell != null && spell.itemId == loadedSpellId)
                {
                    AddSkillToInventory(spell as SpellData);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.spells.Clear();

        foreach (SkillSlot_UI skillSlot in skillSlots)
        {
            if (skillSlot.spell == null)
            {
                continue;
            }
            _data.spells.Add(skillSlot.GetSkillId());
        }
    }
}
