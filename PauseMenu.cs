using Godot;
using System;

namespace gameVisual
{
    public class PauseMenu : Node
    {

        public Button Quit;
        public Button Continue;
        public Button Capitulation;

        public override void _Ready()
        {
            Quit = GetNode<Button>("Tree/Continue/Quit");
            Continue = GetNode<Button>("Tree/Continue");
            Capitulation = GetNode<Button>("Tree/Continue/Capitulation");
        }

        public override void _Process(float delta)
        {
            if (Quit.Pressed)
            {
                GetTree().Quit();
            }
            if (Continue.Pressed)
            {
                board.PauseMenu.QueueFree();
            }
            if (Capitulation.Pressed)
            {
<<<<<<< HEAD
                board.Game.resetGame();
                board.VisualBoard.resetVisualGame();
=======
                // board.VisualBoard.resetVisualGame();
>>>>>>> 205f9adab204bc7f1080a35202acfc56f73cd68f
                GetTree().ChangeScene("res://mainMenu.tscn");
            }
        }
    }
}
