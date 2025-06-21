using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    [SerializeField] private bool magicDamage;
    [SerializeField] private StatType statToIncrease;
    [SerializeField] private int increaseStatBy;
    private Player player;

    void Start()
    {
        player = PlayerManager.Instance.player;
        if (player == null)
        {
            Debug.Log("Player is null");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (magicDamage)
        {
            player.stats.IncreaseStatBy(increaseStatBy, 3f, player.stats.getStat(statToIncrease));
        }

        if (collision.GetComponent<Enemy>() != null)
        {
            Debug.Log("Enemy detected in laser collision");

            EnemyStats _target = collision.GetComponent<EnemyStats>();

            if (_target != null)
            {
                player.stats.DoMagicDamage(_target, true);
            }
            
        }
    }
}
