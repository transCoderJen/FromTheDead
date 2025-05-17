using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Represents the player's dashing state in the state machine.
/// This state is responsible for handling the player's dash action,
/// including initiating the dash, applying the dash force, 
/// and transitioning to other states when necessary.
/// </summary>
public class PlayerDashState : PlayerState
{
    private bool jumpingOut = false;

    /// <summary>
    /// Initializes a new instance of the PlayerDashState class.
    /// </summary>
    /// <param name="_player">Reference to the player object.</param>
    /// <param name="_stateMachine">Reference to the state machine managing this state.</param>
    /// <param name="_animBoolName">Name of the animation boolean associated with this state.</param>
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
    {
    }

    /// <summary>
    /// Called when the state is entered. 
    /// This method makes the player invincible, creates dust particles if on the ground, 
    /// and triggers the clone-on-dash skill effect.
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        //TODO player.stats.MakeInvincible(true); // Make the player invincible during the dash

        // Create dust particles if the player is on the ground

        player.sr.material = player.dashMat; // Change the player's material to the dash material

        if (player.IsGroundDetected())
            player.fx.CreateDustParticles(DustParticleType.Running);

        // // Trigger the skill effect for cloning during the dash
        // player.skill.dash.CloneOnDash();

        // Set the timer for how long the dash lasts
        stateTimer = player.dashDuration;
        stateName = "Dash"; // Set the state name for debugging or logging purposes
        
        AudioManager.Instance.PlaySFX("Player_Dash"); // Play the dash sound effect
    }

    /// <summary>
    /// Called when the state is exited.
    /// This method removes the player's invincibility, stops the dash movement, 
    /// and triggers the clone-on-arrival skill effect.
    /// </summary>
    public override void Exit()
    {
        //TODO player.stats.MakeInvincible(false); // Remove invincibility after the dash
        if (!jumpingOut)
        {
            player.SetVelocity(0, rb.linearVelocity.y); // Stop horizontal movement but retain vertical velocity
        }
        // player.skill.dash.CloneOnArrival(); // Trigger the skill effect for cloning on arrival
        base.Exit();
    }

    /// <summary>
    /// Called every frame during the dash state.
    /// This method handles trail creation, state transitions, and dash movement.
    /// </summary>
    public override void Update()
    {
        base.Update();

        CreateTrailAfterImage(); // Create a trail effect for the dash

        

        // If the player is airborne and detects a wall, transition to the wall slide state
        // if (!player.IsGroundDetected() && player.IsWallDetected())
        //     stateMachine.ChangeState(player.wallSlideState);

        // Apply dash movement
        player.SetVelocity(player.dashSpeed * player.dashDir, 0);
        
        if (player.IsGroundDetected() && Input.GetKeyDown(KeyCode.Space))
        {
            player.fx.CreateDustParticles(DustParticleType.Landing);
            player.fx.ScreenShake(player.fx.lightShakePower);
            jumpingOut = true; // Set the flag to indicate jumping out of the dash
            stateMachine.ChangeState(player.jumpState);
        }

        // Transition to the idle state when the dash duration is over
        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
