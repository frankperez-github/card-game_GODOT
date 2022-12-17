using Godot;
using System.Collections.Generic;
using gameEngine;
namespace gameVisual
{
    public class board : Godot.Node2D
    {   

        public static Node child = new Node();

        #region Visual Objects
        const int maxinHand = 6;

        public static int Player1emptySlots = 4;
        public static int Player2emptySlots = 4;

        static Sprite Relic = new Sprite();

        public static bool selecting = false;
        public static gameEngine.Player discardPlayer = new Player("default");
        public static List<Button> discardButtons = new List<Button>();
        public static PackedScene SelectCards = (PackedScene)GD.Load("res://SelectCards.tscn");

        public static Node PauseMenu = new Node();

        Sprite Preview;

        public static List<Sprite> selectCards;
        public static int selectQuant = 1;
        public static List<Relics> SelectedCards;
        public static List<Relics> SourceToSelect;

        static Button AcceptButton = InstanciateButton();

        bool player1Attack = false;
        bool player2Attack = false;
        public static Label Turnlabel;
        public static Button Attack;

        public static Game Game;
        public static Board VisualBoard;

        #endregion

        

        public class Board : Godot.Node2D
        {
            public VisualGraveYard visualGraveYard;
            public VisualBattleField visualBattleField1;
            public VisualBattleField visualBattleField2;
            public VisualHand visualHand1;
            public VisualHand visualHand2;
            public SceneTree Tree;

            public Board(Game game, Sprite GraveYard, SceneTree Tree)
            {
                this.visualGraveYard = new VisualGraveYard(game.GraveYard, GraveYard);
                this.visualBattleField1 = new VisualBattleField(game.player1.BattleField, new Sprite[4]);
                this.visualBattleField2 = new VisualBattleField(game.player2.BattleField, new Sprite[4]);
                this.visualHand1 = new VisualHand(game.player1.hand, Tree, 50, "RelicsNode1");
                this.visualHand2 = new VisualHand(game.player2.hand, Tree, 1000, "RelicsNode2");
                this.Tree = Tree;
            }

            public class VisualGraveYard 
            {
                List<Relics> graveYard;
                List<Sprite> visualGraveYard;
                Sprite GraveYardCard;

                public VisualGraveYard(List<Relics> graveYard, Sprite GraveYard)
                {
                    this.graveYard = graveYard;
                    GraveYardCard = GraveYard;
                    GraveYardCard.Visible = false;

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
                static List<Sprite> PlayerVisualHand;
                public List<Relics> Hand;
                public List<Sprite> visualHand;
                SceneTree Tree;
                public Vector2 VisualHandPosition;
                public string group;

                public VisualHand(List<Relics> Hand, SceneTree Tree, int Position, string group)
                {
                    this.Hand = Hand;
                    this.visualHand = new List<Sprite>();
                    PlayerVisualHand = new List<Sprite>();
                    this.Tree = Tree;
                    VisualHandPosition = new Vector2(500 - (Hand.Count * 5), Position);
                    this.group = group;
                }
                
            }

            public class VisualBattleField 
            {
                public static Vector2[] Player1FieldPositions;
                public static Vector2[] Player2FieldPositions;
                public Relics[] BattleField;
                public Sprite[] visualBattleField;

                public VisualBattleField(Relics[] BattleField, Sprite[] visualBattleField)
                {
                    this.BattleField = BattleField;
                    this.visualBattleField = visualBattleField;
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
                    
                }
            }
            public void UpdateVisualHand(VisualHand VisualHand)
            {
                // bool VirtualPlayer = false;
                // if (Game.player1 is VirtualPlayer)
                // {
                //     VirtualPlayer = true;
                // }
                if (VisualHand.visualHand.Count - VisualHand.Hand.Count >= 0)
                {
                
                    // Delete old relics
                    for(int i = 0; i < VisualHand.Hand.Count; i++)
                    {
                        try
                        {
                            if (((Label)VisualHand.visualHand[i].GetChild(0)).Text != VisualHand.Hand[i].name)
                            {
                                ((Node)Tree.GetNodesInGroup(VisualHand.group)[i]).QueueFree();
                                VisualHand.visualHand.RemoveAt(i);
                            }
                        }
                        catch (System.Exception)
                        {
                            throw new System.Exception("PP esto está dando error justo aqui, revisalo, xd");
                            if (VisualHand.visualHand.Count != 0)
                            {
                                VisualHand.visualHand.RemoveAt(i);
                                ((Node)Tree.GetNodesInGroup(VisualHand.group)[i]).QueueFree();
                            }
                        }
                        
                        if (i >= maxinHand)
                        {
                            selecting = true;
                            selectVisually(VisualHand.Hand, VisualHand.Hand.Count - maxinHand);
                        }
                    }
                }
                else if (VisualHand.visualHand.Count - VisualHand.Hand.Count < 0)
                {
                    //Create new relics in hand
                    for(int i = VisualHand.visualHand.Count; i < VisualHand.Hand.Count; i++)
                    {
                        Sprite relic;
                        if (VisualHand.Hand[i].Owner is VirtualPlayer) relic = InstanciateVisualBackCard(VisualHand.Hand[i]);
                        else relic = InstanciateVisualCard(VisualHand.Hand[i]);

                        VisualHand.visualHand.Add(Relic);
                        child.AddChild(relic);
                        relic.AddToGroup(VisualHand.group);
                        if (i >= maxinHand)
                        {
                            selecting = true;
                            selectVisually(VisualHand.Hand, VisualHand.Hand.Count - maxinHand);
                        }
                    }

                    VisualHand.VisualHandPosition.x = (1920/2) - VisualHand.visualHand.Count*(float)73.5;
                    for(int i = 0; i < VisualHand.visualHand.Count; i++)
                    {
                        VisualHand.visualHand[i].Position = new Vector2(VisualHand.VisualHandPosition.x + 200*i, VisualHand.VisualHandPosition.y);
                    }
                }

                if (Game.player1.hand.Count > maxinHand)
                {
                    selectVisually(Game.player1.hand, Game.player1.hand.Count-maxinHand);
                }
                if (Game.player2.hand.Count > maxinHand)
                {
                    selectVisually(Game.player2.hand, Game.player2.hand.Count-maxinHand);
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
            // public void resetVisualGame()
            // {
            //     child = new Node();
            //     Player2VisualHand = new List<Sprite>();
            //     Player1VisualHand = new List<Sprite>();

            //     selecting = false;

            //     Player1emptySlots = 4;
            //     Player2emptySlots = 4;

            //     selectCards = null;
            //     selectQuant = 1;
            //     SelectedCards = null;
            //     SourceToSelect = null;
            //     Game = null;
            //     VisualBoard = null;

            //     try
            //     {
            //         foreach (Node node in GetTree().GetNodesInGroup("RelicsNodes1"))
            //         {
            //             node.QueueFree();
            //         }
            //     }
            //     catch (System.Exception){}
                
            //     try
            //     {
            //         foreach (Node node in GetTree().GetNodesInGroup("RelicsNodes2"))
            //         {
            //             node.QueueFree();
            //         }
            //     }
            //     catch (System.Exception){}
            // }
        }
       
        public override void _Ready()
        {
            AddChild(child);
            Game = new Game(SelectPlayer.player1, SelectPlayer.player2);
            SceneTree pedro = GetTree();
            VisualBoard = new Board(Game, GetNode<Sprite>("GraveYard/Relic"), pedro);

            Preview = GetNode<Sprite>("Preview/Relic");
            Preview.Visible = false;
            // GD.Print(VisualBoard.visualHand1.group);
            // GD.Print(VisualBoard.visualHand1.Hand.Count);
            // GD.Print(VisualBoard.visualHand1.visualHand.Count);
            // GD.Print(VisualBoard.visualHand1.VisualHandPosition.Length());
            RefreshVisualHands();
        }


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
                ((VirtualPlayer)(Game.player1)).Play();
                // PAUSAR EL JUEGO AQUI PARA QUE SE VEAN LOS EFECTOS DEL VIRTUAL PLAYER/***************************************************************///
                AttackButtonFunction();
                EndButtonFunction();
            }

            Attack = GetNode<Button>("Attack");
            Button endButton = GetNode<Button>("endButton");

            // ATTACK BUTTON
            Attack.Disabled = false;
            AttackButtonFunction();

            //Change Turn (END BUTTON)
            if (endButton.Pressed)
            {
                EndButtonFunction();
                endButton.Disabled = true; // Disabling button, increment turn just one time
            }
            endButton.Disabled = false;

            Attack.Disabled = false;
            if (Attack.Pressed)
            {
                AttackButtonFunction();
                Attack.Disabled = true;
            }

            
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
                UpdateBattleField(Game.player1);
                RefreshVisualHands();
            }
            Game.turn++;
            Turnlabel.Text = "Turno: " + Game.turn;
        }
        public void AttackButtonFunction()
        {
            if (Game.turn % 2 == 0 && !player1Attack) // Player1's Turn
            {
                Game.player2.life -= Game.player1.character.attack;
                player1Attack = true;
                player2Attack = false; // Enabling attack button to next turn 
            }
            if (Game.turn % 2 != 0 && !player2Attack)// Player2's Turn
            {
                Game.player1.life -= Game.player2.character.attack;
                player2Attack = true;
                player1Attack = false; // Enabling attack button to next turn 
            }
        }
        public void RefreshVisualHands()
        {
            // Updating cards in board
            VisualBoard.UpdateVisualHand(VisualBoard.visualHand1);
            VisualBoard.UpdateVisualHand(VisualBoard.visualHand2);

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
            int PlayerEmptySlots;
            Sprite[] field;

            if (player == Game.player1)
            {
                PlayerEmptySlots = Player1emptySlots;
                field = VisualBoard.visualBattleField1.visualBattleField;
            }
            else
            {
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
                            int Defaultpassive = mainMenu.Inventory.CardsInventory[player.BattleField[index].id].passiveDuration;
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
        
        public void PreviewPropierties(Relics relic)
        {
            if (((Label)Preview.GetChild(0)).Text != relic.name)
            {
                Sprite Relic = board.InstanciateVisualCard(relic);
                Preview.Visible = true;
                ((Label)Preview.GetChild(0)).Text = ((Label)Relic.GetChild(0)).Text; //Name
                ((Sprite)Preview.GetChild(1)).Texture = ((Sprite)Relic.GetChild(1)).Texture; //Image
                ((Label)Preview.GetChild(2)).Text = ((Label)Relic.GetChild(2)).Text; //Duration
                ((Sprite)Preview.GetChild(3)).Texture = ((Sprite)Relic.GetChild(3)).Texture; //Type
                ((Label)Preview.GetChild(4)).Text = ((Label)Relic.GetChild(4)).Text; //Description

                // Scale of Image and Type sprites
                ((Sprite)Preview.GetChild(1)).Scale = new Vector2((float)0.60, (float)0.50);
                ((Sprite)Preview.GetChild(3)).Scale = new Vector2((float)0.15, (float)0.15);
            }
        }
        public void PauseGame(int time)
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            while (watch.ElapsedMilliseconds < time*1000)
            {
                GetTree().Paused = true;
            }
            GetTree().Paused = false;
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
                            for(int i = 0; i < VisualBoard.visualHand2.visualHand.Count; i++)
                            {
                                if  (VisualBoard.visualHand2.visualHand[i].GetRect().HasPoint(VisualBoard.visualHand2.visualHand[i].ToLocal(mouseEvent.Position)) && Player2emptySlots > 0)
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
                if (!(Game.player1 is VirtualPlayer))
                {
                    for (int i = 0; i < VisualBoard.visualHand1.visualHand.Count; i++)
                    {
                        if(VisualBoard.visualHand1.visualHand[i].GetRect().HasPoint(VisualBoard.visualHand1.visualHand[i].ToLocal(mouseMove.Position)))
                        {
                            PreviewPropierties(Game.player1.hand[i]);
                        }
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
                
                //Preview Player2 hand
                if (!(Game.player2 is VirtualPlayer))
                {
                    for (int i = 0; i < VisualBoard.visualHand2.visualHand.Count; i++)
                    {
                        if(VisualBoard.visualHand2.visualHand[i].GetRect().HasPoint(VisualBoard.visualHand2.visualHand[i].ToLocal(mouseMove.Position)))
                        {
                            Sprite Relic = board.InstanciateVisualCard(Game.player2.hand[i]);
                            AddChild(Relic);
                            Relic.Scale = new Vector2((float)0.44, (float)0.45);
                            Relic.Position = new Vector2(170, 500);
                        }
                    }
                }

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