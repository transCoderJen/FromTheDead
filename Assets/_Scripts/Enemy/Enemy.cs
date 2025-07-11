using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EntityFX))]
// [RequireComponent(typeof(ItemDrop))]

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;
 
    [Header("Stunned Info")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;


    [Header("Move Info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    private float defaultMoveSpeed;

    [Header("ATtack Info")]
    public float attackDistance;
    public float attackCooldown;
    public float minAttackCooldown;
    public float maxAttackCooldown;
    [HideInInspector] public float lastTimeAttacked;

    public EnemyStateMachine stateMachine { get; private set; }
    public EntityFX fx { get; private set; }
    public string lastAnimBoolName { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
        
    }

    protected override void Start()
    {
        base.Start();
        fx = GetComponent<EntityFX>();
    }

    protected override void Update()
    {            
        base.Update();
        stateMachine.currentState.Update();
        
        UpdateStateText();

        if (Vector2.Distance(transform.position, PlayerManager.Instance.player.transform.position) < .1f)
        {
            if (PlayerManager.Instance.player.GetComponent<PlayerStats>().isInvincible)
                return;
            GetComponent<CharacterStats>().DoDamage(PlayerManager.Instance.player.GetComponent<CharacterStats>(), true, 1, false);
        }

        if (transform.position.y < -20)
            Destroy(this.gameObject);

    }

    public override void DamageEffect(bool _knockback)
    {
        fx.FlashHitFX();
        
        base.DamageEffect(_knockback);   
    }

    public virtual void AssignLastAnimName(string _animBoolName) => lastAnimBoolName = _animBoolName;

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else if(!_timeFrozen)
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    public virtual void FreezeTimeFor(float duration) => StartCoroutine(FreezeTimeCoroutine(duration));
    
    protected virtual IEnumerator FreezeTimeCoroutine(float _seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);
    }
    
    #region Counter Attack Window
        
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        GetComponent<PulseIntensity>().TriggerPulse();
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishedTrigger();
    public void ProjectileTrigger() => stateMachine.currentState.projecticleTrigger();

    public virtual RaycastHit2D isPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir,  50, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
        
    }

    private void UpdateStateText()
    {
        if (debugStateText == null)
            return;
            
        debugStateText.text = stateMachine.currentStateName;

        debugStateText.GetComponent<Transform>().position = transform.position + new Vector3(0, 2, 0);
    }
}
