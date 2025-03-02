//-----------------------------------------------------------------------
// <copyright file="TestLevel.cs" company="Radosław Grzanka">
//     Copyright (c) Radosław Grzanka. Licensed under the GPL-3.0 license.
// </copyright>
//-----------------------------------------------------------------------

namespace org.grzanka.Kiddo;
using Godot;
using System;

public partial class TestLevel : Node2D
{
    private int originalNumberOfCoins;
    private bool allCoinsCollected;

    private int CurrentCoins { get; set; }

    private Node OvaniPlayer { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        originalNumberOfCoins = GetNode<Node>("Coins").GetChildCount();
        CurrentCoins = originalNumberOfCoins;
        OvaniPlayer = GetNode<Node>("OvaniPlayer");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        int newCoins = GetNode<Node>("Coins").GetChildCount();
        if (newCoins != CurrentCoins)
        {
            CurrentCoins = newCoins;
            double intensity = (originalNumberOfCoins - CurrentCoins) / (double)originalNumberOfCoins;
            OvaniPlayer.Call("FadeIntensity", intensity, 2);
        }

        // Change to one time event
        if (CurrentCoins == 0 && !allCoinsCollected)
        {
            allCoinsCollected = true;
            foreach (Node key in GetNode<Node>("Keys").GetChildren())
            {
                if (key is Area2D)
                {
                    ((Area2D)key).Visible = true;
                }
            }
        }
    }
}
