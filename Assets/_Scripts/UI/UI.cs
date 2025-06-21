using UnityEngine;
using UnityEngine.Audio;

public class UI : Singleton<UI>
{
    private PlayerControlller playerControls;
    [SerializeField] private GameObject inGame;
    [SerializeField] private GameObject characterMenu;
    [SerializeField] private EquipSkillMenu_UI equipSkillMenu;
    [SerializeField] private AudioMixer audioMixer;

    private bool menuOpened = false;
    private bool showPopupText = true;

    void Start()
    {
        playerControls = PlayerManager.Instance.player.playerControls;
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
}
