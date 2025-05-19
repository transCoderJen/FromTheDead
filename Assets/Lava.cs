using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private GameObject lavaburstPrefab;
    [SerializeField] private float lavaburstInterval = 5f;
    private float lavaburstTimer = 0;

    private void Update()
    {
        lavaburstTimer += Time.deltaTime;
        if (lavaburstTimer >= lavaburstInterval)
        {
            SpawnLavaBurst();
            lavaburstTimer = 0f;
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
            Player player = collision.GetComponent<Player>();
            GetComponent<CharacterStats>().DoMagicDamage(player.stats, false);
            player.stateMachine.ChangeState(player.respawnHolyState);
        }
        
        if (collision.GetComponent<EnemySkeleton>() != null)
        {
            EnemySkeleton enemy = collision.GetComponent<EnemySkeleton>();
            enemy.stateMachine.ChangeState(enemy.deadState);
        }
    }
}
