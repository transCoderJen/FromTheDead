using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class SobberIdleState : EnemyState
{
    private EnemySobber enemy;

    public SobberIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySobber _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        // Move the enemy randomly within a small radius around its original position
        float moveRadius = 1.5f; // Adjust as needed
        float moveSpeed = 1.0f;  // Adjust as needed

        Vector2 originalPosition = enemy.transform.position;


        Vector2 randomOffset = new Vector2(
            Mathf.PerlinNoise(Time.time * moveSpeed, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, Time.time * moveSpeed) - 0.5f
        ) * 2f * moveRadius;

        Vector2 targetPosition = (Vector2)originalPosition + randomOffset;
        enemy.transform.position = Vector2.Lerp(enemy.transform.position, targetPosition, Time.deltaTime);

        if (Vector2.Distance(enemy.transform.position, PlayerManager.Instance.player.transform.position) < enemy.atackRange)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
        
    }

    override public void Exit()
    {
        base.Exit();
    }
}