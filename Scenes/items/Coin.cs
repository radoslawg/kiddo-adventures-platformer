//-----------------------------------------------------------------------
// <copyright file="Coin.cs" company="Radosław Grzanka">
//     Copyright (c) Radosław Grzanka. Licensed under the GPL-3.0 license.
// </copyright>
//-----------------------------------------------------------------------

namespace Org.Grzanka.Kiddo;

using Godot;
using Org.Grzanka.Kiddo.Audio;

public partial class Coin : Area2D
{
    [Signal]
    public delegate void CoinPickedUpEventHandler(Coin coin);

    [Export]
    public AudioStream PickupSound { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void OnBodyEntered(Node body)
    {
        if (body is Player && GetNode<AnimatedSprite2D>("Coin").Visible)
        {
            CoinPickup();
        }
    }

    private void CoinPickup()
    {
        GetNode<AnimatedSprite2D>("Coin").Visible = false;
        LevelOverlay levelManager = GetNode<LevelOverlay>("%LevelOverlay");
        levelManager.IncreaseScore();
        AudioPlayer.Instance.PlaySound(PickupSound);
        AnimatedSprite2D anim = GetNode<AnimatedSprite2D>("PickupAnim");
        anim.Visible = true;
        anim.Play("default");
        EmitSignal(nameof(CoinPickedUp), this);
    }

    private void OnPickupAnimationFinished()
    {
        QueueFree();
    }
}
