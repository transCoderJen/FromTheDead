using UnityEngine;

public class SkillPickup : MonoBehaviour
{
    [SerializeField] private SpellData skillData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            SpellInventory.Instance.AddSkillToInventory(skillData);
            Destroy(gameObject);
        }
    }
}
