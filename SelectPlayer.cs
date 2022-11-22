using Godot;
using System.Collections.Generic;

namespace gameVisual
{
    public class SelectPlayer : Node
    {
        // Called when the node enters the scene tree for the first time.
        Vector2[] charactPositions = new Vector2[5]
        {
            new Vector2(200,200),
            new Vector2(350, 200),
            new Vector2(500,200),
            new Vector2(650,200),
            new Vector2(800,200)
        };

        gameEngine.Character character1;
        List<Sprite> characters = new List<Sprite>();
        TextEdit Nick;
        Label choose;

        public override void _Ready()
        {
            int index = 0;
            foreach(var character in gameEngine.Settings.CharactersInventary)
            {
                Sprite Character = gameVisual.board.InstanciateVisualCard(character);
                AddChild(Character);
                characters.Add(Character);
                Character.Position = charactPositions[index];
                index++;
            }
        }
       
        public override void _Process(float delta)
        {
            Button Continue = GetNode<Button>("Tree/Continue"); 
            Nick = GetNode<TextEdit>("Tree/Label/Nick");
            choose = GetNode<Label>("Tree/Choose");
            if (mainMenu.gameType.ToLower() == "virtual")
            {
                choose.Text = "Player 2 choose your character: ";
            }

            if (Continue.Pressed)
            {
                if (Nick.Text != "")
                {
                    gameEngine.Player player = new gameEngine.Player(character1, Nick.Text);
                    gameEngine.Settings.PlayersInventary.Add(player);
                    if (mainMenu.gameType.ToLower() == "human")
                    {
                        GetTree().ChangeScene("res://SelectPlayer.tscn");
                        mainMenu.gameType = "virtual";
                    }
                    else
                    {
                        GetTree().ChangeScene("res://board.tscn");
                    }
                }
            }
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseButton mouseButton)
            {
                switch ((ButtonList)mouseButton.ButtonIndex)
                {
                    case ButtonList.Left:
                        for (int i = 0; i < characters.Count; i++)
                        {
                            if (characters[i] != null && characters[i].GetRect().HasPoint(characters[i].ToLocal(mouseButton.Position)))
                            {
                                foreach (var charact in characters)
                                {
                                    charact.Scale = new Vector2((float)0.402,(float)0.375);
                                }
                                characters[i].Scale = new Vector2((float)0.5,(float)0.5);

                                character1 = gameEngine.Settings.CharactersInventary[i];
                            }
                        }
                        break;
                }
            }
        }
    }

}