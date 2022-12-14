using Godot;
using System.Collections.Generic;
using gameEngine;
namespace gameVisual
{
    public class SelectPlayer : Node
    {
        // Called when the node enters the scene tree for the first time.
        
        gameEngine.CharacterProperties character1;
        List<Sprite> characters; 
        Label choose;
        public static Player player1;
        public static RandomVirtPlayer virtualPlayer1;
        public static Player player2;
        

        public override void _Ready()
        {
            Vector2[] charactPositions = new Vector2[5]
            {
                new Vector2(400,400),
                new Vector2(650, 400),
                new Vector2(900,400),
                new Vector2(1150,400),
                new Vector2(1400,400)
            };
            characters = new List<Sprite>();

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

            player1 = new Player("Player1");
            player2 = new Player("Player2");
        }
       
        public override void _Process(float delta)
        {
            
            TextEdit Nick;
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
                    if (mainMenu.gameType.ToLower() == "virtual")
                    {
                        player1 = new RandomVirtPlayer("virtualPl");
                        player1.SetCharacter(character1);

                        player2.SetCharacter(character1);
                        player2.nick = Nick.Text;
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