using Godot;
using System;

namespace Org.Grzanka.Kiddo;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed { get; set; } = 300.0f;
	[Export]
	public float Decelleration { get; set; } = 5.0f;
	[Export]
	public float JumpVelocity { get; set; } = -400.0f;

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
			this.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("walk");
		}
		else
		{
			if (IsOnFloor())
			{
				this.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("default");
			}
		}
		if (direction != Vector2.Zero)
		{

			if (direction.X < 0)
			{
				bool flipping = this.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Scale.X > 0;
				if (flipping)
				{
					this.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Scale = new Vector2(this.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Scale.X * -1, this.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Scale.Y);
				}

			}
			else
			{
				bool flipping = this.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Scale.X < 0;
				if (flipping)
				{
					this.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Scale = new Vector2(this.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Scale.X * -1, this.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Scale.Y);
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
			this.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("jump");
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
