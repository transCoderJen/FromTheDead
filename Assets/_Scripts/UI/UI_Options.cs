using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UI_Options : MonoBehaviour
{
    [SerializeField] private Toggle showPopupTextToggle;
    [SerializeField] private GameObject optionsGroup;
    [SerializeField] private GameObject debugOptionsGroup;
    [SerializeField] private GameObject debugOptionsOnGroup;  // The options to show when debug mode is enabled
    [SerializeField] private GameObject teleportWindow;
    [SerializeField] private GameObject enemyWindow;
    [SerializeField] private Toggle parryToggle;
    [SerializeField] private Toggle swordThrowToggle;
    [SerializeField] private TMP_Dropdown swordTypeDropdown;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject sobberPrefab;
    [SerializeField] private GameObject impPrefab;
    [SerializeField] private GameObject beggarPrefab;
    [SerializeField] private GameObject hyrdaPrefab;

    void OnEnable()
    {
        showPopupTextToggle.isOn = UI.Instance.IsPopupTextEnabled();
    }

    public void OnClickOptionsButton()
    {
        optionsGroup.SetActive(true);
        debugOptionsGroup.SetActive(false);
    }

    public void OnClickDebugOptionsButton()
    {
        optionsGroup.SetActive(false);
        debugOptionsGroup.SetActive(true);
    }

    public void onClickTeleportButton()
    {
        if (DebugManager.Instance.isDebugModeEnabled())
            teleportWindow.SetActive(!teleportWindow.activeSelf);
    }

    public void OnClickSaveAndExitButton()
    {
        SaveManager.instance.SaveGame();
        Debug.Log("Game saved successfully.");
        //TODO Enable Quit Option
        // Application.Quit();
    }

#region  Spawn Enemies
    public void onClickSpawnEnemyButton()
    {
        if (DebugManager.Instance.isDebugModeEnabled())
            enemyWindow.SetActive(!enemyWindow.activeSelf);
    }

    public void OnClickSpawnSobber()
    {
        if (DebugManager.Instance.isDebugModeEnabled())
        {

            Instantiate(sobberPrefab, GetSpawnPosition(randomizeY: true), Quaternion.identity);
        }
    }

    public void OnClickSpawnImp()
    {
        if (DebugManager.Instance.isDebugModeEnabled())
        {
            Instantiate(impPrefab, GetSpawnPosition(), Quaternion.identity);
        }
    }

    public void OnClickSpawnBeggar()
    {
        if (DebugManager.Instance.isDebugModeEnabled())
        {
            Instantiate(beggarPrefab, GetSpawnPosition(), Quaternion.identity);
        }
    }

    public void OnClickSpawnHydra()
    {
        if (DebugManager.Instance.isDebugModeEnabled())
        {
            Instantiate(hyrdaPrefab, GetSpawnPosition(), Quaternion.identity);
        }
    }

    private static Vector3 GetSpawnPosition(bool randomizeY = false)
    {
        // Get the player's position and facing direction
        Transform playerTransform = PlayerManager.Instance.player.transform;
        float xOffset = randomizeY ? PlayerManager.Instance.player.facingDir * Random.Range(-20, 20) : PlayerManager.Instance.player.facingDir * Random.Range(15, 25);
        float yOffset = randomizeY ? Random.Range(5, 15) : 0;

        return new Vector3(playerTransform.position.x + xOffset, playerTransform.position.y + yOffset, 0);
    }
#endregion

#region Debug Options
    public void ShowDebugOptions(bool show)
    {
        debugOptionsOnGroup.SetActive(show);
    }

    private void SetRightClickAction()
    {
        SkillManager.Instance.canUseParry = parryToggle.isOn;
        SkillManager.Instance.canUseSword = swordThrowToggle.isOn;
    }

    public void ToggleParry()
    {
        if (parryToggle.isOn)
        {
            swordThrowToggle.isOn = false;
        }
        else if (!swordThrowToggle.isOn)
        {
            // Ensure at least one is always on
            parryToggle.isOn = true;
        }
        SetRightClickAction();
    }

    public void ToggleSwordThrow()
    {
        if (swordThrowToggle.isOn)
        {
            parryToggle.isOn = false;
        }
        else if (!parryToggle.isOn)
        {
            // Ensure at least one is always on
            swordThrowToggle.isOn = true;
        }
    }

    public void OnDropdownValueChanged(int value)
    {
        switch (swordTypeDropdown.value)
        {
            case 0: // Default Sword
                SkillManager.Instance.sword.swordType = SwordType.Regular;
                break;
            case 1: // Bounce
                SkillManager.Instance.sword.swordType = SwordType.Bounce;
                break;
            case 2: // Pierce
                SkillManager.Instance.sword.swordType = SwordType.Pierce;
                break;
            case 3: // Spin
                SkillManager.Instance.sword.swordType = SwordType.Spin;
                break;
            default:
                Debug.LogWarning("Unknown sword type selected.");
                break;
        }
    }
#endregion
    
}
