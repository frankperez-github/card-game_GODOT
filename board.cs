using Godot;
using System.Collections.Generic;
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
        public static gameEngine.Player discardPlayer = default;
        public static List<Button> discardButtons = new List<Button>();
        static PackedScene DiscardScene = (PackedScene)GD.Load("res://DiscardLabel.tscn");


        public static Vector2 Player1VisualHandPosition;
        public static Vector2 Player2VisualHandPosition;
        #endregion


        public override void _Ready()
        {
            
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
                gameEngine.Settings.PlayersInventary.Add(new gameEngine.Player(gameEngine.Settings.CharactersInventary[0], "el otro"));
            }

            gameEngine.Settings.player1 = gameEngine.Settings.PlayersInventary[0];
            gameEngine.Settings.player2 = gameEngine.Settings.PlayersInventary[1];

            
            gameEngine.Settings.player1.TakeFromDeck(gameEngine.Settings.player1, gameEngine.Settings.player2, 5, new List<gameEngine.Relics>());
            gameEngine.Settings.player2.TakeFromDeck(gameEngine.Settings.player2, gameEngine.Settings.player1, 5, new List<gameEngine.Relics>());

            RefreshBoard();
        }

        bool player1Attack = false;
        bool player2Attack = false;
        public override void _Process(float delta)
        {

            Label Turnlabel = GetNode<Label>("TurnLabel");

            // Checking end of game
            if (!(gameEngine.Settings.player1.life > 0 && gameEngine.Settings.player2.life > 0))
            {
                GetTree().ChangeScene("res://GameOver.tscn");
            }

            Button Attack = GetNode<Button>("Attack");
            Button endButton = GetNode<Button>("endButton");

            
            #region Updating visualy playerInfo
            Label PlayerNick = GetNode<Label>("PlayerInfo/Player's Nick");
            PlayerNick.Text = gameEngine.Settings.PlayersInventary[0].nick;
            Label PlayerLife = GetNode<Label>("PlayerInfo/Player's Life");
            PlayerLife.Text = "Life : "+ gameEngine.Settings.PlayersInventary[0].life.ToString();
            Label PlayerShield = GetNode<Label>("PlayerInfo/Player's Shield");
            PlayerShield.Text = "Shield : "+ gameEngine.Settings.PlayersInventary[0].defense.ToString();
            Label PlayerAttack = GetNode<Label>("PlayerInfo/Player's Attack");
            PlayerAttack.Text = "Attack : "+ gameEngine.Settings.PlayersInventary[0].attack.ToString();
            Label Player1State = GetNode<Label>("PlayerInfo/Player's State");
            Player1State.Text = "State : "+ gameEngine.Settings.PlayersInventary[0].state.ToString();

            Label Player2Nick = GetNode<Label>("Player2Info/Player2's Nick");
            Player2Nick.Text = gameEngine.Settings.PlayersInventary[1].nick;
            Label Player2Life = GetNode<Label>("Player2Info/Player2's Life");
            Player2Life.Text = "Life : "+ gameEngine.Settings.PlayersInventary[1].life.ToString();
            Label Player2Shield = GetNode<Label>("Player2Info/Player2's Shield");
            Player2Shield.Text = "Shield : "+ gameEngine.Settings.PlayersInventary[1].defense.ToString();
            Label Player2Attack = GetNode<Label>("Player2Info/Player2's Attack");
            Player2Attack.Text = "Attack : "+ gameEngine.Settings.PlayersInventary[1].attack.ToString();
            Label Player2State = GetNode<Label>("Player2Info/Player2's State");
            Player2State.Text = "State : "+ gameEngine.Settings.PlayersInventary[1].state.ToString();
            #endregion

            // ATTACK BUTTON
            Attack.Disabled = false;
            if (Attack.Pressed)
            {
                if (gameEngine.Settings.turn % 2 == 0 && !player1Attack) // Player1's Turn
                {
                    gameEngine.Settings.player2.life -= gameEngine.Settings.player1.attack;
                    player1Attack = true;
                    player2Attack = false; // Enabling attack button to next turn 
                    Attack.Disabled = true;
                }
                if (gameEngine.Settings.turn % 2 != 0 && !player2Attack)// Player2's Turn
                {
                    gameEngine.Settings.player1.life -= gameEngine.Settings.player2.attack;
                    player2Attack = true;
                    player1Attack = false; // Enabling attack button to next turn 
                    Attack.Disabled = true;
                }
            }


            //Change Turn (END BUTTON)
            if (endButton.Pressed)
            {
                if (gameEngine.Settings.turn % 2 == 0) // Player1's turn
                {
                    // Next Player takes a card
                    gameEngine.Settings.player1.TakeFromDeck(gameEngine.Settings.player2, gameEngine.Settings.player1, 1, new List<gameEngine.Relics>());
                    UpdateBattleField(gameEngine.Settings.player1);
                }
                else // Player2's turn
                {
                    // Next Player takes a card
                    gameEngine.Settings.player2.TakeFromDeck(gameEngine.Settings.player1, gameEngine.Settings.player2, 1, new List<gameEngine.Relics>());
                    UpdateBattleField(gameEngine.Settings.player2);
                }
                RefreshBoard();
                gameEngine.Settings.turn++;
                Turnlabel.Text = "Turno: " + gameEngine.Settings.turn;
                endButton.Disabled = true; // Disabling button, increment turn just one time
            }
            endButton.Disabled = false;
        }

        public void RefreshBoard()
        {
            PackedScene relic = (PackedScene)GD.Load("res://Relic.tscn");
            
            Player1VisualHandPosition = new Vector2(350 - (gameEngine.Settings.player1.hand.Count * 10), 1000);
            Player2VisualHandPosition = new Vector2(350 - (gameEngine.Settings.player2.hand.Count * 10), 50);
            

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
            catch (System.NullReferenceException)
            {}
                
             int index = 1;
            // Updating cards in board
            foreach (var card in gameEngine.Settings.player1.hand)
            {
                Relic = InstanciateVisualCard(card);
                Relic.AddToGroup("VisibleCards");
                Player1VisualHand.Add (Relic);
                Relic.Position = new Vector2(Player1VisualHandPosition.x + 200*index,  Player1VisualHandPosition.y);
                AddChild(Relic);
                Label name = (Label) Relic.GetChild(0);
                name.Text = card.name;
                index++; 

            }
            if (index > maxinHand)
            {
                discarding = true;
                CheckAndDiscard(gameEngine.Settings.player1);
            }

            int enemyIndex = 1;
            // Updating enemy's cards in board
            foreach (var card in gameEngine.Settings.player2.hand)
            {
                Relic = InstanciateVisualBackCard(card);
                Relic.AddToGroup("VisibleCards");
                Player2VisualHand.Add(Relic);
                Relic.Position = new Vector2(Player2VisualHandPosition.x + 200*enemyIndex, Player2VisualHandPosition.y);
                AddChild(Relic);
                enemyIndex++; 
            }
            if (enemyIndex > maxinHand)
            {
                discarding = true;
                CheckAndDiscard(gameEngine.Settings.player2);
            }
        }
        public void CheckAndDiscard(gameEngine.Player player)
        {
            
            Vector2 FirstDiscardPosition = new Vector2(180, 450);

            if (player.hand.Count > 6)
            {
                discardPlayer = player;
                Tree DiscardTscn = (Tree)DiscardScene.Instance();
                Label label = DiscardTscn.GetNode<Label>("DiscardLabel");
                label.Text = player.nick+" must discard:  "+(player.hand.Count - 6).ToString();
                AddChild(DiscardTscn);
                DiscardTscn.AddToGroup("discardGroup");

                int index = 1;
                foreach (var card in player.hand)
                {
                    Sprite relic = InstanciateVisualCard(card);
                    Button button = InstanciateButton();
                    relic.Scale = new Vector2((float)0.08,(float)0.09);
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
        }
        public static Button InstanciateButton()
        {
            PackedScene relic = (PackedScene)GD.Load("res://DiscardButton.tscn");
            Button button = (Button)relic.Instance();
            return button;
        }
        public static Sprite InstanciateVisualCard(gameEngine.Cards card)
        {
            PackedScene relic = (PackedScene)GD.Load("res://Relic.tscn");
            Relic = (Sprite)relic.Instance();
            Label name = (Label)Relic.GetChild(0);
            name.Text = card.name;
            return Relic;
        }
        public static Sprite InstanciateVisualBackCard(gameEngine.Cards card)
        {
            PackedScene relic = (PackedScene)GD.Load("res://Back-relic.tscn");
            Relic = (Sprite)relic.Instance();
            return Relic;
        }
        public static void UpdateBattleField(gameEngine.Player player)
        {
            bool[] boolPlayerField;
            int PlayerEmptySlots;
            if (player == gameEngine.Settings.player1)
            {
                boolPlayerField = boolPlayer1Field;
                PlayerEmptySlots = Player1emptySlots;
            }
            else
            {
                boolPlayerField = boolPlayer2Field;
                PlayerEmptySlots = Player2emptySlots;
            }

            for (int index = 0; index < player.userBattleField.Length; index++)
            {
                if (player.userBattleField[index] != null)
                {
                    
                    if (player.userBattleField[index].activeDuration == 1)
                    {
                        foreach (var effect in player.userBattleField[index].EffectsOrder)
                        {
                            if(effect.Key == 5)
                            {
                                effect.Value.affects = effect.Value.affects*(-1); 
                                player.userBattleField[index].Effect();
                                effect.Value.affects = effect.Value.affects*(-1); 
                            }
                            if(effect.Key == 8)
                            {
                                player.userBattleField[index].Affected.state = gameEngine.State.Safe;
                            }
                        }
                        // Removing card from battelfield
                        PlayerEmptySlots++;
                        boolPlayerField[index] = false;
                        player.userVisualBattleField[index].QueueFree();
                        player.userVisualBattleField[index] = null;
                        gameEngine.Settings.GraveYard.Add(player.userBattleField[index]);
                        player.userBattleField[index] = null; 
                    }
                    else
                    {
                        if (player.userBattleField[index].passiveDuration != 0)
                        {
                            player.userBattleField[index].passiveDuration--;
                        }
                        else
                        {
                            int Defaultpassive = gameEngine.Settings.CardsInventary[player.userBattleField[index].id].passiveDuration;
                            player.userBattleField[index].passiveDuration = Defaultpassive;
                            player.userBattleField[index].activeDuration--;
                        }
                    }

                }
            }


            if (player == gameEngine.Settings.player1)
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
                                    gameEngine.Settings.GraveYard.Add(discardPlayer.hand[i]);
                                    discardPlayer.hand.Remove(discardPlayer.hand[i]);
                                }

                                // Discarding finished
                                // Removing all instances
                                if (discardPlayer.hand.Count <= 6)
                                {
                                    foreach (Node item in GetTree().GetNodesInGroup("discardGroup"))
                                    {
                                        item.QueueFree();
                                    }

                                    // Cleaning discardButtons List after discard all we need
                                    discardButtons.Clear();

                                    // We don't need to discard for now
                                    discarding = false;
                                    
                                    //Update Conditions
                                }
                            }
                        }


                        bool news = false;

                        if(gameEngine.Settings.turn % 2 == 0)
                        {
                            // Player 1 is clicking
                            for(int i = 0; i < Player1VisualHand.Count; i++)
                            {
                                if (Player1VisualHand[i].GetRect().HasPoint(Player1VisualHand[i].ToLocal(mouseEvent.Position)) && Player1emptySlots > 0)
                                {
                                    // Add to player's battlefield logicaly and visualy
                                    gameEngine.Settings.player1.hand[i].Effect(); // Activating effect of card
                                    Player1emptySlots--;
                                    news = true;
                                }
                                
                            }

                            // Fulling (visualy) battlefield
                            for (int slot = 0; slot < gameEngine.Settings.player1.userBattleField.Length; slot++)
                            {
                                if (!boolPlayer1Field[slot] && news && gameEngine.Settings.player1.userVisualBattleField[slot] != null)
                                {
                                    AddChild(gameEngine.Settings.player1.userVisualBattleField[slot]);
                                    gameEngine.Settings.player1.userVisualBattleField[slot].Position = Player1FieldPositions[slot];
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
                                    gameEngine.Settings.player2.hand[i].Effect(); // Activating effect of card
                                    Player2emptySlots--;
                                    news = true;
                                }
                            }

                            // Fulling (visualy) battlefield
                            for (int slot = 0; slot < gameEngine.Settings.player2.userBattleField.Length; slot++)
                            {
                                if (!boolPlayer2Field[slot] && news && gameEngine.Settings.player2.userVisualBattleField[slot] != null)
                                {
                                    AddChild(gameEngine.Settings.player2.userVisualBattleField[slot]);
                                    gameEngine.Settings.player2.userVisualBattleField[slot].Position = Player2FieldPositions[slot];
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
                        Sprite Relic = gameVisual.board.InstanciateVisualCard(gameEngine.Settings.player1.hand[i]);
                        AddChild(Relic);
                        Relic.Scale = new Vector2((float)0.14, (float)0.15);
                        Relic.Position = new Vector2(170, 500);
                    }
                }
                if (gameEngine.Settings.player1.userVisualBattleField.Length != 0)
                {
                    for (int i = 0; i < gameEngine.Settings.player1.userVisualBattleField.Length; i++)
                    {
                        if (gameEngine.Settings.player1.userVisualBattleField[i] != null)
                        {
                            if(gameEngine.Settings.player1.userVisualBattleField[i].GetRect().HasPoint(gameEngine.Settings.player1.userVisualBattleField[i].ToLocal(mouseMove.Position)))
                            {
                                Sprite Relic = gameVisual.board.InstanciateVisualCard(gameEngine.Settings.player1.userBattleField[i]);
                                AddChild(Relic);
                                Relic.Scale = new Vector2((float)0.14, (float)0.15);
                                Relic.Position = new Vector2(170, 500);
                            }
                        }  
                    }
                }
                
                if (gameEngine.Settings.player2.userVisualBattleField.Length != 0)
                {
                    for (int i = 0; i < gameEngine.Settings.player2.userVisualBattleField.Length; i++)
                    {
                        if (gameEngine.Settings.player2.userVisualBattleField[i] != null)
                        {
                            if(gameEngine.Settings.player2.userVisualBattleField[i].GetRect().HasPoint(gameEngine.Settings.player2.userVisualBattleField[i].ToLocal(mouseMove.Position)))
                            {
                                Sprite Relic = gameVisual.board.InstanciateVisualCard(gameEngine.Settings.player2.userBattleField[i]);
                                AddChild(Relic);
                                Relic.Scale = new Vector2((float)0.14, (float)0.15);
                                Relic.Position = new Vector2(170, 500);
                            }
                        }  
                    }
                }
            }
        }
    }       
}