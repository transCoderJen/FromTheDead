using UnityEngine;

public class LastScreamController : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float growthRate;
    [SerializeField] private float spinRate;
    Player player => PlayerManager.Instance.player;

    private void Update()
    {
        // LeanTween.scale(gameObject, transform.localScale + Vector3.one * growthRate * Time.deltaTime, Time.deltaTime).setEase(LeanTweenType.linear);
        LeanTween.rotateZ(gameObject, transform.eulerAngles.z + spinRate * Time.deltaTime, Time.deltaTime).setEase(LeanTweenType.linear);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        player.stats.IncreaseStatBy(10, 3f, player.stats.getStat(StatType.lightningDamage));

        if (collision.GetComponent<Enemy>() != null)
        {
            EnemyStats _target = collision.GetComponent<EnemyStats>();

            if (_target != null)
            {
                player.stats.DoMagicDamage(_target, true);
            }
        }
    }
}
