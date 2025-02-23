//-----------------------------------------------------------------------
// <copyright file="Coin.cs" company="Radosław Grzanka">
//     Copyright (c) Radosław Grzanka. Licensed under the GPL-3.0 license.
// </copyright>
//-----------------------------------------------------------------------

namespace Org.Grzanka.Kiddo;

using System.Threading.Tasks;
using Godot;
using org.grzanka.Kiddo;
using org.grzanka.Kiddo.Audio;

public partial class Coin : Area2D
{
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
        if (body is Player)
        {
            CoinPickup();
        }
    }

    private void CoinPickup()
    {
        LevelOverlay levelManager = GetNode<LevelOverlay>("%LevelOverlay");
        levelManager.IncreaseScore();
        AudioPlayer.Instance.PlaySound(AudioPlayer.Coin);
        GetNode<AnimatedSprite2D>("Coin").Visible = false;
        AnimatedSprite2D anim = GetNode<AnimatedSprite2D>("PickupAnim");
        anim.Visible = true;
        anim.Play("default");
    }

    private void OnPickupAnimationFinished()
    {
        QueueFree();
    }
}
