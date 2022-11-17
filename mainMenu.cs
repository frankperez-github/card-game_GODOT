using Godot;
using System;
using System.Collections.Generic;

namespace card_gameEngine
{
   
    public class mainMenu : Node2D
    {
        public static List<Character> CharactersInventary = new List<Character>();
        public static string gameType = "virtual";


        public override void _Ready()
        {

            #region Defining characters
            CharactersInventary.Add(new Character("El dragón indiferente", 1, 0, "imgpath1", 10, 3));
            CharactersInventary.Add(new Character("El toro alado", 3, 0, "imgpath2", 0, 5));
            CharactersInventary.Add(new Character("La serpiente truhana", 1, 0, "imgpath3", 5, 0));
            CharactersInventary.Add(new Character("El tigre recursivo", 1, 0, "imgpath4", 8, 0));
            CharactersInventary.Add(new Character("El leon amistoso", 2, 0, "imgpath", 0, 1));
            #endregion

        
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