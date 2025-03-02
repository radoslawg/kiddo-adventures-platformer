//-----------------------------------------------------------------------
// <copyright file="Door.cs" company="Radosław Grzanka">
//     Copyright (c) Radosław Grzanka. Licensed under the GPL-3.0 license.
// </copyright>
//-----------------------------------------------------------------------

namespace Org.Grzanka.Kiddo;

using Godot;

public partial class Door : Node2D
{
    public bool IsOpen { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void Open(Key key)
    {
        IsOpen = true;
        GetNode<TileMapLayer>("Lock").Visible = false;
    }
}
