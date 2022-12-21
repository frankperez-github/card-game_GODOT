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
            PauseMode = PauseModeEnum.Process;
            Quit = GetNode<Button>("PauseMenu/Continue/Quit");
            Continue = GetNode<Button>("PauseMenu/Continue");
            Capitulation = GetNode<Button>("PauseMenu/Continue/Capitulation");
        }
        //List<>
        public override void _Process(float delta)
        {
            if (Quit.Pressed)
            {
                GetTree().Quit();
            }
            if (Continue.Pressed)
            {
                this.QueueFree();
            }
            if (Capitulation.Pressed)
            {
                VisualMethods.resetVisualGame();
                GetTree().ChangeScene("res://mainMenu.tscn");
            }
            
        }
    }
}
