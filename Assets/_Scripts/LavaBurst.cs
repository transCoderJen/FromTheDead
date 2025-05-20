using UnityEngine;

public class LavaBurst : MonoBehaviour
{
    private BoxCollider2D cd;

    private void Awake()
    {
        cd = GetComponent<BoxCollider2D>();
        cd.isTrigger = true;
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
            GetComponentInParent<CharacterStats>().DoMagicDamage(player.stats, false);
        }
    }
}
