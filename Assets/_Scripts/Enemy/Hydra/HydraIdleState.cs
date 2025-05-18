using UnityEngine;

public class HydraIdleState : EnemyState
{
    EnemyHydra enemy;
    public HydraIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyHydra _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        stateTimer = Random.Range(2f, 6f);
    }

    public override void Update()
    {
        base.Update();
        
        if (Vector2.Distance(enemy.transform.position, PlayerManager.Instance.player.transform.position) < enemy.attackDistance)
        {
            stateMachine.ChangeState(enemy.attackState);
        }
        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(enemy.shootState);
        }
        else
        {
            stateTimer -= Time.deltaTime;
        }
    }

    public override void Exit()
    {
        base.Exit();


    }
}