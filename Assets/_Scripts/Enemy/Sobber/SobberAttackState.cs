public class SobberAttackState : EnemyState
{
    private EnemySobber enemy;

    public SobberAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySobber _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
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