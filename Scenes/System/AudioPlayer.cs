//-----------------------------------------------------------------------
// <copyright file="AudioPlayer.cs" company="Radosław Grzanka">
//     Copyright (c) Radosław Grzanka. Licensed under the GPL-3.0 license.
// </copyright>
//-----------------------------------------------------------------------

namespace org.grzanka.Kiddo.Audio;

using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class AudioPlayer : Node
{
    public static AudioPlayer Instance { get; private set; }

    private List<AudioStreamPlayer2D> AudioPlayers { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance = this;
        AudioPlayers = GetNode<Node>("AudioPlayers")
            .GetChildren()
            .Cast<AudioStreamPlayer2D>()
            .ToList();
    }

    public void PlaySound(AudioStream sound)
    {
        foreach (var audioStreamPlayer in AudioPlayers)
        {
            if (!audioStreamPlayer.IsPlaying())
            {
                audioStreamPlayer.Stream = sound;
                audioStreamPlayer.Play();
                return;
            }
        }
    }
}
