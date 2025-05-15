using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;    
        enemy.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        stateName = "Idle";
    }

    public override void Exit()
    {
        base.Exit();

        //TODO AudioManager.Instance.PlaySFX(SFXSounds.skeleton_bones, enemy.transform);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);
    }
}
