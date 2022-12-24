using Godot;
using System.Collections.Generic;
using System;
namespace gameVisual
{
    public class EditDeck : Node2D
    {
        Button Menu;
        Button SelectToRemove;
        Button RemoveSelected;
        Button CreateCard;
        Label Inventory;
        List<Sprite> CardsToSelect;
        List<gameEngine.Relics> SelectedCards;
        bool selecting;
        public override void _Ready()
        {
            CardsToSelect = new List<Sprite>();
            SelectedCards = new List<gameEngine.Relics>();

            Vector2 FirstPosition = new Vector2(100, 100);

            CreateCard = GetNode<Button>("Select/Create");
            Inventory = GetNode<Label>("Inventory Label");
            Menu = GetNode<Button>("Select/Menu");
            SelectToRemove = GetNode<Button>("Select");
            RemoveSelected = GetNode<Button>("Select/Remove");

            selecting = false;

            int index = 0;
            foreach (var card in mainMenu.Inventory.CardsInventory)
            {
                Sprite relic = VisualMethods.InstanciateVisualCard(card);
                relic.Position = new Vector2(FirstPosition.x + index, FirstPosition.y);
                Inventory.AddChild(relic);
                CardsToSelect.Add(relic);
                index+=200;
                if(index > 1400)
                {
                    FirstPosition.y += 330;
                    index = 0;
                }
            }
        }

        public override void _Process(float delta)
        {
            if (CreateCard.Pressed)
            {
                GetTree().ChangeScene("res://Editor.tscn");
            }
            if (Menu.Pressed)
            {
                GetTree().ChangeScene("res://mainMenu.tscn");
            }
            if (SelectToRemove.Pressed)
            {
                RemoveSelected.Visible = true;
                SelectToRemove.Visible = true;
                selecting = true;
            }
            if (RemoveSelected.Pressed)
            {
                foreach (var relic in SelectedCards)
                {
                    mainMenu.Inventory.CardsInventory.Remove(relic);
                }
                mainMenu.Inventory.OverrideJson();
                this.QueueFree();
                GetTree().ChangeScene("EditDeck.tscn");
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
                        if (selecting)
                        {
                            for(int i = 0; i < CardsToSelect.Count; i++)
                            {
                                if (CardsToSelect[i].GetRect().HasPoint(CardsToSelect[i].ToLocal(mouseEvent.Position)))
                                {
                                    if (CardsToSelect[i].GetRect().HasPoint(CardsToSelect[i].ToLocal(mouseEvent.Position)))
                                    {
                                        // Deselecting
                                        if (CardsToSelect[i].Scale == new Vector2((float)0.20,(float)0.20))
                                        {
                                            // Removing selected cards from Inventory
                                            SelectedCards.Remove(mainMenu.Inventory.CardsInventory[i]);
                                            CardsToSelect[i].Scale = new Vector2((float)0.170,(float)0.170);
                                        }
                                        // Selecting to remove
                                        else
                                        {
                                            SelectedCards.Add(mainMenu.Inventory.CardsInventory[i]);
                                            CardsToSelect[i].Scale = new Vector2((float)0.20,(float)0.20);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }
    }
    
}
