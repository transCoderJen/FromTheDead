using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private EnemySkeleton enemy;
    private int moveDir;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        if (PlayerManager.Instance.player.isDead)
            stateMachine.ChangeState(enemy.moveState);
        base.Enter();
        player = PlayerManager.Instance.player.transform;
        stateName = "Battle";
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.isPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if(enemy.isPlayerDetected().distance < enemy.attackDistance)
            {
                if (canAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
                stateMachine.ChangeState(enemy.idleState);

        }

        if (player.position.x > enemy.transform.position.x && Vector2.Distance(player.transform.position, enemy.transform.position) > 1)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x && Vector2.Distance(player.transform.position, enemy.transform.position) > 1)
            moveDir =-1;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.linearVelocity.y);
    }

    private bool canAttack()
    {
        if (Time.time - enemy.attackCooldown >= enemy.lastTimeAttacked)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }else return false;
    }
        
}
