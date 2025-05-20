using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlaySFX("thunderstrike", null);
    }
    
    protected virtual void OnTrigger2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            PlayerStats playerStats = PlayerManager.Instance.player.stats as PlayerStats;
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();
            playerStats.DoMagicDamage(enemyTarget, false);

            Debug.Log("Played thunderstrike sound");
        }
    }
}
