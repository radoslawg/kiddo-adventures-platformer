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
    int currentCoins;

    private Node OvaniPlayer { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        originalNumberOfCoins = GetNode<Node>("Coins").GetChildCount();
        currentCoins = originalNumberOfCoins;
        OvaniPlayer = GetNode<Node>("OvaniPlayer");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        int newCoins = GetNode<Node>("Coins").GetChildCount();
        if (newCoins != currentCoins)
        {
            currentCoins = newCoins;
            double intensity = (originalNumberOfCoins - currentCoins) / (double)originalNumberOfCoins;
            GD.Print(intensity);
            OvaniPlayer.Call("FadeIntensity", intensity, 2);
        }
    }
}
