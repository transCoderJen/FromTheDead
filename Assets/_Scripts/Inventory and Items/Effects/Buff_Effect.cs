using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Effect/Buff Effect")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private int buffDuration;

    private void OnValidate()
    {
        effectDescription = "+ " + buffAmount + " " + buffType + " for " + buffDuration + " seconds on taking damage";
    }

    public override void ExecuteEffect(Transform _spawnPosition)
    {
        stats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        stats.IncreaseStatBy(buffAmount, buffDuration, stats.getStat(buffType));
    }
}
