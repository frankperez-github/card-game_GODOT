using Godot;
using System;
using System.Collections.Generic;

namespace card_gameEngine
{
   
    public class mainMenu : Node2D
    {
        public override void _Process(float delta)
        {
            Button Start = GetNode<Button>("Start");
            if (Start.Pressed)
            {
                GetTree().ChangeScene("res://board.tscn");
            }
        }
    }
    
}