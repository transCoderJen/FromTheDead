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

        if (comboCounter == 0)
        {
            player.sr.material = player.attack1Mat;
            AudioManager.Instance.PlaySFX("Player_Attack_1");
        }
        else if (comboCounter == 1)
        {
            player.sr.material = player.attack2Mat;
            AudioManager.Instance.PlaySFX("Player_Attack_1");
        }
        else if (comboCounter == 2)
        {
            player.sr.material = player.attack3Mat;
            AudioManager.Instance.PlaySFX("Player_Attack_3");
        }

        player.anim.SetInteger("ComboCounter", comboCounter);
        stateName = "Attack" + comboCounter;

        float attackDir = player.facingDir;

        if (xInput != 0)
            attackDir = xInput;

        if (player.IsGroundDetected() && comboCounter > 0)
            player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);
        else
            player.SetVelocity(rb.linearVelocity.x, rb.linearVelocity.y);

        stateTimer = .15f;
        
        player.ResetMaterial();
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
        if (xInput == 1 && player.facingDir == -1)
            player.Flip();
        else if (xInput == -1 && player.facingDir == 1)
            player.Flip();

        base.Update();

        if (stateTimer < 0 && player.IsGroundDetected())
            player.ZeroVelocity();
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    public int getComboCounter() => comboCounter;

}
