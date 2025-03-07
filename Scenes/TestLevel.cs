//-----------------------------------------------------------------------
// <copyright file="TestLevel.cs" company="Radosław Grzanka">
//     Copyright (c) Radosław Grzanka. Licensed under the GPL-3.0 license.
// </copyright>
//-----------------------------------------------------------------------

namespace Org.Grzanka.Kiddo;
using Godot;
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

        foreach (Key key in GetNode<Node>("Keys").GetChildren().OfType<Key>())
        {
            AllCoinsCollected += key.MakeVisible;
            key.KeyPickedUp += GetNode<Door>("Door").Open;
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
