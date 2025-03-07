//-----------------------------------------------------------------------
// <copyright file="LevelBase.cs" company="Radosław Grzanka">
//     Copyright (c) Radosław Grzanka. Licensed under the GPL-3.0 license.
// </copyright>
//-----------------------------------------------------------------------

namespace Org.Grzanka.Kiddo;

using Godot;
using System.Linq;

public partial class LevelBase : Node2D
{
    private int originalNumberOfCoins;

    private bool allCoinsCollected;

    [Signal]
    public delegate void AllCoinsCollectedEventHandler();

    [Export]
    public PackedScene PlayerScene { get; set; }

    private Player Player { get; set; }

    private Vector2 PlayerSpawnLocation { get; set; }

    private int CoinsLeft { get; set; }

    private Node OvaniPlayer { get; set; }

    public override void _Ready()
    {
        RenderingServer.SetDefaultClearColor(new Color(0.1f, 0.1f, 1.0f));
        Player = GetNode<Player>("Player");
        ConnectCamera();
        PlayerSpawnLocation = Player.GlobalPosition;
        Player.PlayerDie += OnPlayerDie;

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

    private void ConnectCamera()
    {
        Camera2D camera = GetNode<Camera2D>("Camera2D");
        Player.ConnectCamera(camera);
    }

    private void OnPlayerDie()
    {
        Player.QueueFree();
        Player = PlayerScene.Instantiate<Player>();
        Player.Position = PlayerSpawnLocation;
        AddChild(Player);
        ConnectCamera();
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
