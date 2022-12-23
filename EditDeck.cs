using Godot;
using System;

public class EditDeck : Node2D
{
    Button SelectToRemove;
    Button RemoveSelected;
    Button CreateCard;
    public override void _Ready()
    {
        CreateCard = GetNode<Button>("Button/Create");
        
    }

    public override void _Process(float delta)
    {
        if(CreateCard.Pressed)
        {
            GetTree().ChangeScene("res://Editor.tscn");
        }
    }
}
