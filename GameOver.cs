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
            if (SelectPlayer.Inventary.player1.life == 0 && SelectPlayer.Inventary.player1.life == 0)
            {
                winner = "Empate";
            }
            else if (SelectPlayer.Inventary.player1.life == 0)
            {
                winner = SelectPlayer.Inventary.player1.nick+" wins";
            }
            else
            {
                winner = SelectPlayer.Inventary.player2.nick+" wins";
            }
            Label winnerLabel = GetNode<Label>("Tree/wins");
            winnerLabel.Text = winner;
        }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
    }
}