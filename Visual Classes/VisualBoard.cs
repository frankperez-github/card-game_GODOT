using Godot;
using System.Collections.Generic;
using gameEngine;
using System.Diagnostics;
namespace gameVisual
{
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
                Vector2[] Player2FieldPositions = new Vector2[4]
                {
                    new Vector2(575, 725),
                    new Vector2(860, 725),
                    new Vector2(1145, 725),
                    new Vector2(1430, 725)
                };
                Vector2[] Player1FieldPositions = new Vector2[4]
                {
                    new Vector2(575, 330),
                    new Vector2(860, 330),
                    new Vector2(1145, 330),
                    new Vector2(1430, 330)
                };
                this.visualGraveYard = new VisualGraveYard(game.GraveYard, GraveYard);
                this.visualBattleField1 = new VisualBattleField(game.player1.BattleField, Player1FieldPositions);
                this.visualBattleField2 = new VisualBattleField(game.player2.BattleField, Player2FieldPositions);
                this.visualHand1 = new VisualHand(game.player1.hand, Tree, 50, "RelicsNodes1");
                this.visualHand2 = new VisualHand(game.player2.hand, Tree, 1000, "RelicsNodes2");
                this.Tree = Tree;
            }

            public class VisualGraveYard 
            {
                public List<Relics> graveYard;
                // List<Sprite> visualGraveYard;
                public Sprite GraveYardCard;

                public VisualGraveYard(List<Relics> graveYard, Sprite GraveYard)
                {
                    this.graveYard = graveYard;
                    GraveYardCard = GraveYard;
                    GraveYardCard.Visible = false;

                }
                public void Show()
                {
                    if (board.Game.GraveYard.Count != 0)
                    {
                        Relics relic = graveYard[graveYard.Count-1];
                        if (((Label)GraveYardCard.GetChild(0)).Text != relic.name)
                        {
                            Sprite Relic = VisualMethods.InstanciateVisualCard(relic);
                            GraveYardCard.Visible = true;
                            ((Label)GraveYardCard.GetChild(0)).Text = ((Label)Relic.GetChild(0)).Text;
                            ((Sprite)GraveYardCard.GetChild(1)).Texture = ((Sprite)Relic.GetChild(1)).Texture;
                            ((Label)GraveYardCard.GetChild(2)).Text = ((Label)Relic.GetChild(2)).Text;
                            ((Sprite)GraveYardCard.GetChild(3)).Texture = ((Sprite)Relic.GetChild(3)).Texture;
                            ((Label)GraveYardCard.GetChild(4)).Text = ((Label)Relic.GetChild(4)).Text;
                            ((Sprite)GraveYardCard.GetChild(1)).Scale = new Vector2((float)0.96, (float)0.72);
                            ((Sprite)GraveYardCard.GetChild(3)).Scale = new Vector2((float)0.25, (float)0.25);
                        }
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
                public Vector2[] FieldPositions;
                public Relics[] BattleField;
                public Sprite[] visualBattleField;

                public VisualBattleField(Relics[] BattleField, Vector2[] Position)
                {
                    this.BattleField = BattleField;
                    this.visualBattleField = new Sprite[4];
                    FieldPositions = Position;
                }
                public void UpdateBattleField()
                {
                    Sprite[] field = visualBattleField;

                    for (int index = 0; index < BattleField.Length; index++)
                    {
                        if (BattleField[index] != null)
                        {
                            
                            if (BattleField[index].activeDuration == 1)
                            {
                                // Removing card from battelfield
                                field[index].QueueFree();
                                field[index] = null;
                                board.Game.GraveYard.Add(BattleField[index]);
                                BattleField[index] = null; 
                            }
                            else
                            {
                                if (BattleField[index].passiveDuration != 0)
                                {
                                    BattleField[index].passiveDuration--;
                                }
                                else
                                {
                                    int Defaultpassive = mainMenu.Inventory.CardsInventory[BattleField[index].id].passiveDuration;
                                    BattleField[index].passiveDuration = Defaultpassive;
                                    BattleField[index].activeDuration--;
                                }
                            }

                        }
                    }
                }
            }
            public void Update()
            {
                UpdateVisualHand(visualHand1);
                UpdateVisualHand(visualHand2);
                VisualMethods.UpdatePlayersVisualProperties();
                visualGraveYard.Show();

                if (board.Game.player1.hand.Count > Game.MaxInHand)
                {
                    if(board.Game.player1 is VirtualPlayer)
                        ((VirtualPlayer)(board.Game.player1)).Discard(board.Game.player1.hand.Count-Game.MaxInHand);
                    else Discard(board.Game.player1.hand);
                }
                if (board.Game.player2.hand.Count > Game.MaxInHand)
                {
                    if(board.Game.player2 is VirtualPlayer)
                        ((VirtualPlayer)(board.Game.player2)).Discard(board.Game.player2.hand.Count-Game.MaxInHand);
                    else Discard(board.Game.player2.hand);
                }    
            }
            public void UpdateBattleFields(Player player)
            {
                if(player == board.Game.player1)
                {
                    visualBattleField1.UpdateBattleField();
                }
                else
                {
                    visualBattleField2.UpdateBattleField();
                }
            }
            public void UpdateVisualHand(VisualHand VisualHand)
            {
                if (VisualHand.visualHand.Count - VisualHand.Hand.Count >= 0)
                {
                    VisualMethods.RemoveVisualCards(VisualHand);
                }
                else if (VisualHand.visualHand.Count - VisualHand.Hand.Count < 0)
                {
                    VisualMethods.CreateNewCards(VisualHand);
                }
                VisualMethods.RefreshPositionCards(VisualHand);
  
            }
            public void Discard(List<Relics> playerHand)
            {
                if (VisualMethods.SelectedCards == null)
                {
                    VisualMethods.selectVisually(playerHand, playerHand.Count-Game.MaxInHand, Discard, playerHand);
                }
                else
                {
                    foreach (var item in VisualMethods.SelectedCards)
                    {
                        board.Game.GraveYard.Add(item);
                        playerHand.Remove(item);
                    }
                    VisualMethods.SelectedCards = null;
                }
                UpdateVisualHand(visualHand1);
                UpdateVisualHand(visualHand2);
                VisualMethods.UpdatePlayersVisualProperties();
                visualGraveYard.Show();
            }

        }
       
}