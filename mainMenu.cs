using Godot;
using System;
using System.Collections.Generic;

namespace gameVisual
{
   
    public class mainMenu : Node2D
    {
        public static string gameType = "virtual";


        public override void _Ready()
        {
            gameEngine.Settings.SetConfig();
        }
        public override void _Process(float delta)
        {
            Button Start = GetNode<Button>("Start");
            if (Start.Pressed)
            {
                GetTree().ChangeScene("res://SelectPlayer.tscn");
            }
            Button HumanPlayer = GetNode<Button>("Start/Human");

            if (HumanPlayer.Pressed)
            {
                gameType = "Human";
                GetTree().ChangeScene("res://SelectPlayer.tscn");
            }

            Button Quit = GetNode<Button>("Start/Quit");
            if (Quit.Pressed)
            {
                GetTree().Quit();
            }
        }
    }
    
}