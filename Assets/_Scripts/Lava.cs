using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private GameObject lavaburstPrefab;
    [SerializeField] private float lavaburstInterval = 5f;
    private float lavaburstTimer = 0;

    [SerializeField] private float playerDamageInterval = 2f;
    private float playerDamageTimer = 0f;
    private Player playerInLava = null;

    private void Update()
    {
        lavaburstTimer += Time.deltaTime;
        if (lavaburstTimer >= lavaburstInterval)
        {
            SpawnLavaBurst();
            lavaburstTimer = 0f;
        }

        if (playerInLava != null)
        {
            playerDamageTimer += Time.deltaTime;
            if (playerDamageTimer >= playerDamageInterval)
            {
                GetComponent<CharacterStats>().DoMagicDamage(playerInLava.stats, false);
                playerDamageTimer = 0f;
            }
        }
    }

    public void SpawnLavaBurst()
    {
        GameObject lavaburst = Instantiate(lavaburstPrefab, transform.position, Quaternion.identity);
        lavaburst.transform.SetParent(transform);
        lavaburst.transform.localScale = new Vector3(1, Random.Range(1, 4), 1f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            playerInLava = collision.GetComponent<Player>();
            GetComponent<CharacterStats>().DoMagicDamage(playerInLava.stats, false);
            playerDamageTimer = 0f;
        }

        if (collision.GetComponent<EnemySkeleton>() != null)
        {
            EnemySkeleton enemy = collision.GetComponent<EnemySkeleton>();
            enemy.stateMachine.ChangeState(enemy.deadState);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && playerInLava == collision.GetComponent<Player>())
        {
            playerInLava = null;
            playerDamageTimer = 0f;
        }
    }
}
