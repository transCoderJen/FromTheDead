using UnityEngine;

public class SkillPickup : MonoBehaviour
{
    [SerializeField] private SkillData skillData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            SkillInventory.Instance.AddSkillToInventory(skillData);
            Destroy(gameObject);
        }
    }
}
