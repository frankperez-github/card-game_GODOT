using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
namespace card_gameEngine
{
    public class board : Godot.Node2D
    {   
        static Player player1;
        static Player player2;
        public static List<Player> PlayersInventary = new List<Player>();
        public static Dictionary<int, Relics> CardsInventary= new Dictionary<int, Relics>();
        public static Dictionary<int, Character> CharactersInventary = new Dictionary<int, Character>();
        public static List<Relics> GraveYard = new List<Relics>();
        public static int turn = 1;
        const int maxinHand = 6;
        static Sprite Relic = new Sprite();
        Player discardPlayer = default;
        public static List<Button> discardButtons = new List<Button>();

        #region Visual Board elements
        public List<Sprite> Player1Hand = new List<Sprite>();
        public List<Sprite> Player2Hand = new List<Sprite>();
        static Vector2[] Player2FieldPositions = new Vector2[4]
        {
            new Vector2(345, 142),
            new Vector2(465, 142),
            new Vector2(585, 142),
            new Vector2(705, 142)
        };
        static bool[] boolPlayer2Field = new bool[4];

        static Vector2[] Player1FieldPositions = new Vector2[4]
        {
            new Vector2(345, 401),
            new Vector2(465, 401),
            new Vector2(585, 401),
            new Vector2(705, 401)
        };
        static bool[] boolPlayer1Field = new bool[4];

        static int Player1emptySlots = 4;
        static int Player2emptySlots = 4;

        static Sprite[] Player1Activated = new Sprite[4];
        static Sprite[] Player2Activated = new Sprite[4];

        #endregion

        PackedScene DiscardScene = (PackedScene)GD.Load("res://DiscardLabel.tscn");
        static bool discarding = false;

        public override void _Ready()
        {
            #region Defining characters
            CharactersInventary.Add(1, new Character("El dragón indiferente", 1, 0, "imgpath1", 10, 3));
            CharactersInventary.Add(2, new Character("El toro alado", 3, 0, "imgpath2", 0, 5));
            CharactersInventary.Add(3, new Character("La serpiente truhana", 1, 0, "imgpath3", 5, 0));
            CharactersInventary.Add(4, new Character("El tigre recursivo", 1, 0, "imgpath4", 8, 0));
            CharactersInventary.Add(5, new Character("El leon amistoso", 2, 0, "imgpath", 0, 1));
            #endregion

            Player defaultPlayer = new Player(CharactersInventary[1], "defaultName");
            #region Defining cards
            //Espada del Destino
            //Te suma 15 de ataque
            Dictionary<int, ActionInfo> card1Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card1Info = new ActionInfo(relativePlayer.Owner, 15);
            card1Dict.Add(5, card1Info);
            CardsInventary.Add(1, new Relics(defaultPlayer, defaultPlayer, 1, "Espada del destino", 0, 3, "img", false, "", "damage", card1Dict));

            //Capsula del Tiempo
            //Roba una carta del cementerio
            Dictionary<int, ActionInfo> card2Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card2Info = new ActionInfo(relativePlayer.Owner, 1,"deck");
            card2Dict.Add(3, card2Info);
            CardsInventary.Add(2,new Relics(defaultPlayer, defaultPlayer, 2, "Capsula del Tiempo", 0, 1, "imgpath2", false, "", "draw", card2Dict));

            //Anillo de Zeus
            //Ganas 5 de vida por cada carta en tu mano
            Player defaultPlayer1 = new Player(CharactersInventary[1], "pepito");
            Dictionary<int, ActionInfo> card3Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card3Info = new ActionInfo(relativePlayer.Owner, 5, 1, relativeFactor.OwnerHand);
            card3Dict.Add(4, card3Info);
            CardsInventary.Add(3 ,new Relics(defaultPlayer1, defaultPlayer, 3, "Anillo de Zeus", 0, 1, "imgpath3", false,  "", "cure", card3Dict));

            //Escudo de la pobreza
            //Trap, evita el 50% del daño del enemigo
            Dictionary<int, ActionInfo> card4Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card4Info = new ActionInfo(relativePlayer.Owner, 1, 0.5, relativeFactor.Fixed);
            card4Dict.Add(4, card4Info);
            CardsInventary.Add(4,new Relics(defaultPlayer, defaultPlayer, 4, "Escudo de la pobreza", 0, 1, "imgpath", true, "", "defense", card4Dict));

            //Libro de los secretos 
            //Robas 2 cartas del deck
            Dictionary<int, ActionInfo> card5Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card5Info = new ActionInfo(2, "deck");
            card5Dict.Add(1, card5Info);
            CardsInventary.Add(5,new Relics(defaultPlayer, defaultPlayer, 5, "Libro de los secretos", 0, 1, "imgpath4", false, "", "draw", card5Dict));
            
            //Caliz de la Venganza
            //Tu adversario descarta 2 cartas de su mano
            Dictionary<int, ActionInfo> card6Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card6Info = new ActionInfo(relativePlayer.Enemy, 2, "deck");
            card6Dict.Add(7, card6Info);
            CardsInventary.Add(6,new Relics(defaultPlayer, defaultPlayer, 6, "Caliz de la Venganza", 0, 1, "imgpath4", false, "", "draw", card6Dict));

            //Resfriado
            //El adversario queda congelado por 2 turnos
            Dictionary<int, ActionInfo> card7Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card7Info = new ActionInfo(relativePlayer.Enemy, State.Freezed);
            card7Dict.Add(8, card7Info);
            CardsInventary.Add(7,new Relics(defaultPlayer, defaultPlayer, 7, "Resfriado", 0, 2, "imgpath4", false, "", "state", card7Dict));

            //Objetivo enemigo
            //Destruye 1 reliquia que tenga activa el enemigo
            Dictionary<int, ActionInfo> card8Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card8Info = new ActionInfo(relativePlayer.Enemy, 1, "deck");
            card8Dict.Add(9, card8Info);
            CardsInventary.Add(8,new Relics(defaultPlayer, defaultPlayer, 8, "Objetivo Enemigo", 0, 1, "imgpath4", false, "", "trap", card8Dict));
            #endregion

            player1 = new Player(CharactersInventary[1], "Pepe el macho");
            player2 = new Player(CharactersInventary[2], "Juan la sombra");
            PlayersInventary.Add(player1);
            PlayersInventary.Add(player2);

            player1.TakeFromDeck(player1, player2, 5, new List<Relics>());
            player2.TakeFromDeck(player2, player1, 5, new List<Relics>());

            RefreshBoard();
            discardPlayer = player1;
        }

        public override void _Process(float delta)
        {
            Player player1 = PlayersInventary[0];
            Player player2 = PlayersInventary[1];

            Button Attack = GetNode<Button>("Attack");
            Button endButton = GetNode<Button>("endButton");

            Label Turnlabel = GetNode<Label>("TurnLabel");

            #region updating players' info
            Label PlayerNick = GetNode<Label>("PlayerInfo/Player's Nick");
            PlayerNick.Text = PlayersInventary[0].nick;
            Label PlayerLife = GetNode<Label>("PlayerInfo/Player's Life");
            PlayerLife.Text = "Life : "+ PlayersInventary[0].life.ToString();
            Label PlayerShield = GetNode<Label>("PlayerInfo/Player's Shield");
            PlayerShield.Text = "Shield : "+ PlayersInventary[0].defense.ToString();
            Label PlayerAttack = GetNode<Label>("PlayerInfo/Player's Attack");
            PlayerAttack.Text = "Attack : "+ PlayersInventary[0].attack.ToString();
            Label Player1State = GetNode<Label>("PlayerInfo/Player's State");
            Player1State.Text = "State : "+ PlayersInventary[0].state.ToString();

            Label Player2Nick = GetNode<Label>("Player2Info/Player2's Nick");
            Player2Nick.Text = PlayersInventary[1].nick;
            Label Player2Life = GetNode<Label>("Player2Info/Player2's Life");
            Player2Life.Text = "Life : "+ PlayersInventary[1].life.ToString();
            Label Player2Shield = GetNode<Label>("Player2Info/Player2's Shield");
            Player2Shield.Text = "Shield : "+ PlayersInventary[1].defense.ToString();
            Label Player2Attack = GetNode<Label>("Player2Info/Player2's Attack");
            Player2Attack.Text = "Attack : "+ PlayersInventary[1].attack.ToString();
            Label Player2State = GetNode<Label>("Player2Info/Player2's State");
            Player2State.Text = "State : "+ PlayersInventary[1].state.ToString();
            #endregion

            // Checking end of game
            if (!(player1.life > 0 && player2.life > 0))
            {
                GetTree().ChangeScene("res://GameOver.tscn");
            }
            
            Attack.Visible = true;
            if (turn % 2 == 0) // Player1's Turn
            {
                if (Attack.Pressed)
                {
                    player2.life -= player1.attack;
                    Attack.Visible = false;
                }
            }
            else // Player2's Turn
            {
                if (Attack.Pressed)
                {
                    player1.life -= player2.attack;
                    Attack.Visible = false;
                }
            }

            //Change Turn (END BUTTON)
            if (endButton.Pressed)
            {
                if (turn % 2 == 0) // Player1's turn
                {
                    // Next Player takes a card
                    player1.TakeFromDeck(player2, player1, 1, new List<Relics>());
                    UpdateBattleField(player1);
                }
                else // Player2's turn
                {
                    // Next Player takes a card
                    player2.TakeFromDeck(player1, player2, 1, new List<Relics>());
                    UpdateBattleField(player2);
                }
                RefreshBoard();
                turn++;
                Turnlabel.Text = "Turno: " + turn;
                endButton.Disabled = true; // Disabling button, increment turn just one time
            }
            endButton.Disabled = false;
        }
  


        public void RefreshBoard()
        {
            Player player1 = PlayersInventary[0];
            Player player2 = PlayersInventary[1];

            PackedScene relic = (PackedScene)GD.Load("res://Relic.tscn");
            
            Vector2 Player1HandPosition = new Vector2(175 - (player1.hand.Count * 10), 532);
            Vector2 Player2HandPosition = new Vector2(175 - (player2.hand.Count * 10), 12);

            // Erasing old data
            Player2Hand.Clear();
            Player1Hand.Clear();
            foreach (Node node in GetTree().GetNodesInGroup("VisibleCards"))
            {
                node.QueueFree();
            } 

            int index = 1;
            // Updating cards in board
            foreach (var card in player1.hand)
            {
                Relic = InstanciateRelic(card);
                Relic.AddToGroup("VisibleCards");
                Player1Hand.Add(Relic);
                Relic.Position = new Vector2(Player1HandPosition.x + 115*index, Player1HandPosition.y);
                AddChild(Relic);
                Label name = (Label)Relic.GetChild(1);
                name.Text = card.name;
                index++; 

            }
            if (index > maxinHand)
            {
                discarding = true;
                CheckAndDiscard(player1);

            }

            int enemyIndex = 1;
            // Updating enemy's cards in board
            foreach (var card in player2.hand)
            {
                Relic = InstanciateRelic(card);
                Relic.AddToGroup("VisibleCards");
                Player2Hand.Add(Relic);
                Relic.Position = new Vector2(Player2HandPosition.x + 115*enemyIndex, Player2HandPosition.y);
                AddChild(Relic);
                enemyIndex++; 
            }
            if (enemyIndex > maxinHand)
            {
                discarding = true;
                CheckAndDiscard(player2);
            }
        }
        public void CheckAndDiscard(Player player)
        {
            
            Vector2 FirstDiscardPosition = new Vector2(0, 247);

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
                    Sprite relic = board.InstanciateRelic(card);
                    Button button = board.InstanciateButton();
                    relic.Scale = new Vector2((float)0.3,(float)0.3);
                    AddChild(relic);
                    relic.AddToGroup("discardGroup");
                    AddChild(button);
                    button.AddToGroup("discardGroup");
                    discardButtons.Add(button);
                    button.SetPosition(new Vector2( 90 * index - 40, FirstDiscardPosition.y + 85) ,false);
                    relic.Position = new Vector2( 90 * index, FirstDiscardPosition.y); 
                    Label name = (Label)Relic.GetChild(1);
                    name.Text = card.name;
                    index++;
                }
            }
        }
        public static Sprite InstanciateRelic(Relics card)
        {
            PackedScene relic = (PackedScene)GD.Load("res://Relic.tscn");
            Relic = (Sprite)relic.Instance();
            Label name = (Label)Relic.GetChild(1);
            name.Text = card.name;
            return Relic;
        }
        public static Button InstanciateButton()
        {
            PackedScene relic = (PackedScene)GD.Load("res://DiscardButton.tscn");
            Button button = (Button)relic.Instance();
            return button;
        }
        public void UpdateBattleField(Player player)
        {
            bool[] boolPlayerField;
            int PlayerEmptySlots;
            Sprite[] PlayerActivated;
            if (player == player1)
            {
                boolPlayerField = boolPlayer1Field;
                PlayerEmptySlots = Player1emptySlots;
                PlayerActivated = Player1Activated;
            }
            else
            {
                boolPlayerField = boolPlayer2Field;
                PlayerEmptySlots = Player2emptySlots;
                PlayerActivated = Player2Activated;
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
                                player.userBattleField[index].Affected.state = State.Safe;
                            }
                        }
                        // Removing card from battelfield
                        PlayerEmptySlots++;
                        boolPlayerField[index] = false;
                        player.userVisualBattleField[index].QueueFree();
                        player.userVisualBattleField[index] = null;
                        GraveYard.Add(player.userBattleField[index]);
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
                            int Defaultpassive = CardsInventary[player.userBattleField[index].id].passiveDuration;
                            player.userBattleField[index].passiveDuration = Defaultpassive;
                            player.userBattleField[index].activeDuration--;
                        }
                    }

                }
            }


            if (player == player1)
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
                        
                        bool news = false;
                        
                        if (turn % 2 == 0)
                        {
                            // Player 1 is clicking
                            for(int i = 0; i < Player1Hand.Count; i++)
                            {
                                if (Player1Hand[i].GetRect().HasPoint(Player1Hand[i].ToLocal(mouseEvent.Position)) && Player1emptySlots > 0)
                                {
                                    // Add to player's battlefield logicaly and visualy
                                    player1.hand[i].Effect(); // Activating effect of card
                                    Player1emptySlots--;
                                    news = true;
                                }
                                
                            }

                            // Fulling (visualy) battlefield
                            for (int slot = 0; slot < player1.userBattleField.Length; slot++)
                            {
                                if (!boolPlayer1Field[slot] && news && player1.userVisualBattleField[slot] != null)
                                {
                                    AddChild(player1.userVisualBattleField[slot]);
                                    player1.userVisualBattleField[slot].Position = Player1FieldPositions[slot];
                                    boolPlayer1Field[slot] = true;
                                }
                            }
                            news = false;

                        }
                        else // Player2 is clicking
                        {
                            for(int i = 0; i < Player2Hand.Count; i++)
                            {
                                if (Player2Hand[i].GetRect().HasPoint(Player2Hand[i].ToLocal(mouseEvent.Position)) && Player2emptySlots > 0)
                                {
                                    // Add to player's battlefield
                                    player2.hand[i].Effect(); // Activating effect of card
                                    Player2emptySlots--;
                                    news = true;
                                }
                            }

                            // Fulling (visualy) battlefield
                            for (int slot = 0; slot < player2.userBattleField.Length; slot++)
                            {
                                if (!boolPlayer2Field[slot] && news && player2.userVisualBattleField[slot] != null)
                                {
                                    AddChild(player2.userVisualBattleField[slot]);
                                    player2.userVisualBattleField[slot].Position = Player2FieldPositions[slot];
                                    boolPlayer2Field[slot] = true;
                                }
                            }
                            news = false;

                        }
                        
                        if (discarding)
                        {
                            for (int i = 0; i < discardButtons.Count; i++)
                            {
                                if (discardButtons[i].GetRect().HasPoint(ToLocal(mouseEvent.Position)))
                                {
                                    GraveYard.Add(discardPlayer.hand[i]);
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
                        RefreshBoard();
                        break;
                }
            }
        }
    }       
}