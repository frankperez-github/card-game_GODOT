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
        public static RandomVirtPlayer virtualplayer1;
        public static Player player2;
        public Player player1initial;
        public Player player2initial;

        public override void _Ready()
        {
            Vector2[] charactPositions = new Vector2[5]
            {
                new Vector2(400,450),
                new Vector2(650, 450),
                new Vector2(900,450),
                new Vector2(1150,450),
                new Vector2(1400,450)
            };
            characters = new List<Sprite>();

            for (var i = 0; i < mainMenu.Inventory.CharactersInventory.Count; i++)
            {
                Sprite Character = VisualMethods.InstanciateVisualCharact(mainMenu.Inventory.CharactersInventory[i]);
                AddChild(Character);
                characters.Add(Character);
                Character.Position = charactPositions[i];
            }
            foreach (var charact in characters)
            {
                charact.Scale = new Vector2((float)0.25,(float)0.26);
            }

            player1initial = new Player("Player1");
            player2initial = new Player("Player2");
            
            player2initial.Enemy = player1initial;
            player1initial.Enemy = player2initial;

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
                        if (player2initial.character == null) // VS virtual player
                        {
                            player1initial = new OffensiveVirtPlayer("virtualPl");
                            player1initial.SetCharacter(character1);
                            player1initial.Enemy = player2initial;
                            player1 = player1initial;
                            player2initial.nick = Nick.Text;
                            player2initial.SetCharacter(character1);
                            player2initial.Enemy = player1initial;
                            player2 = player2initial;
                        }
                        else // VS human player
                        {
                            player1initial.nick = Nick.Text;
                            player1initial.SetCharacter(character1);
                            player1initial.Enemy = player1initial;
                            player1 = player1initial;
                        }

                    }

                    if (mainMenu.gameType.ToLower() == "human")
                    {
                        //Setting First human player
                        player2initial.nick = Nick.Text;
                        player2initial.SetCharacter(character1);
                        player2initial.Enemy = player1initial;
                        player2 = player2initial;

                        mainMenu.gameType = "virtual";
                        GetTree().ChangeScene("res://SelectPlayer.tscn");
                    }
                    else
                    {
                        this.QueueFree();
                        PackedScene board = (PackedScene)ResourceLoader.Load("res://board.tscn");
                        Node2D boardInstance = (Node2D)board.Instance();
                        this.GetParent().AddChild(boardInstance);
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
                                    charact.Scale = new Vector2((float)0.17,(float)0.18);
                                }
                                
                                characters[i].Scale = new Vector2((float)0.27,(float)0.28);
                                character1 = mainMenu.Inventory.CharactersInventory[i];
                            }
                        }
                        break;
                }
            }
        }
    }

}