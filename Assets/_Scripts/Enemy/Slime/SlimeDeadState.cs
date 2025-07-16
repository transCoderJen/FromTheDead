using UnityEngine;

public class SlimeDeadState : EnemyState
{
    private EnemySlime enemy;
    public SlimeDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            Object.Destroy(enemy.gameObject);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}