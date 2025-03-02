//-----------------------------------------------------------------------
// <copyright file="TestLevel.cs" company="Radosław Grzanka">
//     Copyright (c) Radosław Grzanka. Licensed under the GPL-3.0 license.
// </copyright>
//-----------------------------------------------------------------------

namespace org.grzanka.Kiddo;
using Godot;
using Org.Grzanka.Kiddo;
using System;
using System.Linq;

public partial class TestLevel : Node2D
{
    private int originalNumberOfCoins;
    private bool allCoinsCollected;

    [Signal]
    public delegate void AllCoinsCollectedEventHandler();

    private int CoinsLeft { get; set; }

    private Node OvaniPlayer { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach (Coin coin in GetNode<Node>("Coins").GetChildren().OfType<Coin>())
        {
            coin.CoinPickedUp += OnCoinCollected;
        }

        foreach (Org.Grzanka.Kiddo.Key key in GetNode<Node>("Keys").GetChildren().OfType<Org.Grzanka.Kiddo.Key>())
        {
            AllCoinsCollected += key.MakeVisible;
        }

        originalNumberOfCoins = GetNode<Node>("Coins").GetChildren().OfType<Coin>().Count(c => c.Visible);
        CoinsLeft = originalNumberOfCoins;
        OvaniPlayer = GetNode<Node>("OvaniPlayer");
    }

    private void OnCoinCollected(Coin pickedupCoin)
    {
        CoinsLeft--;
        double intensity = (originalNumberOfCoins - CoinsLeft) / (double)originalNumberOfCoins;
        OvaniPlayer.Call("FadeIntensity", intensity, 2);

        if (CoinsLeft == 0)
        {
            EmitSignal(nameof(AllCoinsCollected));
        }
    }
}
