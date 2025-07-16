using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_InGame : Singleton<UI_InGame>, ISaveManager
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
    [SerializeField] private SpellData skill1;
    [SerializeField] private SpellData skill2;

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

        if (!SaveManager.instance.HasSavedData())
        {
            SetSkill1(skill1);
            SetSkill2(skill2);
        }
    }

    public void SetSkill1(SpellData _skill)
    {
        skill1 = _skill;
        skill1Image.sprite = skill1.spellSprite;
        cooldownImageSkill1.sprite = skill1.spellSprite;
        SkillManager.Instance.AssignSkill(skill1, SkillSlot.Slot1);
    }

    public void SetSkill2(SpellData _skill)
    {
        skill2 = _skill;
        skill2Image.sprite = skill2.spellSprite;
        cooldownImageSkill2.sprite = skill2.spellSprite;
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
        CheckCooldownOf(cooldownImageSkill2, skill2.spellPrefab.GetComponent<ISpell>().GetCooldown());    
        CheckCooldownOf(cooldownImageSkill1, skill1.spellPrefab.GetComponent<ISpell>().GetCooldown());
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

    public void LoadData(GameData _data)
    {
        if (_data.equippedSpells.Count < 2)
        {
            return;
        }
        
        foreach (var spell in Inventory.instance.spellDataBase)
        {
            if (spell != null && spell.itemId == _data.equippedSpells[0])
                SetSkill1(spell as SpellData);
            else if (spell != null && spell.itemId == _data.equippedSpells[1])
                SetSkill2(spell as SpellData);
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.equippedSpells.Clear();
        if (skill1 != null)
            _data.equippedSpells.Add(skill1.itemId);
        if (skill2 != null)
            _data.equippedSpells.Add(skill2.itemId);
    }
}