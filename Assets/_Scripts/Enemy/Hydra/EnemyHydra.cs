using System;
using UnityEngine;

public class EnemyHydra : Enemy
{
    [SerializeField] private GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    #region States
    public HydraIdleState idleState { get; private set; }
    public HydraAttackState attackState { get; private set; }
    public HydraDeathState deadState { get; private set; }
    public HydraShootState shootState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new HydraIdleState(this, stateMachine, "idle", this);
        attackState = new HydraAttackState(this, stateMachine, "swipe", this);
        shootState = new HydraShootState(this, stateMachine, "shoot", this);
        deadState = new HydraDeathState(this, stateMachine, "dead", this);

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

