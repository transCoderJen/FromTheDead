using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private float lastTimeAttaked;
    private float comboWindow = 0.3f;
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        xInput = 0;
        
        if (comboCounter > 2 || Time.time >= lastTimeAttaked + comboWindow)
            comboCounter = 0;
        
        player.anim.SetInteger("ComboCounter", comboCounter);
        stateName = "Attack" + comboCounter;

        float attackDir = player.facingDir;

        if (xInput !=0)
            attackDir = xInput;

        if (player.IsGroundDetected())
            player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);
        else
            player.SetVelocity(rb.linearVelocity.x, rb.linearVelocity.y);

        stateTimer = .15f;
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .15f);

        comboCounter++;
        lastTimeAttaked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0 && player.IsGroundDetected())
            player.ZeroVelocity();
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    public int getComboCounter() => comboCounter;

}
