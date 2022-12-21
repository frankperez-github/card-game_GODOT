using Godot;
using System.Collections.Generic;
using System;
using gameEngine;

namespace gameVisual
{
    public class SelectCards : Tree
    {
        public static Button AcceptButton;
        public static int selectQuant = 1;
        public static List<Sprite> selectCards;
        public static Action<List<Relics>> SelectDelegate;
        public static List<Relics> target;
        public static Label SelectLabel;
        static PackedScene SelectCardsScene = (PackedScene)GD.Load("res://SelectCards.tscn");
        public static Node SelectCardInstance = SelectCardsScene.Instance(); 

        public override void _Ready()
        {
            PauseMode = PauseModeEnum.Process;
            AcceptButton = GetNode<Button>("Button");
            SelectLabel = GetNode<Label>("DiscardLabel");
            SelectLabel.Text = "Select: " + selectQuant;
            SelectLabel.Visible = true;
            VisualMethods.selecting = true;
        }

        public override void _Process(float delta)
        {
            // SHOW SELECT CARDS SCENE
            if(selectQuant == 0)
            {
                AcceptButton.Visible = true;
            }
            else
            {
                VisualMethods.selecting = true;
                AcceptButton.Visible = false;
            }

            // Selection Finished
            if(AcceptButton.Pressed)
            {
                board.child.GetTree().Paused = false;
                this.QueueFree();
                SelectDelegate(target);
            }

        }
        public override void _Input(InputEvent @event)
        {
            // Click
            if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
            {
                switch ((ButtonList)mouseEvent.ButtonIndex)
                {
                    case ButtonList.Left:
                        if (VisualMethods.selecting)
                        {
                            for(int i = 0; i < selectCards.Count; i++)
                            {
                                if (selectCards[i].GetRect().HasPoint(selectCards[i].ToLocal(mouseEvent.Position)))
                                {
                                    // Deselecting
                                    if (selectCards[i].Scale == new Vector2((float)0.30,(float)0.30))
                                    {
                                        selectQuant++;
                                        VisualMethods.SelectedCards.Remove(VisualMethods.SourceToSelect[i]);
                                        selectCards[i].Scale = new Vector2((float)0.25,(float)0.25);
                                    }
                                    // Selecting
                                    else if (selectQuant != 0)
                                    {
                                        // GD.Print(selectCards[i].Scale);
                                        selectQuant--;
                                        VisualMethods.SelectedCards.Add(VisualMethods.SourceToSelect[i]);
                                        selectCards[i].Scale = new Vector2((float)0.30,(float)0.30);
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                }
            }
            
            // Pause Menu
            if (@event is InputEventKey eventKey && eventKey.Scancode == (int)KeyList.Escape)
            {
                VisualMethods.ActiveEscapeMenu();
            }
        }
    }
}