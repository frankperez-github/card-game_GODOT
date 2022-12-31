using Godot;
using System.Collections.Generic;
using System;
using gameEngine;
namespace gameVisual
{
    public class SelectCards : Sprite
    {
        public static Button AcceptButton;
        Button Left;
        Button Right;
        public static int selectQuant = 1;
        public static List<Sprite> selectCards;
        public static Action<List<Relics>, int> SelectDelegate;
        public static InterpretAction action;
        public static Label SelectLabel;
        public static PackedScene SelectCardsScene = (PackedScene)GD.Load("res://SelectCards.tscn");
        public static Node SelectCardInstance = SelectCardsScene.Instance(); 
        public static Dictionary<int, List<int>> SelectedIndexes = new Dictionary<int, List<int>>();

        public static List<Relics> Source;
        public static int actualSwipe = 0;
        public static int partitions = 0;
        public static string selectName;

        public override void _Ready()
        {
            board.music.PauseMode = PauseModeEnum.Process;
            PauseMode = PauseModeEnum.Process;
            AcceptButton = GetNode<Button>("Tree/Button");
            SelectLabel = GetNode<Label>("Tree/DiscardLabel");
            SelectLabel.Visible = true;
            VisualMethods.selecting = true;

            Left = GetNode<Button>("Left");
            Right = GetNode<Button>("Left/Right");
        }

        public override void _Process(float delta)
        {
            // SHOW SELECT CARDS SCENE
            if(selectQuant == 0 || selectCards.Count == 0)
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
                partitions = 0;
                board.child.GetTree().Paused = false;
                SelectedIndexes = new Dictionary<int, List<int>>();
                if(action != null)
                {
                    action.Effect();
                    board.VisualBoard.Update();
                    action = null;
                }
                else
                {
                    SelectDelegate(VisualMethods.SelectedCards, -1);
                }
                this.QueueFree();
            }

            Left.Disabled = false;
            if (actualSwipe == 0) Left.Disabled = true;
            Right.Disabled = false;
            if (actualSwipe+1 == partitions) Right.Disabled = true;

            if(Left.Pressed)
            {
                VisualMethods.SetSelectCardsProperties(selectName, partitions, actualSwipe-1, Source, selectQuant);
            }
            if(Right.Pressed)
            {
                VisualMethods.SetSelectCardsProperties(selectName, partitions, actualSwipe+1, Source, selectQuant);
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
                        if (VisualMethods.selecting && selectQuant >= 0)
                        {
                            for(int i = 0; i < selectCards.Count; i++)
                            {
                                if (selectCards[i].GetRect().HasPoint(selectCards[i].ToLocal(mouseEvent.Position)))
                                {
                                    // Deselecting
                                    if (selectCards[i].Scale == new Vector2((float)0.185,(float)0.185))
                                    {
                                        // Removing selected index from partition
                                        List<int> updatedIndexes = SelectedIndexes[actualSwipe];
                                        updatedIndexes.Remove(i);
                                        SelectedIndexes[actualSwipe] = updatedIndexes;

                                        selectQuant++;
                                        VisualMethods.SelectedCards.Remove(SelectCards.Source[i+actualSwipe*VisualMethods.partitionLength]);
                                        selectCards[i].Scale = new Vector2((float)0.170,(float)0.170);
                                        GD.Print(String.Join(" ", SelectedIndexes[actualSwipe]));
                                    }
                                    // Selecting
                                    else if (selectQuant != 0)
                                    {
                                        // Adding selected index to partition
                                        List<int> updatedIndexes = SelectedIndexes[actualSwipe];
                                        updatedIndexes.Add(i);
                                        SelectedIndexes[actualSwipe] = updatedIndexes;

                                        selectQuant--;
                                        VisualMethods.SelectedCards.Add(SelectCards.Source[i+actualSwipe*VisualMethods.partitionLength]);
                                        selectCards[i].Scale = new Vector2((float)0.185,(float)0.185);
                                        GD.Print(String.Join(" ", SelectedIndexes[actualSwipe]));

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
    public class Select
    {
        public PackedScene SelectCardsScene;
        public InterpretAction action;
        public Action<List<Relics>, int> SelectDelegate;
        public Label SelectLabel;
        public List<Sprite> selectCards;
        public int selectQuant;
        public static Node SelectCardInstance;
        Dictionary<int, List<Relics>> Source; 


        public Select(InterpretAction action, Label selectLabel, int quant, List<Relics> Sourse)
        {
            SelectCardsScene = (PackedScene)GD.Load("res://SelectCards.tscn");
            SelectCardInstance = SelectCardsScene.Instance();
            this.action = action;
            SelectLabel = selectLabel;
            selectCards = new List<Sprite>();
            selectQuant = quant;
        }

        public Select(Action<List<Relics>, int> selectDelegate, Label selectLabel, int quant, List<Relics> Sourse)
        {
            SelectCardsScene = (PackedScene)GD.Load("res://SelectCards.tscn");
            SelectCardInstance = SelectCardsScene.Instance();
            SelectDelegate = selectDelegate;
            SelectLabel = selectLabel;
            selectCards = new List<Sprite>();
            selectQuant = quant;
        }
    }
}