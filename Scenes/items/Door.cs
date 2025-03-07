//-----------------------------------------------------------------------
// <copyright file="Door.cs" company="Radosław Grzanka">
//     Copyright (c) Radosław Grzanka. Licensed under the GPL-3.0 license.
// </copyright>
//-----------------------------------------------------------------------

namespace Org.Grzanka.Kiddo;

using System.Threading.Tasks;
using Godot;

public partial class Door : Node2D
{
    [Export(PropertyHint.File, "*.tscn")]
    public string NextLevel { get; set; }

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

    public async Task GoToNextLevel()
    {
        SceneTree tree = GetTree();
        tree.Paused = true;
        Transitions.Instance.PlayExitTransition();
        await ToSignal(Transitions.Instance, "TransitionCompleted");
        tree.ChangeSceneToFile(NextLevel);
        Transitions.Instance.PlayEnterTransition();
        tree.Paused = false;
    }

    public void OnBodyEntered(Node body)
    {
        if (body is Player && IsOpen)
        {
            ((Player)body).IsOnDoor = this;
        }
    }
}
