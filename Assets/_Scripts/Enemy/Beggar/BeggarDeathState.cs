using UnityEngine;

public class BeggarDeathState : EnemyState
{
    private EnemyBeggar enemy;

    public BeggarDeathState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyBeggar _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateName = "Death";
        enemy.ZeroVelocity();

        
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            Object.Destroy(enemy.gameObject);
        }
    }


}