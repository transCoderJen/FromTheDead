using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        // AudioManager.instance.PlaySFX(SFXSounds.attack3, null);
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                if (_target != null)
                    player.stats.DoDamage(_target, true);

                //TODO WEAPON MODIFIER
                //Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(_target.transform);
            }
        }
    }

    private void AttackTrigger2()
    {
        // AudioManager.instance.PlaySFX(SFXSounds.attack3, null);
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck2.position, player.attackCheckRadius2);

        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                if (_target != null)
                    player.stats.DoDamage(_target, true);

                //TODO WEAPON MODIFIER
                //Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(_target.transform);
            }
        }
    }
}
