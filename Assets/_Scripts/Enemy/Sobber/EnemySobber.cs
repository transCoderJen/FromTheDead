using UnityEngine;
public class EnemySobber : Enemy
{
    #region States
    public SobberIdleState idleState { get; private set; }
    public SobberBattleState battleState { get; private set; }
    public SobberAttackState attackState { get; private set; }
    public SobberDeadState deadState { get; private set; }
    #endregion

    public float atackRange;

    protected override void Awake()
    {
        base.Awake();

        idleState = new SobberIdleState(this, stateMachine, "Idle", this);
        battleState = new SobberBattleState(this, stateMachine, "Move", this);
        attackState = new SobberAttackState(this, stateMachine, "Attack", this);
        deadState = new SobberDeadState(this, stateMachine, "Idle", this);

        SetupDefaultFacingDir(-1);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atackRange);
    }
}

