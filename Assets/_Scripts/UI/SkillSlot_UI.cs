using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot_UI : MonoBehaviour
{
    public SpellData spell;
    private Image image;
    PlayerControlller playerControls => PlayerManager.Instance.player.playerControls;
    EquipSkillMenu_UI skillMenu_UI => GetComponentInParent<EquipSkillMenu_UI>();
    SpellMenu_UI spellMenu_UI => GetComponentInParent<SpellMenu_UI>();

    private void Awake()
    {
        if (spell == null)
        {
            Debug.LogError("Spell is not assigned in SpellSlot_UI!");
        }
    }

    void OnEnable()
    {
        image = GetComponent<Image>();
        image.sprite = spell.spellSprite;
    }

    public void AssignSkill(SpellData _skill)
    {
        this.spell = _skill;
    }

    [ContextMenu("EquipSkill1")]
    public void EquipSkill()
    {
        UI_InGame.Instance.SetSkill1(spell);
    }

    public String GetSkillId()
    {
        return spell.itemId;
    }

    void Update()
    {
        bool isPointerOver = IsPointerOverThisUIElement();

        if (isPointerOver)
        {
            int id = spellMenu_UI.FindSpellIndexByName(spell.name);
            Debug.Log($"Hovering over: {spell.name} id: {id}");
            spellMenu_UI.UpdateCurrentSelectedSlot(id);


            if (playerControls.UI.Click.WasPressedThisFrame() && skillMenu_UI.IsMenuClosed())
            {
                OnRightClick();
            }
        }

    }

    private void OnRightClick()
    {
        Debug.Log($"Right-clicked on skill: {spell.name}");
        GetComponentInParent<EquipSkillMenu_UI>().EnableMenu();
        GetComponentInParent<EquipSkillMenu_UI>().SetSkill(spell);
    }
    
    private bool IsPointerOverThisUIElement()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // Only trigger if this GameObject is at the top of the UI stack
        return results.Count > 0 && results[0].gameObject == gameObject;
    }


}
