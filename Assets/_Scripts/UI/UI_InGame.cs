using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_InGame : Singleton<UI_InGame>
{
    [Header("UI Elements")]
    [SerializeField] private Slider slider;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = (PlayerStats)PlayerManager.Instance.player.stats;
        if (playerStats != null)
            playerStats.onHealthChanged += UpdateHealthUI;
    }

    void Update()
    {
        if (PlayerManager.Instance.player.isDead && !gameOverPanel.activeSelf)
        {
            StartCoroutine(WaitForGameOver());
        }
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

    public void OnPauseButtonClicked()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
        Time.timeScale = pausePanel.activeSelf ? 0 : 1;
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
}