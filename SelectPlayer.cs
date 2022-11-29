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

        gameEngine.CharacterProperties character1;
        List<Sprite> characters = new List<Sprite>();
        TextEdit Nick;
        Label choose;

        public override void _Ready()
        {
            for (var i = 0; i < mainMenu.Inventary.CharactersInventary.Count; i++)
            {
                Sprite Character = gameVisual.board.InstanciateVisualCharact(mainMenu.Inventary.CharactersInventary[i]);
                AddChild(Character);
                characters.Add(Character);
                Character.Position = charactPositions[i];
            }
            foreach (var charact in characters)
            {
                charact.Scale = new Vector2((float)0.25,(float)0.26);
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
                        mainMenu.Inventary.player2.SetCharacter(character1);
                        mainMenu.Inventary.player2.nick = Nick.Text;
                    }
                    else
                    {
                        mainMenu.Inventary.player1.SetCharacter(character1);
                        mainMenu.Inventary.player1.nick = Nick.Text;
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
                                    charact.Scale = new Vector2((float)0.24,(float)0.25);
                                }
                                
                                characters[i].Scale = new Vector2((float)0.40,(float)0.41);
                                character1 = mainMenu.Inventary.CharactersInventary[i];
                            }
                        }
                        break;
                }
            }
        }
    }

}