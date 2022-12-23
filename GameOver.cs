using Godot;
using System;
namespace gameVisual
{
    public class GameOver : Node
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            string winner;
            if (board.Game.player1.life == 0 && board.Game.player2.life == 0)
            {
                winner = "Empate";
            }
            else if (board.Game.player1.life == 0)
            {
                winner = board.Game.player1.nick+" wins";
            }
            else
            {
                winner = board.Game.player2.nick+" wins";
            }
            Label winnerLabel = GetNode<Label>("Tree/wins");
            winnerLabel.Text = winner;

            GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();

        }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
    }
}