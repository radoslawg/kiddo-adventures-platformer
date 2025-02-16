//-----------------------------------------------------------------------
// <copyright file="Player.cs" company="Radosław Grzanka">
//     Copyright (c) Radosław Grzanka. Licensed under the GPL-3.0 license.
// </copyright>
//-----------------------------------------------------------------------
namespace Org.Grzanka.Kiddo;

using Godot;

public partial class Player : CharacterBody2D
{
    [Export]
    public float Speed { get; set; } = 300.0f;

    [Export]
    public float Decelleration { get; set; } = 5.0f;

    [Export]
    public float JumpVelocity { get; set; } = -400.0f;

    private AnimatedSprite2D AnimatedSprite2D { get; set; }

    public override void _Ready()
    {
        AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction.X != Vector2.Zero.X && IsOnFloor())
        {
            AnimatedSprite2D.Play("walk");
        }
        else
        {
            if (IsOnFloor())
            {
                AnimatedSprite2D.Play("default");
            }
        }

        if (direction != Vector2.Zero)
        {
            if (direction.X < 0)
            {
                bool flipping = AnimatedSprite2D.Scale.X > 0;
                if (flipping)
                {
                    AnimatedSprite2D.Scale = new Vector2(AnimatedSprite2D.Scale.X * -1, AnimatedSprite2D.Scale.Y);
                }
            }
            else
            {
                bool flipping = AnimatedSprite2D.Scale.X < 0;
                if (flipping)
                {
                    AnimatedSprite2D.Scale = new Vector2(AnimatedSprite2D.Scale.X * -1, AnimatedSprite2D.Scale.Y);
                }
            }

            velocity.X = direction.X * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Decelleration);
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
            AnimatedSprite2D.Play("jump");
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
