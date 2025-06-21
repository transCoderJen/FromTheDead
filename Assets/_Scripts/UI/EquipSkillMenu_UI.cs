using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSkillMenu_UI : MonoBehaviour
{
    [SerializeField] private GameObject equipMenu;
    private SkillData skill;
    [SerializeField] Button buttonToHighlightByDefault;

    public void EnableMenu(bool fromMouseClick = true, Transform menuSpawnTransform = null)
    {
        Debug.Log("Enabling Menu");
        equipMenu.SetActive(true);
        EventSystem.current.sendNavigationEvents = false;

        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(SelectButtonNextFrame());
        }
        

        if (fromMouseClick)
        {
            RepositionMenu();
        }
        else
        {
            equipMenu.transform.position = menuSpawnTransform.position + new Vector3(10, 10, 0);
        }
    }

    private IEnumerator SelectButtonNextFrame()
    {
        // Wait one frame to avoid processing the same Enter keypress
        yield return null;
        EventSystem.current.sendNavigationEvents = true;
        EventSystem.current.SetSelectedGameObject(buttonToHighlightByDefault.gameObject);
    }

    public bool IsMenuClosed()
    {
        return !equipMenu.activeSelf;
    }

    public void SetSkill(SkillData _skill)
    {
        this.skill = _skill;
    }

    private void RepositionMenu()
    {
        Vector3 mousePosition = Input.mousePosition;
        float screenWidth = Screen.width;
        RectTransform menuRect = equipMenu.GetComponent<RectTransform>();

        // Determine if mouse is on left or right side
        bool isLeft = mousePosition.x < screenWidth / 2f;

        // Get menu width (assumes pivot is at center)
        float menuWidth = menuRect.rect.width;

        // Offset so menu doesn't overlap mouse
        float offset = 20f;

        // Calculate new position
        Vector3 newPosition = mousePosition;
        if (isLeft)
        {
            newPosition.x += menuWidth / 2f + offset;
        }
        else
        {
            newPosition.x -= menuWidth / 2f + offset;
        }

        // Convert screen position to canvas position
        Canvas canvas = equipMenu.GetComponentInParent<Canvas>();
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            menuRect.position = newPosition;
        }
        else
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, newPosition, canvas.worldCamera, out pos);
            menuRect.localPosition = pos;
        }
    }

    public void EquipSkill1()
    {
        if (skill.name == UI_InGame.Instance.GetSkill2Name())
        {
            Debug.Log("Skill 1 cannot be the same as Skill 2");
            return;
        }
        UI_InGame.Instance.SetSkill1(skill);
        Cancel();
    }

    public void EquipSkill2()
    {
        if (skill.name == UI_InGame.Instance.GetSkill1Name())
        {
            Debug.Log("Skill 2 cannot be the same as Skill 1");
            return;
        }
        UI_InGame.Instance.SetSkill2(skill);
        Cancel();
    }

    public void Cancel()
    {
        equipMenu.SetActive(false);
    }


}
