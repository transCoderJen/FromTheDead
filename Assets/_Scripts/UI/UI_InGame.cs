using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
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

    private void Update()
    {
        if (playerStats.currentHealth <= 0)
        {
            gameOverPanel.SetActive(true);
        }
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}