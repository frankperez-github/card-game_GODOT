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
            if (mainMenu.Inventary.player1.life == 0 && mainMenu.Inventary.player1.life == 0)
            {
                winner = "Empate";
            }
            else if (mainMenu.Inventary.player1.life == 0)
            {
                winner = mainMenu.Inventary.player1.nick+" wins";
            }
            else
            {
                winner = mainMenu.Inventary.player2.nick+" wins";
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