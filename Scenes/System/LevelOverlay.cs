//-----------------------------------------------------------------------
// <copyright file="LevelOverlay.cs" company="Radosław Grzanka">
//     Copyright (c) Radosław Grzanka. Licensed under the GPL-3.0 license.
// </copyright>
//-----------------------------------------------------------------------

namespace Org.Grzanka.Kiddo;

using Godot;

public partial class LevelOverlay : Node2D
{
    private int CoinsScore { get; set; }

    private Node Level { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        GetNode<Label>("CanvasLayer/Score").Text = $"{CoinsScore}";
    }

    public void IncreaseScore()
    {
        CoinsScore++;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (Level != null)
            {
                Level.Dispose();
            }
        }

        base.Dispose(disposing);
    }
}
