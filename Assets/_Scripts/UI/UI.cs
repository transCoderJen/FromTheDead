using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UI : Singleton<UI>, ISaveManager
{
    private PlayerControlller playerControls;
    [SerializeField] private GameObject inGame;
    [SerializeField] private GameObject characterMenu;
    [SerializeField] private EquipSkillMenu_UI equipSkillMenu;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private UI_VolumeSlider[] volumeSettings;

    public UI_StatTooltip statTooltip;
    public UI_ItemTooltip itemTooltip;
    

    private bool menuOpened = false;
    private bool showPopupText = true;

    void Start()
    {
        playerControls = PlayerManager.Instance.player.playerControls;
        statTooltip.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (playerControls.UI.ToggleMenu.triggered)
        {
            if (menuOpened)
            {
                Time.timeScale = 1;
                SwitchTo(inGame);
                menuOpened = false;

                AudioManager.Instance.ResumeAllSFX();
                equipSkillMenu.Cancel();
            }
            else
            {
                Time.timeScale = 0;
                SwitchTo(characterMenu);
                menuOpened = true;

                AudioManager.Instance.PauseAllSFX();

            }
        }
    }

    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
            _menu.SetActive(true);
    }

    public bool IsMenuOpened()
    {
        return menuOpened;
    }

    public void togglePopupText()
    {
        showPopupText = !showPopupText;
    }

    public bool IsPopupTextEnabled()
    {
        return showPopupText;
    }

    public void LoadData(GameData _data)
    {
        Debug.Log("Loading volume settings...");
        foreach (KeyValuePair<string, float> pair in _data.volumeSettings)
        {
            foreach (UI_VolumeSlider item in volumeSettings)
            {
                if (item.parameter == pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }

        showPopupText = _data.showPopupText;
        Debug.Log("ShowPopupText loaded as: " + showPopupText);
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach (UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parameter, item.slider.value);
        }

        _data.showPopupText = showPopupText;
        Debug.Log("ShowPopupText saved as: " + showPopupText);
    }
}
