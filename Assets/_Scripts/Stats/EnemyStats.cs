using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    // private ItemDrop myDropSystem;
    public Stat soulsDropAmount;

    [Header("Level Details")]
    [SerializeField] private int level = 1;
    
    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .2f;

    protected override void Start()
    {

        base.Start();

        soulsDropAmount.SetDefaultValue(100);
        ApplyLevelModifiers();
        enemy = GetComponent<Enemy>();
        // myDropSystem = GetComponent<ItemDrop>();

    }

    private void ApplyLevelModifiers()
    {
        Modify(strength);        
        Modify(agility);        
        Modify(intelligence);        
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);
        
        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);
        Modify(soulsDropAmount);
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage, bool _knockback)
    {
        base.TakeDamage(_damage, _knockback);
    }

    protected override void Die()
    {
        base.Die();
        //TODO AudioManager.Instance.PlaySFX(SFXSounds.attack3, null)
        enemy.Die();

        // myDropSystem.GenerateDrop();

        // PlayerManager.Instance.currency += soulsDropAmount.GetValue();
    }
}
