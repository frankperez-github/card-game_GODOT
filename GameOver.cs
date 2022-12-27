using Godot;
using System;
namespace gameVisual
{
    public class GameOver : Node
    {
        Button MainMenu;
        public override void _Ready()
        {
            string winner;
            if (board.Game.player1.life <= 0 && board.Game.player2.life <= 0)
            {
                winner = "Empate";
            }
            else if (board.Game.player1.life <= 0)
            {
                winner = board.Game.player2.nick+" wins";
            }
            else
            {
                winner = board.Game.player1.nick+" wins";
            }
            Label winnerLabel = GetNode<Label>("Tree/wins");
            winnerLabel.Text = winner;

            // GetNode<AudioStreamPlayer2D>("Tree/AudioStreamPlayer2D").Play();
            MainMenu = GetNode<Button>("Tree/Button");

        }

        public override void _Process(float delta)
        {
            if(MainMenu.Pressed)
            {
                VisualMethods.resetVisualGame();
                GetTree().ChangeScene("res://mainMenu.tscn");
            }
        }
    }
}