using Godot;
using System.Collections.Generic;
using gameEngine;
namespace gameVisual
{
    public class board : Godot.Node2D
    {   
        #region Visual Objects
        const int maxinHand = 6;


        static List<Sprite> Player1VisualHand = new List<Sprite>();
        static List<Sprite> Player2VisualHand = new List<Sprite>();

        static Vector2[] Player2FieldPositions;
        static bool[] boolPlayer2Field = new bool[4];
        
        static Vector2[] Player1FieldPositions;
        static bool[] boolPlayer1Field = new bool[4];


        static Sprite Relic = new Sprite();


        static int Player1emptySlots = 4;
        static int Player2emptySlots = 4;


        public static bool discarding = false;
        public static gameEngine.Player discardPlayer = new Player("default");
        public static List<Button> discardButtons = new List<Button>();
        static PackedScene DiscardScene = (PackedScene)GD.Load("res://DiscardLabel.tscn");


        public static Vector2 Player1VisualHandPosition;
        public static Vector2 Player2VisualHandPosition;
        #endregion

        public static Game Game;
        public override void _Ready()
        {
            GD.Print(mainMenu.Inventary.CardsInventary.Count);
            Game = new Game(SelectPlayer.player1, SelectPlayer.player2);
            
            Player1FieldPositions = new Vector2[4]
            {
                new Vector2(570, 740),
                new Vector2(860, 740),
                new Vector2(1145, 740),
                new Vector2(1430, 740)
            };
             
            Player2FieldPositions = new Vector2[4]
            {
                new Vector2(570, 275),
                new Vector2(860, 275),
                new Vector2(1145, 275),
                new Vector2(1430, 275)
            };
            
            // setting virtual player's character and nick
            if (mainMenu.gameType.ToLower() == "virtual")
            {
                Game.player1.SetCharacter(mainMenu.Inventary.CharactersInventary[0]);
                Game.player1.nick = "el otro";
            }

            RefreshBoard();
        }

        bool player1Attack = false;
        bool player2Attack = false;
        public override void _Process(float delta)
        {

            Label Turnlabel = GetNode<Label>("TurnLabel");

            // Checking end of game
            if (!(Game.player1.life > 0 && Game.player2.life > 0))
            {
                GetTree().ChangeScene("res://GameOver.tscn");
            }

            Button Attack = GetNode<Button>("Attack");
            Button endButton = GetNode<Button>("endButton");
            
            #region Updating visualy playerInfo
            Label PlayerNick = GetNode<Label>("PlayerInfo/Player's Nick");
            PlayerNick.Text = Game.player1.nick;
            Label PlayerLife = GetNode<Label>("PlayerInfo/Player's Life");
            PlayerLife.Text = "Life : "+ Game.player1.life.ToString();
            Label PlayerShield = GetNode<Label>("PlayerInfo/Player's Shield");
            PlayerShield.Text = "Shield : "+ Game.player1.character.defense.ToString();
            Label PlayerAttack = GetNode<Label>("PlayerInfo/Player's Attack");
            PlayerAttack.Text = "Attack : "+ Game.player1.character.attack.ToString();
            Label Player1State = GetNode<Label>("PlayerInfo/Player's State");
            Player1State.Text = "State : "+ Game.player1.state.ToString();

            Label Player2Nick = GetNode<Label>("Player2Info/Player2's Nick");
            Player2Nick.Text = Game.player2.nick;
            Label Player2Life = GetNode<Label>("Player2Info/Player2's Life");
            Player2Life.Text = "Life : "+ Game.player2.life.ToString();
            Label Player2Shield = GetNode<Label>("Player2Info/Player2's Shield");
            Player2Shield.Text = "Shield : "+ Game.player2.character.defense.ToString();
            Label Player2Attack = GetNode<Label>("Player2Info/Player2's Attack");
            Player2Attack.Text = "Attack : "+ Game.player2.character.attack.ToString();
            Label Player2State = GetNode<Label>("Player2Info/Player2's State");
            Player2State.Text = "State : "+ Game.player2.state.ToString();
            #endregion

            // ATTACK BUTTON
            Attack.Disabled = false;
            AttackButtonFunction(Attack);

            //Change Turn (END BUTTON)
            EndButtonFunction(endButton, Turnlabel);
        }
        public void EndButtonFunction(Button endButton, Label Turnlabel)
        {
            if (endButton.Pressed)
            {
                if (Game.turn % 2 == 0) // Player1's turn
                {
                    // Next Player takes a card
                    Game.player1.TakeFromDeck(1);
                    UpdateBattleField(Game.player1);
                }
                else // Player2's turn
                {
                    // Next Player takes a card
                    Game.player2.TakeFromDeck(1);
                    UpdateBattleField(Game.player2);
                }
                RefreshBoard();
                Game.turn++;
                Turnlabel.Text = "Turno: " + Game.turn;
                endButton.Disabled = true; // Disabling button, increment turn just one time
            }
            endButton.Disabled = false;
        }
        public void AttackButtonFunction(Button Attack)
        {
            if (Attack.Pressed)
            {
                if (Game.turn % 2 == 0 && !player1Attack) // Player1's Turn
                {
                    Game.player2.life -= Game.player1.character.attack;
                    player1Attack = true;
                    player2Attack = false; // Enabling attack button to next turn 
                    Attack.Disabled = true;
                }
                if (Game.turn % 2 != 0 && !player2Attack)// Player2's Turn
                {
                    Game.player1.life -= Game.player2.character.attack;
                    player2Attack = true;
                    player1Attack = false; // Enabling attack button to next turn 
                    Attack.Disabled = true;
                }
            }
        }
        public void RefreshBoard()
        {
            Player1VisualHandPosition = new Vector2(500 - (Game.player1.hand.Count * 5), 1000);
            Player2VisualHandPosition = new Vector2(500 - (Game.player2.hand.Count * 5), 50);
            
            // Erasing old data
            if  (Player1VisualHand.Count != 0)
                Player1VisualHand.Clear();
            if  (Player2VisualHand.Count != 0)
                Player2VisualHand.Clear();

            try
            {
                foreach (Node node in GetTree().GetNodesInGroup("VisibleCards"))
                {
                    node.QueueFree();
                } 
            }
            catch (System.NullReferenceException){}
                
            // Updating cards in board
            UpdateVisualHand(Game.player1);
            UpdateVisualHand(Game.player2);
            if (Game.player1.hand.Count > maxinHand)
            {
                CheckAndDiscard(Game.player1);
            }
            if (Game.player2.hand.Count > maxinHand)
            {
                CheckAndDiscard(Game.player2);
            }
            
        }
        void UpdateVisualHand(gameEngine.Player player)
        {
            List<Sprite> PlayerVisualHand;
            Vector2 PlayerVisualHandPosition;

            if (player == Game.player1)
            {
                PlayerVisualHand = Player1VisualHand;
                PlayerVisualHandPosition = Player1VisualHandPosition;
            }
            else
            {
                PlayerVisualHand = Player2VisualHand;
                PlayerVisualHandPosition = Player2VisualHandPosition;
            }

            for (int i = 0; i < player.hand.Count; i++)
            {
                Relic = InstanciateVisualCard(player.hand[i]);
                Relic.AddToGroup("VisibleCards");
                PlayerVisualHand.Add(Relic);
                Relic.Position = new Vector2(PlayerVisualHandPosition.x + 200*i,  PlayerVisualHandPosition.y);
                AddChild(Relic);
                Label name = (Label) Relic.GetChild(0);
                name.Text = player.hand[i].name;

                if (i >= maxinHand)
                {
                    discarding = true;
                    CheckAndDiscard(player);
                }
            }
        }
        public void CheckAndDiscard(gameEngine.Player player)
        {
            Vector2 FirstDiscardPosition = new Vector2(180, 450);

            discardPlayer = player;
            Tree DiscardTscn = (Tree)DiscardScene.Instance();
            Label label = DiscardTscn.GetNode<Label>("DiscardLabel");
            label.Text = player.nick+" must discard:  "+(player.hand.Count - maxinHand).ToString();
            AddChild(DiscardTscn);
            DiscardTscn.AddToGroup("discardGroup");

            int index = 1;
            foreach (var card in player.hand)
            {
                Sprite relic = InstanciateVisualCard(card);
                Button button = InstanciateButton();
                relic.Scale = new Vector2((float)0.20,(float)0.20);
                AddChild(relic);
                relic.AddToGroup("discardGroup");
                AddChild(button);
                button.AddToGroup("discardGroup");
                discardButtons.Add(button);
                button.SetPosition(new Vector2( 200 * index - 40, FirstDiscardPosition.y + 120) ,false);
                relic.Position = new Vector2( 200 * index, FirstDiscardPosition.y); 
                Label name = (Label)Relic.GetChild(0);
                name.Text = card.name;
                index++;
            }
        }
        public static Button InstanciateButton()
        {
            PackedScene relic = (PackedScene)GD.Load("res://DiscardButton.tscn");
            Button button = (Button)relic.Instance();
            return button;
        }
        public static Sprite InstanciateVisualCard(gameEngine.Relics card)
        {
            PackedScene relic = (PackedScene)GD.Load("res://Relic.tscn");
            Relic = (Sprite)relic.Instance();

            Label name = (Label)Relic.GetChild(0);

            Sprite img = (Sprite)Relic.GetChild(1);
            ImageTexture image = new ImageTexture();
            image.Load(card.imgAddress);

            Label description = (Label)Relic.GetChild(2);

            name.Text = card.name;
            img.Texture = image;
            img.Scale = new Vector2((float)0.40, (float)0.40);
            description.Text = card.description;

            return Relic;
        }
        public static Sprite InstanciateVisualCharact(gameEngine.CharacterProperties character)
        {
            PackedScene relic = (PackedScene)GD.Load("res://Relic.tscn");
            Relic = (Sprite)relic.Instance();
            Label name = (Label)Relic.GetChild(0);

            Sprite img = (Sprite)Relic.GetChild(1);
            ImageTexture image = new ImageTexture();
            image.Load(character.imgAddress);

            Label description = (Label)Relic.GetChild(2);

            name.Text = character.name;
            img.Texture = image;
            img.Scale = new Vector2((float)0.40, (float)0.40);
            description.Text = character.description;
            return Relic;
        }
        public static Sprite InstanciateVisualBackCard(gameEngine.Relics card)
        {
            PackedScene relic = (PackedScene)GD.Load("res://Back-relic.tscn");
            Relic = (Sprite)relic.Instance();
            return Relic;
        }
        public void UpdateBattleField(gameEngine.Player player)
        {
            bool[] boolPlayerField;
            int PlayerEmptySlots;

            if (player == Game.player1)
            {
                boolPlayerField = boolPlayer1Field;
                PlayerEmptySlots = Player1emptySlots;
            }
            else
            {
                boolPlayerField = boolPlayer2Field;
                PlayerEmptySlots = Player2emptySlots;
            }

            for (int index = 0; index < player.battleField.userBattleField.Length; index++)
            {
                if (player.battleField.userBattleField[index] != null)
                {
                    
                    if (player.battleField.userBattleField[index].activeDuration == 1)
                    {
                        // Removing card from battelfield
                        PlayerEmptySlots++;
                        boolPlayerField[index] = false;
                        player.battleField.userVisualBattleField[index].QueueFree();
                        player.battleField.userVisualBattleField[index] = null;
                        Game.GraveYard.Add(player.battleField.userBattleField[index]);
                        player.battleField.userBattleField[index] = null; 
                    }
                    else
                    {
                        if (player.battleField.userBattleField[index].passiveDuration != 0)
                        {
                            player.battleField.userBattleField[index].passiveDuration--;
                        }
                        else
                        {
                            int Defaultpassive = mainMenu.Inventary.CardsInventary[player.battleField.userBattleField[index].id].passiveDuration;
                            player.battleField.userBattleField[index].passiveDuration = Defaultpassive;
                            player.battleField.userBattleField[index].activeDuration--;
                        }
                    }

                }
            }


            if (player == Game.player1)
            {
                Player1emptySlots = PlayerEmptySlots;
            }
            else
            {
                Player2emptySlots = PlayerEmptySlots;
            }
        }
        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
            {
                switch ((ButtonList)mouseEvent.ButtonIndex)
                {
                    case ButtonList.Left:
                        
                        if (discarding)
                        {
                            for (int i = 0; i < discardButtons.Count; i++)
                            {
                                if (discardButtons[i].GetRect().HasPoint(ToLocal(mouseEvent.Position)))
                                {
                                    Game.GraveYard.Add(discardPlayer.hand[i]);
                                    discardPlayer.hand.Remove(discardPlayer.hand[i]);
                                }

                                // Discarding finished
                                // Removing all instances of visual card
                                if (discardPlayer.hand.Count <= maxinHand)
                                {
                                    foreach (Node item in GetTree().GetNodesInGroup("discardGroup"))
                                    {
                                        item.QueueFree();
                                    }

                                    // Cleaning discardButtons List after discard all we need
                                    discardButtons.Clear();

                                    // We don't need to discard for now
                                    discarding = false;
                                    
                                }
                            }
                        }


                        bool news = false;

                        if(Game.turn % 2 == 0)
                        {
                            // Player 1 is clicking
                            for(int i = 0; i < Player1VisualHand.Count; i++)
                            {
                                if (Player1VisualHand[i].GetRect().HasPoint(Player1VisualHand[i].ToLocal(mouseEvent.Position)) && Player1emptySlots > 0)
                                {
                                    // Add to player's battlefield logicaly and visualy
                                    Game.player1.hand[i].Effect(); // Activating effect of card
                                    Player1emptySlots--;
                                    news = true;
                                }
                                
                            }

                            // Fulling (visualy) battlefield
                            for (int slot = 0; slot < Game.player1.battleField.userBattleField.Length; slot++)
                            {
                                if (!boolPlayer1Field[slot] && news && Game.player1.battleField.userVisualBattleField[slot] != null)
                                {
                                    AddChild(Game.player1.battleField.userVisualBattleField[slot]);
                                    Game.player1.battleField.userVisualBattleField[slot].Position = Player1FieldPositions[slot];
                                    boolPlayer1Field[slot] = true;
                                }
                            }
                            news = false;

                        }
                        else // Player2 is clicking
                        {
                            for(int i = 0; i < Player2VisualHand.Count; i++)
                            {
                                if (Player2VisualHand[i].GetRect().HasPoint(Player2VisualHand[i].ToLocal(mouseEvent.Position)) && Player2emptySlots > 0)
                                {
                                    // Add to player's battlefield
                                    Game.player2.hand[i].Effect(); // Activating effect of card
                                    Player2emptySlots--;
                                    news = true;
                                }
                            }

                            // Fulling (visualy) battlefield
                            for (int slot = 0; slot < Game.player2.battleField.userBattleField.Length; slot++)
                            {
                                if (!boolPlayer2Field[slot] && news && Game.player2.battleField.userVisualBattleField[slot] != null)
                                {
                                    AddChild(Game.player2.battleField.userVisualBattleField[slot]);
                                    Game.player2.battleField.userVisualBattleField[slot].Position = Player2FieldPositions[slot];
                                    boolPlayer2Field[slot] = true;
                                }
                            }

                        }
                        
                        RefreshBoard();
                        break;
                }
            }
            if (@event is InputEventMouse mouseMove)
            {
                for (int i = 0; i < Player1VisualHand.Count; i++)
                {
                    if(Player1VisualHand[i].GetRect().HasPoint(Player1VisualHand[i].ToLocal(mouseMove.Position)))
                    {
                        Sprite Relic = board.InstanciateVisualCard(Game.player1.hand[i]);
                        AddChild(Relic);
                        Relic.Scale = new Vector2((float)0.44, (float)0.45);
                        Relic.Position = new Vector2(170, 500);
                    }
                }
                if (Game.player1.battleField.userVisualBattleField.Length != 0)
                {
                    for (int i = 0; i < Game.player1.battleField.userVisualBattleField.Length; i++)
                    {
                        if (Game.player1.battleField.userVisualBattleField[i] != null)
                        {
                            if(Game.player1.battleField.userVisualBattleField[i].GetRect().HasPoint(Game.player1.battleField.userVisualBattleField[i].ToLocal(mouseMove.Position)))
                            {
                                Sprite Relic = board.InstanciateVisualCard(Game.player1.battleField.userBattleField[i]);
                                AddChild(Relic);
                                Relic.Scale = new Vector2((float)0.44, (float)0.45);
                                Relic.Position = new Vector2(170, 500);
                            }
                        }  
                    }
                }
                
                if (Game.player2.battleField.userVisualBattleField.Length != 0)
                {
                    for (int i = 0; i < Game.player2.battleField.userVisualBattleField.Length; i++)
                    {
                        if (Game.player2.battleField.userVisualBattleField[i] != null)
                        {
                            if(Game.player2.battleField.userVisualBattleField[i].GetRect().HasPoint(Game.player2.battleField.userVisualBattleField[i].ToLocal(mouseMove.Position)))
                            {
                                Sprite Relic = board.InstanciateVisualCard(Game.player2.battleField.userBattleField[i]);
                                AddChild(Relic);
                                Relic.Scale = new Vector2((float)0.44, (float)0.45);
                                Relic.Position = new Vector2(170, 500);
                            }
                        }  
                    }
                }
            }
        }
    }       
}