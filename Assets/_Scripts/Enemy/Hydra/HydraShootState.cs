using System.Collections;
using UnityEngine;

public class HydraShootState : EnemyState
{
    private EnemyHydra enemy;

    public HydraShootState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyHydra _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateName = "Shoot";
    }

    public override void Update()
    {
        base.Update();

        if (projecticleTriggerCalled)
        {
            enemy.StartCoroutine(CreateMultipleProjectiles());
            // enemy.CreateProjectile(enemy.transform.position, enemy.transform.rotation);
            projecticleTriggerCalled = false;
        }

        if (triggerCalled) { stateMachine.ChangeState(enemy.idleState); }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private IEnumerator CreateMultipleProjectiles()
    {
        for (int i = 0; i < 3; i++)
        {
            enemy.CreateProjectile(enemy.projectileSpawnPoint.position, enemy.transform.rotation);
            yield return new WaitForSeconds(1f);
        }
    }
}