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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                if (_target != null)
                {
                    float damagePower = 1;
                    if (player.primaryAttack.getComboCounter() == 2) { damagePower = 1.2f; }

                    player.stats.DoDamage(_target, true, damagePower);
                    player.fx.ScreenShake(player.fx.lightShakePower);
                    AudioManager.Instance.PlaySFX("Player_Attack_2");
                }

                //TODO WEAPON MODIFIER
                //Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(_target.transform);
            }
        }
    }

    private void AttackTrigger2()
    {     
        player.stats.IncreaseStatBy(10, 3f, player.stats.getStat(StatType.lightningDamage));

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck2.position, player.attackCheckRadius2);

        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                if (_target != null)
                {
                    player.stats.DoMagicDamage(_target, true);            
                }

                AudioManager.Instance.PlaySFX("Player_Attack_2");
                //TODO WEAPON MODIFIER
                //Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(_target.transform);
            }
        }
    }
}
