public class HydraAttackState : EnemyState
{
    private EnemyHydra enemy;

    public HydraAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyHydra _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateName = "Attack";
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}