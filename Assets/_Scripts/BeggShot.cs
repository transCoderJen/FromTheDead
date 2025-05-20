using System;
using UnityEngine;

public class BeggShot : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private Enemy beggar;
    private bool targetPlayer = true;
    private int travelDirection = 1;

    public void Initialize(Enemy beggar)
    {
        this.beggar = beggar;
    }

    public void CounterAttack()
    {
        targetPlayer = false;
        travelDirection = -1;
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * travelDirection * speed * Time.deltaTime);

        float scale = Mathf.Max(0, lifeTime / 2f);
        transform.localScale = new Vector3(scale, scale, 1f);

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && targetPlayer)
        {
            beggar.stats.DoMagicDamage(collision.GetComponent<Player>().stats, true);
            Destroy(gameObject);
        }

        if (collision.GetComponent<Enemy>() != null && !targetPlayer)
        {
            PlayerManager.Instance.player.stats.DoMagicDamage(collision.GetComponent<Enemy>().stats, true);
            Destroy(gameObject);
        }
    }
}
