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
        public static Label SelectLabel;
        public static PackedScene SelectCardsScene = (PackedScene)GD.Load("res://SelectCards.tscn");
        public static Node SelectCardInstance = SelectCardsScene.Instance(); 
        public static Select select;

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
            if(select.selectQuant == 0)
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
                if(select.action != null)
                {
                    VisualMethods.SelectedCards = select.selectedCards;
                    select.action.Effect();
                    board.VisualBoard.Update();
                }
                else
                {
                    VisualMethods.SelectedCards = select.selectedCards;
                    select.SelectDelegate(VisualMethods.SelectedCards, -1);
                }
                this.QueueFree();
                SelectCardInstance = null;
            }

            Left.Disabled = false;
            if (select.actualSwipe == 0) Left.Disabled = true;
            Right.Disabled = false;
            if (select.actualSwipe+1 == select.Source.Count) Right.Disabled = true;

            if(Left.Pressed)
            {
                select.RemoveVisualCards();
                select.actualSwipe--;
                SelectCards.select.ShowVisualCards();
            }
            if(Right.Pressed)
            {
                select.RemoveVisualCards();
                select.actualSwipe++;
                SelectCards.select.ShowVisualCards();
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
                        if (VisualMethods.selecting && select.selectQuant >= 0)
                        {
                            for(int i = 0; i < select.selectCards.Count; i++)
                            {
                                if (select.selectCards[i].GetRect().HasPoint(select.selectCards[i].ToLocal(mouseEvent.Position)))
                                {
                                    // Deselecting
                                    if (select.selectCards[i].Scale == new Vector2((float)0.185,(float)0.185))
                                    {
                                        select.selectQuant++;
                                        select.RemoveCard(i);
                                        select.selectCards[i].Scale = new Vector2((float)0.170,(float)0.170);
                                    }
                                    // Selecting
                                    else if (select.selectQuant != 0)
                                    {
                                        select.selectQuant--;
                                        select.AddCard(i);
                                        select.selectCards[i].Scale = new Vector2((float)0.185,(float)0.185);
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
        public InterpretAction action;
        public Action<List<Relics>, int> SelectDelegate;
        public List<Sprite> selectCards = new List<Sprite>();
        public List<Relics> selectedCards = new List<Relics>();
        public int selectQuant;
        public Dictionary<int, List<Relics>> Source;
        public int actualSwipe = 0;
        public string selectName;


        public Select(InterpretAction action, string selectLabel, int quant, List<Relics> Sourse)
        {
            this.action = action;
            selectName = selectLabel;
            selectQuant = quant;
            Source = SetSource(Sourse);
        }
        public Select(Action<List<Relics>, int> selectDelegate, string selectLabel, int quant, List<Relics> Sourse)
        {
            SelectDelegate = selectDelegate;
            selectName = selectLabel;
            selectQuant = quant;
            Source = SetSource(Sourse);
        }
        public Dictionary<int, List<Relics>> SetSource(List<Relics> Source)
        {
            int Swipe = 0;
            Dictionary<int, List<Relics>> result = new Dictionary<int, List<Relics>>();
            if(Source.Count != 0)
            {
                int pointer = 0;
                while(pointer <= Source.Count-1)
                {
                    List<Relics> list = new List<Relics>();
                    for (int i = Swipe*5; i < (Swipe+1)*5; i++)
                    {
                        try
                        {
                            list.Add(Source[i]);
                            pointer++;
                        }
                        catch{}
                    }
                    result.Add(Swipe, list);
                    Swipe++;
                }
            }
            return result;
        }
        public void ShowVisualCards()
        {
            int Count = (5*(Source.Count-1))+Source[Source.Count-1].Count; 
            float InitialPosition = (1010) - ((200 * (Source[actualSwipe].Count-1))/2);
            Vector2 FirstPosition = new Vector2(InitialPosition, 470);
            for (int i = 0; i < Source[actualSwipe].Count; i++)
            {
                Sprite Card = VisualMethods.InstanciateVisualCard(Source[actualSwipe][i]);
                Card.ZIndex = 7;
                selectCards.Add(Card);
                SelectCards.SelectCardInstance.AddChild(Card);
                // Remembering selected cards
                if(selectedCards!=null)
                {
                    if(selectedCards.Contains(Source[actualSwipe][i])) // Selected card
                    {
                        Card.Scale = new Vector2((float)0.185,(float)0.185);
                    }
                    else // Not selected card
                    {
                        Card.Scale = new Vector2((float)0.17,(float)0.17);
                    }
                }
                
                // Adjusting position of cards to quantity in source
                if(i==0)
                {
                    selectCards[i].Position = new Vector2(FirstPosition.x, FirstPosition.y);
                }
                else
                {
                    selectCards[i].Position = new Vector2(selectCards[i-1].Position.x + 200, FirstPosition.y);
                }
            }
        }
        public void RemoveVisualCards()
        {
            int count = selectCards.Count;
            for (int i = 0; i < count; i++)
            {
                selectCards[0].QueueFree();
                selectCards.Remove(selectCards[0]);
            }
        } 
        public void RemoveCard(int position)
        {
            selectedCards.Remove(Source[actualSwipe][position]);
        }
        public void AddCard(int position)
        {
            selectedCards.Add(Source[actualSwipe][position]);
        }
    }
}