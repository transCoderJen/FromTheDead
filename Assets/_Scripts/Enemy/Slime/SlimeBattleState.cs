using UnityEngine;
public class SlimeBattleState : SlimeGroundedState
{
    private int moveDir;
    private float originalMoveSpeed;
    private float speedIncreaseRate = 1.3f;

    public SlimeBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime enemy) : base(_enemyBase, _stateMachine, _animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        if (PlayerManager.Instance.player.isDead)
            stateMachine.ChangeState(enemy.moveState);
        base.Enter();
        originalMoveSpeed = enemy.moveSpeed;
        enemy.moveSpeed *= speedIncreaseRate;
        Debug.Log("Slime Battle State Entered");
    }

    public override void Update()
    {
        base.Update();
        if (!enemy.IsGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }

        if (enemy.isPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.isPlayerDetected().distance < enemy.attackDistance)
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
            moveDir = -1;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.moveSpeed = originalMoveSpeed;
    }

    private bool canAttack()
    {
        if (Time.time - enemy.attackCooldown >= enemy.lastTimeAttacked)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        else return false;
    }
}