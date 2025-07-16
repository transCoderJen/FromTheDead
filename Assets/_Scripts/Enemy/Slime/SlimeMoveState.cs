public class SlimeMoveState : SlimeGroundedState
{
    public SlimeMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime enemy) : base(_enemyBase, _stateMachine, _animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            // enemy.Flip();
            // stateMachine.ChangeState(enemy.idleState);
            stateMachine.ChangeState(enemy.turnState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}