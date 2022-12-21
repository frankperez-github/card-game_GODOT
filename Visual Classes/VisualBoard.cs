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
                    new Vector2(570, 725),
                    new Vector2(860, 725),
                    new Vector2(1145, 725),
                    new Vector2(1430, 725)
                };
                Vector2[] Player1FieldPositions = new Vector2[4]
                {
                    new Vector2(570, 330),
                    new Vector2(860, 330),
                    new Vector2(1145, 330),
                    new Vector2(1430, 330)
                };
                this.visualGraveYard = new VisualGraveYard(game.GraveYard, GraveYard);
                this.visualBattleField1 = new VisualBattleField(game.player1.BattleField, Player1FieldPositions);
                this.visualBattleField2 = new VisualBattleField(game.player2.BattleField, Player2FieldPositions);
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
                            ((Sprite)GraveYardCard.GetChild(1)).Scale = new Vector2((float)0.60, (float)0.50);
                            ((Sprite)GraveYardCard.GetChild(3)).Scale = new Vector2((float)0.15, (float)0.15);
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
                    RemoveVisualCards(VisualHand);
                }
                else if (VisualHand.visualHand.Count - VisualHand.Hand.Count < 0)
                {
                    CreateNewCards(VisualHand);
                }
                RefreshPositionCards(VisualHand);

                // if (board.Game.player1.hand.Count > Game.MaxInHand)
                // {
                //     Discard(board.Game.player1);
                // }
                // if (board.Game.player2.hand.Count > Game.MaxInHand)
                // {
                //     Discard(board.Game.player2);
                // }        
            }
            public void Discard(Player player)
            {
                selectVisually(player.hand, player.hand.Count-Game.MaxInHand);
            }
            public void CreateNewCards(VisualHand VisualHand)
            {
                for(int i = VisualHand.visualHand.Count; i < VisualHand.Hand.Count; i++)
                {
                    Sprite relic;
                    if (VisualHand.Hand[i].Owner is VirtualPlayer) relic = VisualMethods.InstanciateVisualBackCard(VisualHand.Hand[i]);
                    else relic = VisualMethods.InstanciateVisualCard(VisualHand.Hand[i]);

                    VisualHand.visualHand.Add(VisualMethods.Relic);
                    board.child.AddChild(relic);
                    relic.AddToGroup(VisualHand.group);
                }   
            }
            public void RefreshPositionCards(VisualHand VisualHand)
            {
                float InitialPosition = (1920/2) - (((VisualHand.visualHand.Count*104) + (50 * (VisualHand.visualHand.Count-1)))/2);
                for(int i = 0; i < VisualHand.visualHand.Count; i++)
                {
                    if(i==0)
                    {
                        VisualHand.visualHand[0].Position = new Vector2(InitialPosition, VisualHand.VisualHandPosition.y);
                    }
                    else
                    {
                        VisualHand.visualHand[i].Position = new Vector2(VisualHand.visualHand[i-1].Position.x + 200, VisualHand.VisualHandPosition.y);
                    }
                    VisualHand.visualHand[i].ZIndex = 1;
                }
            }
            public void RemoveVisualCards(VisualHand VisualHand)
            {
                int HandCount = 0;
                for(int i = 0; i < VisualHand.visualHand.Count; i++)
                {
                    try
                    {
                        if (((Label)VisualHand.visualHand[i].GetChild(0)).Text != VisualHand.Hand[HandCount].name)
                        {
                            ((Node)Tree.GetNodesInGroup(VisualHand.group)[i]).QueueFree();
                            VisualHand.visualHand.RemoveAt(i);
                            i--;
                        }
                        else
                        {
                            HandCount++;
                        }
                    }
                    catch (System.Exception)
                    {
                        if (VisualHand.visualHand.Count != 0)
                        {
                            ((Node)Tree.GetNodesInGroup(VisualHand.group)[i]).QueueFree();
                            VisualHand.visualHand.RemoveAt(i);
                            i--;
                        }
                    }                        
                }
            }
            public void selectVisually(List<Relics> Source, int quant)
            {
                VisualMethods.SelectedCards = new List<Relics>();
                VisualMethods.selectCards = new List<Sprite>();
                VisualMethods.SourceToSelect = Source;
                VisualMethods.selectQuant = quant;
                PackedScene SelectCards = (PackedScene)GD.Load("res://SelectCards.tscn");
                Tree SelectMenu = (Tree)SelectCards.Instance();
                AddChild(SelectMenu);
                
                board.AcceptButton.SetPosition(new Vector2(875, 720));
                
                Vector2 FirstPosition = new Vector2(180, 450);

                // Showing cards to select
                int index = 1;
                foreach (var card in Source)
                {
                    Sprite Card = VisualMethods.InstanciateVisualCard(card);
                    VisualMethods.selectCards.Add(Card);
                    AddChild(Card);
                    Card.Position = new Vector2(210 * index, FirstPosition.y); 
                    index++;
                }

                VisualMethods.selecting = true;
                if (VisualMethods.selectQuant == 0)
                {
                    VisualMethods.selecting = false;
                }

                if(board.AcceptButton.Pressed)
                {
                    GD.Print(VisualMethods.selectCards);
                    foreach (Node node in VisualMethods.selectCards)
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

            //     VisualMethods.selecting = false;

            //     Player1emptySlots = 4;
            //     Player2emptySlots = 4;

            //     VisualMethods.selectCards = null;
            //     VisualMethods.selectQuant = 1;
            //     VisualMethods.SelectedCards = null;
            //     VisualMethods.SourceToSelect = null;
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
       
}