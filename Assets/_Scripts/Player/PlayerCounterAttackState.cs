using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.counterAttackDuration;
        stateName = "Parry";
        player.anim.SetBool("SuccessfulCounterAttack", false);
        player.ZeroVelocity();
        player.ResetMaterial();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10;
                    player.anim.SetBool("SuccessfulCounterAttack", true);
                    stateName = "Parry_Success";
                    //TODO AudioManager.instance.PlaySFX(SFXSounds.attack1, null);   
                    //TODO SkillManager.instance.parry.UseSkill();

                    //TODO player.skill.parry.MakeMirageOnParry(hit.transform, new Vector3(2.5f * player.facingDir, 0));

                }
            }

            if (hit.GetComponent<BeggShot>() != null)
            {
                Debug.Log("Hit BeggShot CoutnerAttack");
                hit.GetComponent<BeggShot>().CounterAttack();
                player.stats.IncreaseStatBy(10, 3f, player.stats.getStat(StatType.fireDamage));
                player.stats.ImpactEffect(.2f);
                stateTimer = 10;
                player.anim.SetBool("SuccessfulCounterAttack", true);
                stateName = "Parry_Success";
            }
        }

        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
