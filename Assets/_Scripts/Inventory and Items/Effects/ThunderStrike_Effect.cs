using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike Effect", menuName = "Data/Item Effect/Thunder Strike")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        if (Random.value <= 0.05f)
        {
            GameObject newThunderstrike = Instantiate(thunderStrikePrefab, _enemyPosition.position, Quaternion.identity);
            Destroy(newThunderstrike, 1);
        }
    }
}
