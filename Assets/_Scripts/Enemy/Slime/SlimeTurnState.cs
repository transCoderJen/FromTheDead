public class SlimeTurnState : SlimeGroundedState
{
    public SlimeTurnState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime enemy) : base(_enemyBase, _stateMachine, _animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.Flip();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
            enemy.stateMachine.ChangeState(enemy.moveState);
    }
}