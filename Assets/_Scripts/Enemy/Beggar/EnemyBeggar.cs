using System;
using UnityEngine;

public class EnemyBeggar : Enemy
{
    [SerializeField] private GameObject projectilePrefab;

    #region States
    public BeggarIdleState idleState { get; private set; }
    public BeggarAttackState attackState { get; private set; }
    public BeggarDeathState deadState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new BeggarIdleState(this, stateMachine, "idle", this);
        attackState = new BeggarAttackState(this, stateMachine, "attack", this);
        deadState = new BeggarDeathState(this, stateMachine, "dead", this);

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

    public void CreateProjectile(Vector3 position, Quaternion rotation)
    {
        if (facingDir == -1)
        {
            rotation = Quaternion.Euler(0, 180, 0);
        }

        GameObject begg_Shot = Instantiate(projectilePrefab, position, rotation);
        begg_Shot.GetComponent<BeggShot>().Initialize(this);
    }
}

