using Godot;
using System.Collections.Generic;
using gameEngine;
namespace gameVisual
{
    public class board : Godot.Node2D
    {   

        public static Node hijo = new Node();
        #region Visual Objects
        const int maxinHand = 6;


        static List<Sprite> Player2VisualHand = new List<Sprite>();
        static List<Sprite> Player1VisualHand = new List<Sprite>();

        public static Vector2[] Player1FieldPositions;
        static bool[] boolPlayer1Field = new bool[4];
        
        public static Vector2[] Player2FieldPositions;
        static bool[] boolPlayer2Field = new bool[4];


        static Sprite Relic = new Sprite();


        public static int Player1emptySlots = 4;
        static int Player2emptySlots = 4;


        public static bool selecting = false;
        public static gameEngine.Player discardPlayer = new Player("default");
        public static List<Button> discardButtons = new List<Button>();
        public static PackedScene SelectCards = (PackedScene)GD.Load("res://SelectCards.tscn");


        public static Vector2 Player2VisualHandPosition;
        public static Vector2 Player1VisualHandPosition;

        public static Node PauseMenu;

        static Sprite GraveYardCard = new Sprite();
        Sprite Preview = new Sprite();

        public static List<Sprite> selectCards;
        public static int selectQuant = 1;
        public static List<Relics> SelectedCards;
        public static List<Relics> SourceToSelect;

        Button AcceptButton = InstanciateButton();
        #endregion


        public class Board : Godot.Node2D
        {
            public VisualGraveYard visualGraveYard;
            public VisualBattleField visualBattleField1;
            public VisualBattleField visualBattleField2;
            public VisualHand visualHand1;
            public VisualHand visualHand2;

            public Board(Game game)
            {
                this.visualGraveYard = new VisualGraveYard(game.GraveYard);
                this.visualBattleField1 = new VisualBattleField(game.player1.BattleField, new Sprite[4]);
                this.visualBattleField2 = new VisualBattleField(game.player2.BattleField, new Sprite[4]);
                this.visualHand1 = new VisualHand(game.player1.hand, new List<Sprite>());
                this.visualHand2 = new VisualHand(game.player2.hand, new List<Sprite>());
            }

            public class VisualGraveYard 
            {
                List<Relics> graveYard;
                List<Sprite> visualGraveYard;

                public VisualGraveYard(List<Relics> graveYard)
                {
                    this.graveYard = graveYard;
                }

                public void show()
                {
                    Relics relic = graveYard[graveYard.Count-1];
                    if (((Label)GraveYardCard.GetChild(0)).Text != relic.name)
                    {
                        Sprite Relic = board.InstanciateVisualCard(relic);
                        GraveYardCard.Visible = true;
                        ((Label)GraveYardCard.GetChild(0)).Text = ((Label)Relic.GetChild(0)).Text;
                        ((Sprite)GraveYardCard.GetChild(1)).Texture = ((Sprite)Relic.GetChild(1)).Texture;
                        ((Label)GraveYardCard.GetChild(2)).Text = ((Label)Relic.GetChild(2)).Text;
                        ((Sprite)GraveYardCard.GetChild(3)).Texture = ((Sprite)Relic.GetChild(3)).Texture;
                        ((Label)GraveYardCard.GetChild(4)).Text = ((Label)Relic.GetChild(4)).Text;
                        ((Sprite)GraveYardCard.GetChild(1)).Scale = new Vector2((float)0.60, (float)0.50);
                        ((Sprite)GraveYardCard.GetChild(3)).Scale = new Vector2((float)0.15, (float)0.15);
                    }
                }
                
            }

            public class VisualHand 
            {
                public List<Relics> Hand;
                public List<Sprite> visualHand;

                public VisualHand(List<Relics> Hand, List<Sprite> visualHand)
                {
                    this.Hand = Hand;
                    this.visualHand = visualHand;
                }
            }

            public class VisualBattleField 
            {
                public Relics[] BattleField;
                public Sprite[] visualBattleField;

                public VisualBattleField(Relics[] BattleField, Sprite[] visualBattleField)
                {
                    this.BattleField = BattleField;
                    this.visualBattleField = visualBattleField;
                }
            }

        }
       
        public static Game Game;
        public static Board VisualBoard;
        public override void _Ready()
        {
            AddChild(hijo);
            Game = new Game(SelectPlayer.player1, SelectPlayer.player2);
            VisualBoard = new Board(Game);

            Preview = GetNode<Sprite>("Preview/Relic");
            GraveYardCard = GetNode<Sprite>("GraveYard/Relic");
            GraveYardCard.Visible = false;
            Preview.Visible = false;
            
            Player2FieldPositions = new Vector2[4]
            {
                new Vector2(570, 760),
                new Vector2(860, 760),
                new Vector2(1145, 760),
                new Vector2(1430, 760)
            };
             
            Player1FieldPositions = new Vector2[4]
            {
                new Vector2(570, 350),
                new Vector2(860, 350),
                new Vector2(1145, 350),
                new Vector2(1430, 350)
            };

            RefreshVisualHands();
        }

        bool player1Attack = false;
        bool player2Attack = false;
        public static Label Turnlabel;

        public override void _Process(float delta)
        {
            Turnlabel = GetNode<Label>("TurnLabel");

            // Checking end of game
            if (!(Game.player1.life > 0 && Game.player2.life > 0))
            {
                GetTree().ChangeScene("res://GameOver.tscn");
            }

            
            #region Updating visually playerInfo
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

            if (Game.turn % 2 == 0) // Player1's turn
            {
                // Next Player takes a card
                ((RandomVirtPlayer)(Game.player1)).Play();
                Turnlabel.Text = "Turno: " + Game.turn;
            }

            Button Attack = GetNode<Button>("Attack");
            Button endButton = GetNode<Button>("endButton");

            // ATTACK BUTTON
            Attack.Disabled = false;
            AttackButtonFunction(Attack);

            //Change Turn (END BUTTON)
            if (endButton.Pressed)
            {
                EndButtonFunction();
                endButton.Disabled = true; // Disabling button, increment turn just one time
            }
            endButton.Disabled = false;
            
            // SHOW SELECT CARDS SCENE
            if (Game.GraveYard.Count != 0)
            {
                VisualBoard.visualGraveYard.show();
            }
            if(selectQuant == 0)
            {
                if (AcceptButton.GetParent() == null)
                {
                    AddChild(AcceptButton);
                }
                AcceptButton.Visible = true;
            }
            else
            {
                if (AcceptButton.GetParent() != null)
                {
                    RemoveChild(AcceptButton);
                }
                AcceptButton.Visible = false;
            }

        }
        public void EndButtonFunction()
        {
            if (Game.turn % 2 != 0) // Player2's turn
            {
                // Next Player takes a card
                Game.player2.TakeFromDeck(1);
                UpdateBattleField(Game.player2);
                RefreshVisualHands();
            }
            else
            {
                // Next Player takes a card
                Game.player1.TakeFromDeck(1);
                PauseGame(3);
                UpdateBattleField(Game.player1);
                RefreshVisualHands();
            }
            Game.turn++;
            Turnlabel.Text = "Turno: " + Game.turn;
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
        public void RefreshVisualHands()
        {
            Player2VisualHandPosition = new Vector2(500 - (Game.player1.hand.Count * 5), 1000);
            Player1VisualHandPosition = new Vector2(500 - (Game.player2.hand.Count * 5), 50);
                
            // Updating cards in board
            UpdateVisualHand(Game.player1.hand);
            UpdateVisualHand(Game.player2.hand);

            if (Game.player1.hand.Count > maxinHand)
            {
                selectVisually(Game.player1.hand, Game.player1.hand.Count-maxinHand);
            }
            if (Game.player2.hand.Count > maxinHand)
            {
                selectVisually(Game.player2.hand, Game.player2.hand.Count-maxinHand);
            }
            
        }
        public void UpdateVisualHand(List<Relics> playerHand)
        {
            List<Sprite> PlayerVisualHand = new List<Sprite>();
            Vector2 PlayerVisualHandPosition;
            string group = "";

            if (board.Game.player1.hand == playerHand)
            {
                group = "RelicsNodes1";
                PlayerVisualHand = Player1VisualHand;
                PlayerVisualHandPosition = Player1VisualHandPosition;
            }
            else
            {
                group = "RelicsNodes2";
                PlayerVisualHand = Player2VisualHand;
                PlayerVisualHandPosition = Player2VisualHandPosition;
            }

            if (PlayerVisualHand.Count - playerHand.Count >= 0)
            {
                // Delete old relics
                for(int i = 0; i < PlayerVisualHand.Count; i++)
                {
                    try
                    {
                        if (((Label)PlayerVisualHand[i].GetChild(0)).Text != playerHand[i].name)
                        {
                            ((Node)GetTree().GetNodesInGroup(group)[i]).QueueFree();
                            PlayerVisualHand.RemoveAt(i);
                        }
                    }
                    catch (System.Exception)
                    {
                        if (PlayerVisualHand.Count != 0)
                        {
                            PlayerVisualHand.RemoveAt(i);
                            ((Node)GetTree().GetNodesInGroup(group)[i]).QueueFree();
                        }
                    }
                    
                    if (i >= maxinHand)
                    {
                        selecting = true;
                        selectVisually(playerHand, playerHand.Count - maxinHand);
                    }
                }
            }
            else if (PlayerVisualHand.Count - playerHand.Count < 0)
            {
                //Create new relics in hand
                for(int i = PlayerVisualHand.Count; i < playerHand.Count; i++)
                {
                    Sprite relic = InstanciateVisualCard(playerHand[i]);
                    PlayerVisualHand.Add(Relic);
                    hijo.AddChild(relic);
                    relic.AddToGroup(group);
                    if (i >= maxinHand)
                    {
                        selecting = true;
                        selectVisually(playerHand, playerHand.Count - maxinHand);
                    }
                }

                PlayerVisualHandPosition.x = (1920/2) - PlayerVisualHand.Count*(float)73.5;
                for(int i = 0; i < PlayerVisualHand.Count; i++)
                {
                    PlayerVisualHand[i].Position = new Vector2(PlayerVisualHandPosition.x + 200*i, PlayerVisualHandPosition.y);
                }
            }            
        }
        public static Button InstanciateButton()
        {
            PackedScene relic = (PackedScene)GD.Load("res://DiscardButton.tscn");
            Button button = (Button)relic.Instance();
            return button;
        }
        public static Sprite InstanciateVisualCard(Relics card)
        {
            PackedScene relic = (PackedScene)GD.Load("res://Relic.tscn");
            Relic = (Sprite)relic.Instance();

            Label name = (Label)Relic.GetChild(0);
            Label description = (Label)Relic.GetChild(2);
            Label duration = (Label)Relic.GetChild(4);

            Sprite img = (Sprite)Relic.GetChild(1);
            ImageTexture image = new ImageTexture();
            image.Load(ProjectSettings.GlobalizePath(card.imgAddress));


            Sprite type = (Sprite)Relic.GetChild(3);
            ImageTexture Type = new ImageTexture();
            switch (card.type)
            {
                case "daño":
                    Type.Load(ProjectSettings.GlobalizePath("res://Sprites/Cards-images/photo_2022-12-05_08-38-52.jpg"));
                    break;
                default:
                    Type.Load(ProjectSettings.GlobalizePath("res://Sprites/Cards-images/photo_2022-12-05_08-38-52.jpg"));
                    break;
            }


            name.Text = card.name;

            img.Texture = image;
            img.Scale = new Vector2((float)0.60, (float)0.50);


            description.Text = card.description;

            duration.Text = card.activeDuration.ToString();

            type.Texture = Type;
            type.Scale = new Vector2((float)0.15, (float)0.15);

            return Relic;
        }
        public static Sprite InstanciateVisualCharact(gameEngine.CharacterProperties character)
        {
            PackedScene relic = (PackedScene)GD.Load("res://Relic.tscn");
            Relic = (Sprite)relic.Instance();
            Label name = (Label)Relic.GetChild(0);

            Sprite img = (Sprite)Relic.GetChild(1);
            ImageTexture image = new ImageTexture();
            image.Load(ProjectSettings.GlobalizePath(character.imgAddress));
            Label description = (Label)Relic.GetChild(2);

            name.Text = character.name;
            img.Texture = image;
            img.Scale = new Vector2((float)0.64, (float)0.52);
            description.Text = character.description;
            return Relic;
        }
        public static Sprite InstanciateVisualBackCard(gameEngine.Relics card)
        {
            PackedScene relic = (PackedScene)GD.Load("res://Back-relic.tscn");
            Relic = (Sprite)relic.Instance();
            return Relic;
        }
        public static void UpdateBattleField(gameEngine.Player player)
        {
            bool[] boolPlayerField;
            int PlayerEmptySlots;
            Sprite[] field;

            if (player == Game.player1)
            {
                boolPlayerField = boolPlayer1Field;
                PlayerEmptySlots = Player1emptySlots;
                field = VisualBoard.visualBattleField1.visualBattleField;
            }
            else
            {
                boolPlayerField = boolPlayer2Field;
                PlayerEmptySlots = Player2emptySlots;
                field = VisualBoard.visualBattleField2.visualBattleField;
            }

            for (int index = 0; index < player.BattleField.Length; index++)
            {
                if (player.BattleField[index] != null)
                {
                    
                    if (player.BattleField[index].activeDuration == 1)
                    {
                        // Removing card from battelfield
                        PlayerEmptySlots++;
                        boolPlayerField[index] = false;
                        field[index].QueueFree();
                        field[index] = null;
                        Game.GraveYard.Add(player.BattleField[index]);
                        player.BattleField[index] = null; 
                    }
                    else
                    {
                        if (player.BattleField[index].passiveDuration != 0)
                        {
                            player.BattleField[index].passiveDuration--;
                        }
                        else
                        {
                            int Defaultpassive = mainMenu.Inventary.CardsInventary[player.BattleField[index].id].passiveDuration;
                            player.BattleField[index].passiveDuration = Defaultpassive;
                            player.BattleField[index].activeDuration--;
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

        public void selectVisually(List<Relics> Source, int quant)
        {
            SelectedCards = new List<Relics>();
            selectCards = new List<Sprite>();
            SourceToSelect = Source;
            selectQuant = quant;

            Tree SelectMenu = (Tree)SelectCards.Instance();
            AddChild(SelectMenu);
            
            AcceptButton.SetPosition(new Vector2(875, 720));
            
            Vector2 FirstPosition = new Vector2(180, 450);

            // Showing cards to select
            int index = 1;
            foreach (var card in Source)
            {
                Sprite Card = InstanciateVisualCard(card);
                selectCards.Add(Card);
                AddChild(Card);
                Card.Position = new Vector2(210 * index, FirstPosition.y); 
                index++;
            }

            selecting = true;
            if (selectQuant == 0)
            {
                selecting = false;
            }

            if(AcceptButton.Pressed)
            {
                GD.Print(selectCards);
                foreach (Node node in selectCards)
                {
                    node.QueueFree();
                }
                SelectMenu.QueueFree();
            }
        }

        public void PreviewPropierties(Relics relic)
        {
            if (((Label)Preview.GetChild(0)).Text != relic.name)
            {
                Sprite Relic = board.InstanciateVisualCard(relic);
                Preview.Visible = true;
                ((Label)Preview.GetChild(0)).Text = ((Label)Relic.GetChild(0)).Text;
                ((Sprite)Preview.GetChild(1)).Texture = ((Sprite)Relic.GetChild(1)).Texture;
                ((Label)Preview.GetChild(2)).Text = ((Label)Relic.GetChild(2)).Text;
                ((Sprite)Preview.GetChild(3)).Texture = ((Sprite)Relic.GetChild(3)).Texture;
                ((Label)Preview.GetChild(4)).Text = ((Label)Relic.GetChild(4)).Text;


                ((Sprite)Preview.GetChild(1)).Scale = new Vector2((float)0.60, (float)0.50);
                ((Sprite)Preview.GetChild(3)).Scale = new Vector2((float)0.15, (float)0.15);
            }
        }
        public static void PauseGame(int time)
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            while (watch.ElapsedMilliseconds < time*1000){}
            watch.Stop();
        }
        public override void _Input(InputEvent @event)  
        {
            // Pause Menu
            if (@event is InputEventKey eventKey)
                if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
                {
                    PackedScene EscMenu = (PackedScene)GD.Load("res://PauseMenu.tscn");
                    PauseMenu = (Node)EscMenu.Instance();
                    AddChild(PauseMenu);
                }

            // Click
            if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
            {
                switch ((ButtonList)mouseEvent.ButtonIndex)
                {
                    case ButtonList.Left:

                        if (selecting)
                        {
                            for(int i = 0; i < selectCards.Count; i++)
                            {
                                if (selectCards[i].GetRect().HasPoint(selectCards[i].ToLocal(mouseEvent.Position)))
                                {
                                    if (selectCards[i].Scale == new Vector2((float)0.30,(float)0.30))
                                    {
                                        selectQuant++;
                                        SelectedCards.Remove(SourceToSelect[i]);
                                        selectCards[i].Scale = new Vector2((float)0.25,(float)0.25);
                                    }
                                    else if (selectQuant != 0)
                                    {
                                        selectQuant--;
                                        SelectedCards.Add(SourceToSelect[i]);
                                        selectCards[i].Scale = new Vector2((float)0.30,(float)0.30);
                                    }
                                }
                            }
                        }

                        if(Game.turn % 2 == 0)
                        {
                            // // Player 1 is clicking
                            // for(int i = 0; i < Player1VisualHand.Count; i++)
                            // {
                            //     if (Player1VisualHand[i].GetRect().HasPoint(Player1VisualHand[i].ToLocal(mouseEvent.Position)) && Player1emptySlots > 0)
                            //     {
                            //         // Add to player's battlefield logically and visually
                            //         Game.player1.hand[i].Effect(); // Activating effect of card
                            //         Player1emptySlots--;
                            //         GetTree().Paused = true;
                            //         RefreshBoard();
                            //         GetTree().Paused = false;
                            //     }
                                
                            // }
                        }
                        else // Player 2 is clicking
                        {
                            for(int i = 0; i < Player2VisualHand.Count; i++)
                            {
                                if (Player2VisualHand[i].GetRect().HasPoint(Player2VisualHand[i].ToLocal(mouseEvent.Position)) && Player2emptySlots > 0)
                                {
                                    // Add to player's battlefield
                                    Game.player2.hand[i].Effect(); // Activating effect of card
                                    Player2emptySlots--;
                                    RefreshVisualHands();
                                }
                            }
                        }
                        break;
                }
            }

            // Hover
            if (@event is InputEventMouse mouseMove)
            {
                // Preview player1 hand
                for (int i = 0; i < Player1VisualHand.Count; i++)
                {
                    if(Player1VisualHand[i].GetRect().HasPoint(Player1VisualHand[i].ToLocal(mouseMove.Position)))
                    {
                        PreviewPropierties(Game.player1.hand[i]);
                    }
                }

                // Preview player1 Field
                for (int i = 0; i < VisualBoard.visualBattleField1.visualBattleField.Length; i++)
                {
                    if (VisualBoard.visualBattleField1.visualBattleField[i] != null)
                    {
                        if(VisualBoard.visualBattleField1.visualBattleField[i].GetRect().HasPoint(VisualBoard.visualBattleField1.visualBattleField[i].ToLocal(mouseMove.Position)))
                        {
                            PreviewPropierties(VisualBoard.visualBattleField1.BattleField[i]);
                        }
                    }
                }
                
                // for (int i = 0; i < Player2VisualHand.Count; i++)
                // {
                //     if(Player2VisualHand[i].GetRect().HasPoint(Player2VisualHand[i].ToLocal(mouseMove.Position)))
                //     {
                //         Sprite Relic = board.InstanciateVisualCard(Game.player2.hand[i]);
                //         AddChild(Relic);
                //         Relic.Scale = new Vector2((float)0.44, (float)0.45);
                //         Relic.Position = new Vector2(170, 500);
                //     }
                // }

                // Preview player2 Field
                for (int i = 0; i < VisualBoard.visualBattleField2.visualBattleField.Length; i++)
                {
                    if (VisualBoard.visualBattleField2.visualBattleField[i] != null)
                    {
                        if(VisualBoard.visualBattleField2.visualBattleField[i].GetRect().HasPoint(VisualBoard.visualBattleField2.visualBattleField[i].ToLocal(mouseMove.Position)))
                        {
                            PreviewPropierties(VisualBoard.visualBattleField2.BattleField[i]);
                        }
                    }
                }
            }
        
        }
    }       
}