//-----------------------------------------------------------------------
// <copyright file="Player.cs" company="Radosław Grzanka">
//     Copyright (c) Radosław Grzanka. Licensed under the GPL-3.0 license.
// </copyright>
//-----------------------------------------------------------------------
namespace Org.Grzanka.Kiddo;

using System;
using System.Runtime.InteropServices;
using Godot;
using org.grzanka.Kiddo.States;

public partial class Player : CharacterBody2D
{
    private enum State
    {
        Idle,
        Walk,
        Jump,
        Fall,
    }

    [Export]
    public float Speed { get; set; } = 300.0f;

    [Export]
    public float Decelleration { get; set; } = 5.0f;

    [Export]
    public float JumpVelocity { get; set; } = -400.0f;

    private AnimatedSprite2D PlayerSprite { get; set; }

    private State CurrentState { get; set; } = State.Idle;

    public override void _Ready()
    {
        PlayerSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        ReloadIfOutOfBounds();

        switch (CurrentState)
        {
            case State.Idle:
                // HandleIdle(delta); // does nothing for now.
                break;
            case State.Walk:
                HandleWalk(delta);
                break;
            case State.Jump:
                HandleJump(delta);
                break;
            case State.Fall:
                HandleFall(delta);
                break;
        }

        TryChangeState();
        MoveAndSlide();
    }

    private void TryChangeState()
    {
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            TransitionTo(State.Jump);
            return;
        }

        if (!IsOnFloor() && CurrentState != State.Jump)
        {
            TransitionTo(State.Fall);
            return;
        }

        if (IsOnFloor() && (direction.X != Vector2.Zero.X || Velocity.X != Vector2.Zero.X))
        {
            TransitionTo(State.Walk);
            return;
        }

        if (IsOnFloor() && Velocity == Vector2.Zero)
        {
            TransitionTo(State.Idle);
            return;
        }
    }

    private void TransitionTo(State state)
    {
        if (CurrentState == state)
        {
            return;
        }

        switch (state)
        {
            case State.Idle:
                PlayerSprite.Play("default");
                CurrentState = State.Idle;
                break;
            case State.Walk:
                PlayerSprite.Play("walk");
                CurrentState = State.Walk;
                break;
            case State.Jump:
                Velocity = Velocity with { Y = JumpVelocity };
                PlayerSprite.Play("jump");
                CurrentState = State.Jump;
                break;
            case State.Fall:
                PlayerSprite.Play("fall");
                CurrentState = State.Fall;
                break;
        }

        CurrentState = state;
    }

    private void HandleFall(double delta)
    {
        if (!IsOnFloor())
        {
            Velocity += GetGravity() * (float)delta;
        }
    }

    private void HandleJump(double delta)
    {
        HandleWalk(delta);
        if (!IsOnFloor())
        {
            Velocity += GetGravity() * (float)delta;
        }
    }

    private void HandleWalk(double delta)
    {
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        if (direction != Vector2.Zero)
        {
            if (direction.X < 0)
            {
                bool flipping = PlayerSprite.Scale.X > 0;
                if (flipping)
                {
                    PlayerSprite.Scale = new Vector2(PlayerSprite.Scale.X * -1, PlayerSprite.Scale.Y);
                }
            }
            else
            {
                bool flipping = PlayerSprite.Scale.X < 0;
                if (flipping)
                {
                    PlayerSprite.Scale = new Vector2(PlayerSprite.Scale.X * -1, PlayerSprite.Scale.Y);
                }
            }

            Velocity = Velocity with { X = direction.X * Speed };
        }
        else
        {
            Velocity = Velocity with { X = Mathf.MoveToward(Velocity.X, 0, Decelleration) };
        }
    }

    private void ReloadIfOutOfBounds()
    {
        if (Position.Y > 150)
        {
            GetTree().ReloadCurrentScene();
        }
    }
}
