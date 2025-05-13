using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb;
    protected CapsuleCollider2D cd;
    protected float xInput;
    protected float yInput;

    private string animBoolName;
    protected float afterImageTimer = 0f;
    protected float stateTimer;
    protected bool triggerCalled;
    public string stateName;
    
    

    public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
        rb = player.GetComponent<Rigidbody2D>();
        cd = player.GetComponent<CapsuleCollider2D>();
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        player.anim.SetBool(animBoolName, true);
    }

    public virtual void Update() {
        stateTimer -= Time.deltaTime;
        afterImageTimer += Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        player.anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    public virtual void FixedUpdate() {

    }

    public virtual void Exit() {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    public string GetAnimBoolName()
    {
        return animBoolName;


    }

    protected void CreateTrailAfterImage()
    {
        if (afterImageTimer > player.fx.afterImageRate)
        {
            Debug.Log("Creating after image");
            player.fx.CreateAfterImageFX(player.transform);
            afterImageTimer = 0;
        }
    }
}