using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_InGame : Singleton<UI_InGame>
{
    [Header("UI Elements")]
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject gameOverPanel;

    [Header("UI Cooldowns")]
    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;

    [SerializeField] private Image skill1Image;
    [SerializeField] private Image skill2Image;

    [SerializeField] private Image cooldownImageSkill1;
    [SerializeField] private Image cooldownImageSkill2;

    [Header("Skillls")]
    [SerializeField] private SkillData skill1;
    [SerializeField] private SkillData skill2;

    private PlayerStats playerStats;
    private SkillManager skills;
    private Player player;

    private void Start()
    {
        skills = SkillManager.Instance;
        playerStats = (PlayerStats)PlayerManager.Instance.player.stats;
        if (playerStats != null)
            playerStats.onHealthChanged += UpdateHealthUI;
        player = PlayerManager.Instance.player;

        SetSkill1(skill1);
        SetSkill2(skill2);
        
    }

    public void SetSkill1(SkillData _skill)
    {
        skill1 = _skill;
        skill1Image.sprite = skill1.skillSprite;
        cooldownImageSkill1.sprite = skill1.skillSprite;
        SkillManager.Instance.AssignSkill(skill1, SkillSlot.Slot1);
    }

    public void SetSkill2(SkillData _skill)
    {
        skill2 = _skill;
        skill2Image.sprite = skill2.skillSprite;
        cooldownImageSkill2.sprite = skill2.skillSprite;
        SkillManager.Instance.AssignSkill(skill2, SkillSlot.Slot2);
    }

    public String GetSkill1Name()
    {
        return skill1 != null ? skill1.name : "No Skill 1 Assigned";
    }

    public String GetSkill2Name()
    {
        return skill2 != null ? skill2.name : "No Skill 2 Assigned";
    }

    void Update()
    {
        if (player.isDead && !gameOverPanel.activeSelf)
        {
            StartCoroutine(WaitForGameOver());
        }

        if (player.playerControls.Player.Dash.triggered)
            SetCooldownOf(dashImage);

        if (player.playerControls.Player.Spell2.triggered)
            SetCooldownOf(cooldownImageSkill2);

        if (player.playerControls.Player.Spell1.triggered)
            SetCooldownOf(cooldownImageSkill1);

        if (player.playerControls.Player.Counter.triggered)
            SetCooldownOf(parryImage);

        CheckCooldownOf(parryImage, skills.parry.cooldown);    
        CheckCooldownOf(dashImage, skills.dash.cooldown);
        CheckCooldownOf(cooldownImageSkill2, skill2.skillPrefab.GetComponent<ISpell>().GetCooldown());    
        CheckCooldownOf(cooldownImageSkill1, skill1.skillPrefab.GetComponent<ISpell>().GetCooldown());
    }

    private IEnumerator WaitForGameOver()
    {
        yield return Helpers.GetWait(2f);
        OnGameOver();
    }

    public void OnGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    public void RestartGame()
    {
        gameOverPanel.SetActive(false);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        AudioManager.Instance.playBGM = true;
        PlayerManager.Instance.player.isDead = false;
        PlayerManager.Instance.player.stats.ResetStats();

        // PlayerManager.Instance.player.transform.position = PlayerManager.Instance.player.respawnPosition.position;
        PlayerManager.Instance.player.stateMachine.ChangeState(PlayerManager.Instance.player.respawnHolyState);
    }
    
    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
        {
            _image.fillAmount = 1;
        }
    }

    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }
}