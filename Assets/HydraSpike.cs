using UnityEngine;

public class HydraSpike : MonoBehaviour
{
    private BoxCollider2D cd;
    private EnemyHydra hydra;

    private void Awake()
    {
        cd = GetComponent<BoxCollider2D>();
        cd.isTrigger = true;
    }

    public void Initialize(EnemyHydra hydra)
    {
        this.hydra = hydra;
    }

    public void EnableCollider()
    {
        cd.enabled = true;
    }

    public void DisableCollider()
    {
        cd.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Player player = collision.GetComponent<Player>();
            GetComponent<CharacterStats>().DoDamage(player.stats, true);
            player.fx.ScreenShake(player.fx.heavyShakePower);
        }
    }
}
