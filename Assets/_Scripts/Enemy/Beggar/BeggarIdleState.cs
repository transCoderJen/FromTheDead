public class BeggarIdleState : EnemyState
{
    private EnemyBeggar enemy;

    public BeggarIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyBeggar _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    override public void Enter()
    {
        base.Enter();
        stateName = "Idle";
        stateTimer = enemy.idleTime;
    }

    override public void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(enemy.attackState);
        }

        if (PlayerManager.Instance.player.transform.position.x < enemy.transform.position.x && enemy.facingDir != -1)
        {
            enemy.Flip();
        }
        else if(PlayerManager.Instance.player.transform.position.x > enemy.transform.position.x && enemy.facingDir != 1)
        {
            enemy.Flip();
        }
    }
}
