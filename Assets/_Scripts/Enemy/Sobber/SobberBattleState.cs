using UnityEngine;

public class SobberBattleState : EnemyState
{
    private EnemySobber enemy;
    private bool isRetreating = false;
    private float retreatTimer = 0f;
    private float retreatDuration = 1f;
    private Vector2 retreatDirection;

    public SobberBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySobber _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.Instance.PlaySFX("wingFlap", this.enemy.transform);
    }


    public override void Update()
    {
        base.Update();

        if (isRetreating)
        {
            // Move away from the player
            enemy.transform.position = Vector2.MoveTowards(
                enemy.transform.position,
                (Vector2)enemy.transform.position + retreatDirection,
                enemy.moveSpeed * Time.deltaTime
            );

            retreatTimer -= Time.deltaTime;
            if (retreatTimer <= 0f)
            {
                isRetreating = false;
            }
            return;
        }

        // Move towards the player
        enemy.transform.position = Vector2.MoveTowards(
            enemy.transform.position,
            PlayerManager.Instance.player.transform.position,
            enemy.moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(enemy.transform.position, PlayerManager.Instance.player.transform.position) <.2f)
        {
            enemy.stats.DoDamage(PlayerManager.Instance.player.stats, true);
            
            // Start retreating
            isRetreating = true;
            retreatTimer = retreatDuration;
            Vector2 toPlayer = (PlayerManager.Instance.player.transform.position - enemy.transform.position).normalized;
            retreatDirection = -toPlayer; // Move away from player
            
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}