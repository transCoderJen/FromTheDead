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
        bool fireball = Random.value > 0.5f ? true : false;
        

        for (int i = 0; i < Random.Range(3,6); i++)
        {
            if (fireball)
                enemy.CreateProjectile(enemy.projectileSpawnPoint.position, enemy.transform.rotation);
            else
                enemy.CreateSpike();

            yield return new WaitForSeconds(Random.Range(1.5f, 3f));
        }
    }
}