//-----------------------------------------------------------------------
// <copyright file="Player.cs" company="Radosław Grzanka">
//     Copyright (c) Radosław Grzanka. Licensed under the GPL-3.0 license.
// </copyright>
//-----------------------------------------------------------------------
namespace Org.Grzanka.Kiddo;

using Godot;

public partial class Player : CharacterBody2D
{
    private AnimatedSprite2D animatedSprite2D;

    [Export]
    public float Speed { get; set; } = 300.0f;

    [Export]
    public float Decelleration { get; set; } = 5.0f;

    [Export]
    public float JumpVelocity { get; set; } = -400.0f;

    public override void _Ready()
    {
        animatedSprite2D = this.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
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
            animatedSprite2D.Play("walk");
        }
        else
        {
            if (IsOnFloor())
            {
                animatedSprite2D.Play("default");
            }
        }

        if (direction != Vector2.Zero)
        {
            if (direction.X < 0)
            {
                bool flipping = animatedSprite2D.Scale.X > 0;
                if (flipping)
                {
                    animatedSprite2D.Scale = new Vector2(animatedSprite2D.Scale.X * -1, animatedSprite2D.Scale.Y);
                }
            }
            else
            {
                bool flipping = animatedSprite2D.Scale.X < 0;
                if (flipping)
                {
                    animatedSprite2D.Scale = new Vector2(animatedSprite2D.Scale.X * -1, animatedSprite2D.Scale.Y);
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
            animatedSprite2D.Play("jump");
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
