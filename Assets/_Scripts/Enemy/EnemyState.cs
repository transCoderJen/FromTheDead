using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{

    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;
    private string animBoolName;
    protected float stateTimer;
    protected bool triggerCalled;
    protected bool projecticleTriggerCalled;
    public string stateName;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }
    
    public virtual void Enter()
    {
        enemyBase.anim.SetBool(animBoolName, true);
        rb = enemyBase.rb;
        triggerCalled = false;
        projecticleTriggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
        enemyBase.AssignLastAnimName(animBoolName);
    }

    public virtual void AnimationFinishedTrigger()
    {
        triggerCalled = true;
    }

    public virtual void projecticleTrigger()
    {
        projecticleTriggerCalled = true;
    }
}
