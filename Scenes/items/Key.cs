//-----------------------------------------------------------------------
// <copyright file="Key.cs" company="Radosław Grzanka">
//     Copyright (c) Radosław Grzanka. Licensed under the GPL-3.0 license.
// </copyright>
//-----------------------------------------------------------------------

namespace Org.Grzanka.Kiddo;

using Godot;
using org.grzanka.Kiddo.Audio;

public partial class Key : Area2D
{
    [Signal]
    public delegate void KeyPickedUpEventHandler(Key key);

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
        if (body is Player && GetNode<AnimatedSprite2D>("Key").Visible && Visible)
        {
            KeyPickup();
        }
    }

    public void MakeVisible()
    {
        Visible = true;
    }

    private void KeyPickup()
    {
        Visible = false;

        // LevelOverlay levelManager = GetNode<LevelOverlay>("%LevelOverlay");
        // levelManager.IncreaseScore();
        AudioPlayer.Instance.PlaySound(AudioPlayer.Coin);
        AnimatedSprite2D anim = GetNode<AnimatedSprite2D>("PickupAnim");
        anim.Visible = true;
        anim.Play("default");
        EmitSignal(nameof(KeyPickedUp), this);
    }

    private void OnPickupAnimationFinished()
    {
        QueueFree();
    }
}
