using Godot;
using System.Collections.Generic;

namespace gameVisual
{
    public class SelectPlayer : Node
    {
        // Called when the node enters the scene tree for the first time.
        Vector2[] charactPositions = new Vector2[5]
        {
            new Vector2(400,400),
            new Vector2(650, 400),
            new Vector2(900,400),
            new Vector2(1150,400),
            new Vector2(1400,400)
        };

        gameEngine.Character character1;
        List<Sprite> characters = new List<Sprite>();
        TextEdit Nick;
        Label choose;

        public static gameEngine.Inventary Inventary = new gameEngine.Inventary();

        public override void _Ready()
        {
            int index = 0;
            foreach(var character in Inventary.CharactersInventary)
            {
                Sprite Character = gameVisual.board.InstanciateVisualCard(character);
                AddChild(Character);
                characters.Add(Character);
                Character.Position = charactPositions[index];
                index++;
            }
            foreach (var charact in characters)
            {
                charact.Scale = new Vector2((float)0.097,(float)0.097);
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
                    if  (mainMenu.gameType.ToLower() == "virtual")
                    {
                        Inventary.player2.character = character1;
                        Inventary.player2.nick = Nick.Text;
                    }
                    else
                    {
                        Inventary.player1.character = character1;
                        Inventary.player1.nick = Nick.Text;
                    }

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
                                    charact.Scale = new Vector2((float)0.097,(float)0.097);
                                }
                                
                                characters[i].Scale = new Vector2((float)0.12,(float)0.12);
                                character1 = Inventary.CharactersInventary[i];
                            }
                        }
                        break;
                }
            }
        }
    }

}