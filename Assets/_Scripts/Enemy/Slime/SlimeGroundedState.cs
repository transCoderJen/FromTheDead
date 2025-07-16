using UnityEngine;

public class SlimeGroundedState : EnemyState
{
    protected EnemySlime enemy;
    protected Transform player;

    public SlimeGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.Instance.player.transform;
    }

    public override void Update()
    {
        base.Update();

        if (player != null)
        {
            Debug.Log("Player detected by raycast: " + (enemy.isPlayerDetected().collider != null));
            if (enemy.isPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 2)
                stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}