using Godot;
using System;
using gameEngine;
using System.Collections.Generic;

namespace gameVisual
{
    public class mainMenu : Node2D
    {
        public static string gameType;
        public static Inventory Inventory;

        public override void _Ready()
        {
            gameType = "virtual";
            Inventory = new Inventory();
            GetTree().SetScreenStretch(SceneTree.StretchMode.Mode2d, SceneTree.StretchAspect.Keep, new Vector2(1920, 1080), 1);
        }

        public override void _Process(float delta)
        {
            Button Start = GetNode<Button>("Start");
            if (Start.Pressed)
            {
                GetTree().ChangeScene("res://SelectPlayer.tscn");
                Inventory.ImportJsonContent();
            }

            Button HumanPlayer = GetNode<Button>("Start/Human");
            if (HumanPlayer.Pressed)
            {
                gameType = "Human";
                GetTree().ChangeScene("res://SelectPlayer.tscn");
            }

            Button Edit = GetNode<Button>("Start/Edit");
            if (Edit.Pressed)
            {
                GetTree().ChangeScene("res://Editor.tscn");
            }

            Button Quit = GetNode<Button>("Start/Quit");
            if (Quit.Pressed)
            {
                GetTree().Quit();
            }
        }
    }
    
}