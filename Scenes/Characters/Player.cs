//-----------------------------------------------------------------------
// <copyright file="Player.cs" company="Radosław Grzanka">
//     Copyright (c) Radosław Grzanka. Licensed under the GPL-3.0 license.
// </copyright>
//-----------------------------------------------------------------------
namespace Org.Grzanka.Kiddo;

using Godot;
using Org.Grzanka.Kiddo.Audio;

public partial class Player : CharacterBody2D
{
    [Signal]
    public delegate void PlayerDieEventHandler();

    private enum State
    {
        Idle,
        Walk,
        Jump,
        Fall,
    }

    [Export]
    public AudioStream JumpSound { get; set; }

    [Export]
    public float Speed { get; set; } = 300.0f;

    [Export]
    public float Decelleration { get; set; } = 5.0f;

    [Export]
    public float JumpVelocity { get; set; } = -400.0f;

    public Door IsOnDoor { get; set; }

    private AnimatedSprite2D PlayerSprite { get; set; }

    private RemoteTransform2D RemoteTransform2D { get; set; }

    private State CurrentState { get; set; } = State.Idle;

    private bool DoubleJump { get; set; }

    public override void _Ready()
    {
        PlayerSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        RemoteTransform2D = GetNode<RemoteTransform2D>("RemoteTransform2D");
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

        MoveAndSlide();
        TryChangeState();
    }

    public void ConnectCamera(Camera2D camera)
    {
        if (camera == null)
        {
            GD.PrintErr("Camera is null");
            return;
        }

        RemoteTransform2D.RemotePath = camera.GetPath();
    }

    private void TryChangeState()
    {
        Vector2 direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");

        if (Input.IsActionJustPressed("move_jump") && IsOnFloor())
        {
            TransitionTo(State.Jump);
            return;
        }

        if (!IsOnFloor() && Velocity.Y >= 0)
        {
            TransitionTo(State.Fall);
            return;
        }

        if (IsOnDoor != null && IsOnFloor() && Input.IsActionJustPressed("move_enter"))
        {
            IsOnDoor.GoToNextLevel().ConfigureAwait(false);
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
                AudioPlayer.Instance.PlaySound(JumpSound);
                DoubleJump = false;
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

        if (Input.IsActionJustPressed("move_jump") && !DoubleJump)
        {
            AudioPlayer.Instance.PlaySound(JumpSound);
            PlayerSprite.Play("jump");
            DoubleJump = true;
            Velocity = Velocity with { Y = JumpVelocity };
        }
    }

    private void HandleWalk(double delta)
    {
        Vector2 direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");

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
        if (Position.Y > 50)
        {
            EmitSignal(nameof(PlayerDie));
        }
    }
}
