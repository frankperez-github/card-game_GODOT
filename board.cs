using Godot;
using System;
using System.Collections.Generic;
namespace card_gameEngine
{
    public class board : Godot.Node2D
    {   
        public Player player1;
        public Player player2;
        public static List<Player> PlayersInventary = new List<Player>();
        public static Dictionary<int, Relics> CardsInventary= new Dictionary<int, Relics>();
        public static Dictionary<int, Character> CharactersInventary = new Dictionary<int, Character>();
        public static List<int> GraveYard = new List<int>();
        public static int turn = 1;
        const int maxinHand = 6;
        static Sprite Relic = new Sprite();
        Player discardPlayer = default;
        public static List<Button> discardButtons = new List<Button>();

        #region Visual Board elements
        public List<Sprite> Player1Hand = new List<Sprite>();
        public List<Sprite> Player2Hand = new List<Sprite>();
        Vector2[] Player2Field = new Vector2[4]
        {
            new Vector2(345, 142),
            new Vector2(465, 142),
            new Vector2(585, 142),
            new Vector2(705, 142)
        };
        bool[] boolPlayer2Field = new bool[4];

        Vector2[] Player1Field = new Vector2[4]
        {
            new Vector2(345, 401),
            new Vector2(465, 401),
            new Vector2(585, 401),
            new Vector2(705, 401)
        };
        bool[] boolPlayer1Field = new bool[4];

        int emptySlots = 4;


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
            CardsInventary.Add(1, new Relics(defaultPlayer, defaultPlayer, 1, "Espada del destino", 0, 3, "img", false, new Condition(), card1Dict));

            //Capsula del Tiempo
            //Roba una carta del cementerio
            Dictionary<int, ActionInfo> card2Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card2Info = new ActionInfo(relativePlayer.Owner, 1);
            card2Dict.Add(3, card2Info);
            CardsInventary.Add(2,new Relics(defaultPlayer, defaultPlayer, 2, "Capsula del Tiempo", 0, 1, "imgpath2", false,new Condition(), card2Dict));

            //Anillo de Zeus
            //Ganas 5 de vida por cada carta en tu mano
            Player defaultPlayer1 = new Player(CharactersInventary[1], "pepito");
            Dictionary<int, ActionInfo> card3Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card3Info = new ActionInfo(relativePlayer.Owner, 5, 1, relativeFactor.OwnerHand);
            card3Dict.Add(4, card3Info);
            CardsInventary.Add(3 ,new Relics(defaultPlayer1, defaultPlayer, 3, "Anillo de Zeus", 0, 1, "imgpath3", false,  new Condition(), card3Dict));

            //Escudo de la pobreza
            //Trap, evita el 50% del dano del enemigo
            Dictionary<int, ActionInfo> card4Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card4Info = new ActionInfo(relativePlayer.Owner, 1, 0.5, relativeFactor.Fixed);
            card4Dict.Add(6, card4Info);
            CardsInventary.Add(4,new Relics(defaultPlayer, defaultPlayer, 4, "Escudo de la pobreza", 0, 1, "imgpath", true, new Condition(), card4Dict));

            //Libro de los secretos 
            //Robas 2 cartas del deck
            Dictionary<int, ActionInfo> card5Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card5Info = new ActionInfo(2, new List<int>());
            card5Dict.Add(1, card5Info);
            CardsInventary.Add(5,new Relics(defaultPlayer, defaultPlayer, 5, "Libro de los secretos", 0, 1, "imgpath4", false, new Condition(), card5Dict));
            
            //Caliz de la Venganza
            //Tu adversario descarta 2 cartas de su mano
            Dictionary<int, ActionInfo> card6Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card6Info = new ActionInfo(relativePlayer.Enemy, 2, new List<int>());
            card6Dict.Add(7, card6Info);
            CardsInventary.Add(6,new Relics(defaultPlayer, defaultPlayer, 5, "Libro de los secretos", 0, 1, "imgpath4", false, new Condition(), card6Dict));
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

            Label Player2Nick = GetNode<Label>("Player2Info/Player2's Nick");
            Player2Nick.Text = PlayersInventary[1].nick;
            Label Player2Life = GetNode<Label>("Player2Info/Player2's Life");
            Player2Life.Text = "Life : "+ PlayersInventary[1].life.ToString();
            Label Player2Shield = GetNode<Label>("Player2Info/Player2's Shield");
            Player2Shield.Text = "Shield : "+ PlayersInventary[1].defense.ToString();
            Label Player2Attack = GetNode<Label>("Player2Info/Player2's Attack");
            Player2Attack.Text = "Attack : "+ PlayersInventary[1].attack.ToString();
            #endregion

            // Checking end of game
            if (!(player1.life > 0 && player2.life > 0))
            {
                if (player1.life <= 0)
                {
                    player1.life = 0;
                    Attack.Disabled = true;
                    Turnlabel.Text = player2.nick + " wins";
                }

                if (player2.life <= 0)
                {
                    player2.life = 0;
                    Attack.Disabled = true;
                    Turnlabel.Text = player1.nick + " wins";
                }
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
                    player2.TakeFromDeck(player1, player2, 1, new List<Relics>());
                    UpdateBattleField(player1);
                    emptySlots = 4;
                }
                else // Player2's turn
                {
                    // Next Player takes a card
                    player1.TakeFromDeck(player2, player1, 1, new List<Relics>());
                    UpdateBattleField(player2);
                    emptySlots = 4;
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
        public static void UpdateBattleField(Player player)
        {
            for (int index = 0; index < player.userBattleField.Count; index++)
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
                    }
                    // Removing card from battelfield
                    GraveYard.Add(player.userBattleField[index].id);
                    player.userBattleField.Remove(player.userBattleField[index]); 
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
        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
            {
                switch ((ButtonList)mouseEvent.ButtonIndex)
                {
                    case ButtonList.Left:
                        
                        if (turn % 2 == 0)
                        {
                            // Player 1 is clicking
                            for(int i = 0; i < Player1Hand.Count; i++)
                            {
                                if (Player1Hand[i].GetRect().HasPoint(Player1Hand[i].ToLocal(mouseEvent.Position)) && emptySlots > 0)
                                {
                                    // Add to player's battlefield
                                    player1.hand[i].Effect(); // Activating effect of card
                                    player1.hand.Remove(player1.hand[i]);
                                    emptySlots--;

                                    RefreshBoard();
                                }
                                
                            }

                            // Fulling (visualy) battlefield
                            for (int slot = 0; slot < player1.userBattleField.Count; slot++)
                            {
                                if (!boolPlayer1Field[slot] && emptySlots < 4)
                                {
                                    Sprite battleCard = InstanciateRelic(player1.userBattleField[slot]);
                                    AddChild(battleCard);
                                    battleCard.Position = Player1Field[slot];
                                    boolPlayer1Field[slot] = true;
                                }
                            }
                        }
                        else // Player2 is clicking
                        {

                            for(int i = 0; i < Player2Hand.Count; i++)
                            {
                                if (Player2Hand[i].GetRect().HasPoint(Player2Hand[i].ToLocal(mouseEvent.Position)) && emptySlots > 0)
                                {
                                    // Add to player's battlefield
                                    player2.hand[i].Effect(); // Activating effect of card
                                    player2.hand.Remove(player2.hand[i]);
                                    emptySlots--;

                                    RefreshBoard();
                                }
                                
                            }

                            // Fulling (visualy) battlefield
                            for (int slot = 0; slot < player2.userBattleField.Count; slot++)
                            {
                                if (!boolPlayer2Field[slot] && emptySlots < 4)
                                {
                                    Sprite battleCard = InstanciateRelic(player2.userBattleField[slot]);
                                    AddChild(battleCard);
                                    battleCard.Position = Player2Field[slot];
                                    boolPlayer2Field[slot] = true;
                                }
                            }
                        }
                        
                        if (discarding)
                        {
                            for (int i = 0; i < discardButtons.Count; i++)
                            {
                                if (discardButtons[i].GetRect().HasPoint(ToLocal(mouseEvent.Position)))
                                {
                                    GraveYard.Add(discardPlayer.hand[i].id);
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
                                    RefreshBoard();
                                }
                            }
                        }

                        break;
                }
            }
        }
    }       
}