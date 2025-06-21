using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using Unity.VisualScripting;

public class SpellMenu_UI : MonoBehaviour
{
    [SerializeField] private GameObject spellSlotParent;
    [SerializeField] private SkillSlot_UI[] skillSlots;
    private int currentSelectedSlot = 0;
    private PlayerControlller playerControls;
    [SerializeField] private TextMeshProUGUI spellName;
    [SerializeField] private TextMeshProUGUI spellInfo;
    [SerializeField] private TextMeshProUGUI spellCooldown;
    [SerializeField] EquipSkillMenu_UI contextMenu;
    [SerializeField] GameObject contextMenuObject;
    private bool isFocused = true;

    private void Start()
    {
        InitializeSkillSlots();
        playerControls = PlayerManager.Instance.player.playerControls;
    }

    private void InitializeSkillSlots()
    {
        if (spellSlotParent != null)
        {
            skillSlots = spellSlotParent.GetComponentsInChildren<SkillSlot_UI>(true);
        }
        else
        {
            Debug.LogError("SpellSlotParent is not assigned!");
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
            playerControls = PlayerManager.Instance.player.playerControls;

        playerControls.UI.Navigate.performed += OnNavigateInput;
        playerControls.UI.Submit.performed += ctx => OnSelect();

        // Hide all except selected
        foreach (SkillSlot_UI skillSlot in skillSlots)
        {
            Image[] parentImages = skillSlot.GetComponentsInParent<Image>(includeInactive: true);
            if (parentImages.Length > 1)
            {
                Image secondParentImage = parentImages[1]; // index 1 = second image up the hierarchy
                Color color = secondParentImage.color;
                color.a = 0f;
                secondParentImage.color = color;
            }
        }

        if (skillSlots.Length > 0)
        {
            Image[] parentImages = skillSlots[currentSelectedSlot].GetComponentsInParent<Image>(includeInactive: true);
            if (parentImages.Length > 1)
            {
                Image secondParentImage = parentImages[1]; // index 1 = second image up the hierarchy
                Color color = secondParentImage.color;
                color.a = 1f;
                secondParentImage.color = color;
            }
            UpdateSkillInfoText();
        }

    }

    void Update()
    {
        if (contextMenuObject.gameObject.activeSelf)
        {
            isFocused = false;
        }
        else
        {
            isFocused = true;
        }
    }

    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.UI.Navigate.performed -= OnNavigateInput;
        }
    }

    private void OnNavigateInput(InputAction.CallbackContext ctx)
    {
        OnNavigate(ctx.ReadValue<Vector2>());
    }

    private void OnNavigate(Vector2 vector2)
    {
        if (!isFocused)
            return;

        // Return if there are no active slots
        bool anyActive = false;
        foreach (var slot in skillSlots)
        {
            if (slot.gameObject.activeInHierarchy)
            {
                anyActive = true;
                break;
            }
        }
        if (!anyActive)
            return;

        int direction = (vector2.x > 0) ? 1 : (vector2.x < 0) ? -1 : 0;

        if (direction != 0)
        {
            int originalSlot = currentSelectedSlot;

            do
            {
                currentSelectedSlot = (currentSelectedSlot + direction + skillSlots.Length) % skillSlots.Length;
            }
            while (!skillSlots[currentSelectedSlot].gameObject.activeInHierarchy && currentSelectedSlot != originalSlot);

            UpdateSkillSlotSelection();
            UpdateSkillInfoText();
        }
    }

    private void UpdateSkillInfoText()
    {
        if (skillSlots[currentSelectedSlot] == null || skillSlots[currentSelectedSlot].skill == null)
            return;

        spellName.text = skillSlots[currentSelectedSlot].skill.name;
        spellInfo.text = skillSlots[currentSelectedSlot].skill.description;
        spellCooldown.text = "Cooldown: " + skillSlots[currentSelectedSlot].skill.cooldown.ToString();
    }

    private void UpdateSkillSlotSelection()
    {
        for (int i = 0; i < skillSlots.Length; i++)
        {
            Image[] parentImages = skillSlots[i].GetComponentsInParent<Image>(includeInactive: true);

            if (parentImages.Length > 1)
            {
                Image secondParentImage = parentImages[1]; // index 1 = second image up the hierarchy

                Color color = secondParentImage.color;
                color.a = (i == currentSelectedSlot) ? 1f : 0f;
                secondParentImage.color = color;
            }
        }
    }

    public void OnSelect()
    {
        if (!isFocused || skillSlots[0].skill == null)
            return;
        contextMenu.EnableMenu(fromMouseClick: false, skillSlots[currentSelectedSlot].gameObject.transform);
        contextMenu.SetSkill(skillSlots[currentSelectedSlot].skill);
    }

    public int FindSpellIndexByName(string spellName)
    {
        for (int i = 0; i < skillSlots.Length; i++)
        {
            if (skillSlots[i].skill != null && skillSlots[i].skill.name.Equals(spellName, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }
        return -1; // Not found
    }

    public void UpdateCurrentSelectedSlot(int newIndex)
    {
        if (newIndex >= 0 && newIndex < skillSlots.Length)
        {
            currentSelectedSlot = newIndex;
            UpdateSkillSlotSelection();
            UpdateSkillInfoText();
        }
        else
        {
            Debug.LogWarning("Invalid index for skill slot selection: " + newIndex);
        }
    }
}
