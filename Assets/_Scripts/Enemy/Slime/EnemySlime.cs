using UnityEngine;

public class EnemySlime : Enemy
{
    public GameObject slimePrefab;
    #region States
    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeDeadState deadState { get; private set; }
    public SlimeTurnState turnState { get; private set; }

    #endregion

    public bool isBig;

    protected override void Awake()
    {
        base.Awake();

        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new SlimeBattleState(this, stateMachine, "Move", this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);
        turnState = new SlimeTurnState(this, stateMachine, "Turn", this);
        deadState = new SlimeDeadState(this, stateMachine, "Dead", this);

        SetupDefaultFacingDir(1);
    }

    public void SetupVelocity(Vector2 _velocity)
    {
        GetComponent<Rigidbody2D>().linearVelocity = _velocity;
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

     public void SpawnClones()
    {
        if (isBig)
        {
            GameObject slimeClone1 = Instantiate(slimePrefab, transform.position, Quaternion.identity);
            GameObject slimeClone2 = Instantiate(slimePrefab, transform.position, Quaternion.identity);

            Vector2 randomVelocity1 = new Vector2(Random.Range(-4, 0), Random.Range(0, 5));
            Vector2 randomVelocity2 = new Vector2(Random.Range(0, 4), Random.Range(0, 5));


            var slimeComponent1 = slimeClone1.GetComponentInChildren<EnemySlime>();
            slimeComponent1.SetupVelocity(randomVelocity1);
            var slimeComponent2 = slimeClone2.GetComponentInChildren<EnemySlime>();
            slimeComponent2.SetupVelocity(randomVelocity2);
        }
    }
}