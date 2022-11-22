using Godot;
using System.Collections.Generic;
namespace gameVisual
{
    public class Visual : Godot.Node2D
    {

        #region Visual Objects
        public const int maxinHand = 6;


        public static List<Sprite> Player1Hand = new List<Sprite>();
        public static List<Sprite> Player2Hand = new List<Sprite>();
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


        public static Sprite Relic = new Sprite();


        static int Player1emptySlots = 4;
        static int Player2emptySlots = 4;
        static Sprite[] Player1Activated = new Sprite[4];
        static Sprite[] Player2Activated = new Sprite[4];


        public static bool discarding = false;
        public static gameEngine.Player discardPlayer = default;
        public static List<Button> discardButtons = new List<Button>();
        static PackedScene DiscardScene = (PackedScene)GD.Load("res://DiscardLabel.tscn");


        public static Vector2 Player1HandPosition;
        public static Vector2 Player2HandPosition;
        #endregion


        public void RefreshBoard()
        {

            PackedScene relic = (PackedScene)GD.Load("res://Relic.tscn");
            
            Player1HandPosition = new Vector2(175 - (gameEngine.Settings.player1.hand.Count * 10), 532);
            Player2HandPosition = new Vector2(175 - (gameEngine.Settings.player2.hand.Count * 10), 12);
            

            // Erasing old data
            if (Player1Hand.Count != 0)
                Player1Hand.Clear();
            if (Player2Hand.Count != 0)
                Player2Hand.Clear();
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
                CheckAndDiscard(gameEngine.Settings.player1);
            }

            int enemyIndex = 1;
            // Updating enemy's cards in board
            foreach (var card in gameEngine.Settings.player2.hand)
            {
                Relic = InstanciateVisualCard(card);
                Relic.AddToGroup("VisibleCards");
                Player2Hand.Add(Relic);
                Relic.Position = new Vector2(Player2HandPosition.x + 115*enemyIndex, Player2HandPosition.y);
                GetNode<Node>(".").AddChild(Relic);
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
            
            Vector2 FirstDiscardPosition = new Vector2(0, 247);

            if (player.hand.Count > 6)
            {
                discardPlayer = player;
                Tree DiscardTscn = (Tree)DiscardScene.Instance();
                Label label = DiscardTscn.GetNode<Label>("DiscardLabel");
                label.Text = player.nick+" must discard:  "+(player.hand.Count - 6).ToString();
                GetNode<Node>(".").AddChild(DiscardTscn);
                DiscardTscn.AddToGroup("discardGroup");

                int index = 1;
                foreach (var card in player.hand)
                {
                    Sprite relic = InstanciateVisualCard(card);
                    Button button = InstanciateButton();
                    relic.Scale = new Vector2((float)0.3,(float)0.3);
                    GetNode<Node>(".").AddChild(relic);
                    relic.AddToGroup("discardGroup");
                    GetNode<Node>(".").AddChild(button);
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
        public void HandleEvent(InputEvent @event)
        {
            if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
            {
                switch ((ButtonList)mouseEvent.ButtonIndex)
                {
                    case ButtonList.Left:
                        
                        bool news = false;
                        
                        if(gameEngine.Settings.turn % 2 == 0)
                        {
                            // Player 1 is clicking
                            for(int i = 0; i < Player1Hand.Count; i++)
                            {
                                if (Player1Hand[i].GetRect().HasPoint(Player1Hand[i].ToLocal(mouseEvent.Position)) && Player1emptySlots > 0)
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
                                    GetNode<Node>(".").AddChild(gameEngine.Settings.player1.userVisualBattleField[slot]);
                                    gameEngine.Settings.player1.userVisualBattleField[slot].Position = Player1FieldPositions[slot];
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
                                    GetNode<Node>(".").AddChild(gameEngine.Settings.player2.userVisualBattleField[slot]);
                                    gameEngine.Settings.player2.userVisualBattleField[slot].Position = Player2FieldPositions[slot];
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
                        RefreshBoard();
                        break;
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
            Label name = (Label)Relic.GetChild(1);
            name.Text = card.name;
            return Relic;
        }
        public static void UpdateBattleField(gameEngine.Player player)
        {
            bool[] boolPlayerField;
            int PlayerEmptySlots;
            Sprite[] PlayerActivated;
            if (player == gameEngine.Settings.player1)
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
       
    }
}