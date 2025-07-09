public class PlayerStats : CharacterStats
{
    private Player player;
    
    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage, bool _knockback = false)
    {
        player.fx.FlashVignette();
        if (_damage >= player.stats.GetMaxHealthValue() * .3f)
            _knockback = true;

        base.TakeDamage(_damage, _knockback);
    }

    protected override void Die()
    {
        base.Die();
        player.isDead = true;
        player.Die();

        // GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        
        PlayerManager.Instance.currency = 0;

        //TODO GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    public override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);
        
        Inventory inventory = Inventory.instance;
        if (inventory.canUseArmor())
        {
            ItemData_Equipment armor =inventory.GetEquipment(EquipmentType.Armor);
            if (armor != null)
                armor.Effect(player.transform);
        }
    }

    public override void OnEvasion()
    {
        // TODO SKILLs
        // player.skill.dodge.CreateMirageOnDodge();
    }
}

