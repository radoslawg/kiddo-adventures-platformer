//-----------------------------------------------------------------------
// <copyright file="Transitions.cs" company="Radosław Grzanka">
//     Copyright (c) Radosław Grzanka. Licensed under the GPL-3.0 license.
// </copyright>
//-----------------------------------------------------------------------

namespace Org.Grzanka.Kiddo;

using Godot;

public partial class Transitions : CanvasLayer
{
    [Signal]
    public delegate void TransitionCompletedEventHandler();

    public static Transitions Instance { get; private set; }

    private AnimationPlayer AnimationPlayer { get; set; }

    public override void _Ready()
    {
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        Instance = this;
    }

    public void PlayExitTransition()
    {
        AnimationPlayer.Play("ExitLevel");
    }

    public void PlayEnterTransition()
    {
        AnimationPlayer.Play("EnterLevel");
    }

    public void OnAnimationPlayerAnimationFinished(string animationName)
    {
        EmitSignal(nameof(TransitionCompleted));
    }
}
