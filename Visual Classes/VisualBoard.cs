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
                    else
                        GraveYardCard.Visible = false;
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
                            GD.Print(BattleField[index].name + " activeDuration: " + BattleField[index].activeDuration + " pasiveDuration: " + BattleField[index].passiveDuration);
                            if (BattleField[index].activeDuration == 1)
                            {
                                // Removing card from battelfield
                                foreach (var effect in BattleField[index].Actions)
                                {
                                    if(effect is Attack)
                                    {
                                        Attack Negate = new Attack("-" + effect.expressionA, effect.Relic, effect.Affected, effect.NotAffected, effect.Relic.Owner, effect.Relic.Enemy);
                                        Negate.Effect();
                                    }
                                    else if(effect is ChangeState)
                                    {
                                        ChangeState Negate = new ChangeState("Safe", effect.Relic, effect.Affected, effect.NotAffected, effect.Relic.Owner, effect.Relic.Enemy);
                                    }
                                }
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
                                    BattleField[index].activeDuration--;
                                }
                                else
                                {
                                    int Defaultpassive;
                                    if(BattleField[index] is Character)
                                    {
                                        Defaultpassive = mainMenu.Inventory.CharactersInventory[BattleField[index].id-1000].passiveDuration;
                                    }
                                    else
                                    {
                                        Defaultpassive = mainMenu.Inventory.CardsInventory[BattleField[index].id].passiveDuration;
                                    }
                                    BattleField[index].passiveDuration = Defaultpassive;
                                    BattleField[index].activeDuration--;
                                }
                            GD.Print(BattleField[index].name + " activeDuration: " + BattleField[index].activeDuration + " pasiveDuration: " + BattleField[index].passiveDuration);
                            }
                        }
                    }
                }
            }
            public void Update()
            {
                UpdateVisualHand(visualHand1);
                UpdateVisualHand(visualHand2);
                UpdateBattleField(visualBattleField1);
                UpdateBattleField(visualBattleField2);
                VisualMethods.UpdatePlayersVisualProperties();
                visualGraveYard.Show();

                if (visualHand1.Hand.Count > Game.MaxInHand)
                {
                    if(board.Game.player1 is VirtualPlayer)
                        ((VirtualPlayer)(board.Game.player1)).Discard(visualHand1.Hand.Count-Game.MaxInHand);
                    else
                    Discard(visualHand1.Hand, visualHand1.Hand.Count-Game.MaxInHand);
                }
                if (visualHand2.Hand.Count > Game.MaxInHand)
                {
                    if(board.Game.player2 is VirtualPlayer)
                        ((VirtualPlayer)(board.Game.player2)).Discard(visualHand2.Hand.Count-Game.MaxInHand);
                    else 
                    Discard(visualHand2.Hand, visualHand2.Hand.Count-Game.MaxInHand);
                }
                CheckSpecialAttack(visualHand1);
                CheckSpecialAttack(visualHand2);
            }
            public void CheckSpecialAttack(VisualHand player)
            {
                int count = 0;
                foreach (var card in player.Hand)
                {
                    if(card.name == "Token")
                    {
                        count++;
                    }
                }
                if(count >= 2)
                {
                    count = 0;
                    for (int i = 0; i < player.Hand.Count; i++)
                    {
                        if(count < 2 || player.Hand[i].name == "Token")
                        {
                            player.Hand.Remove(player.Hand[i]);
                            count++;
                        }
                    }
                    VisualMethods.Effect(player.Hand[0].Owner.character, false);
                }
            }
            public void UpdateBattleField(VisualBattleField battleField)
            {
                for (int i = 0; i < battleField.BattleField.Length; i++)
                {
                    if(battleField.BattleField[i] == null)
                    {   
                        if(!(battleField.visualBattleField[i] == null))
                        {
                            battleField.visualBattleField[i].QueueFree();
                            battleField.visualBattleField[i] = null;
                        }
                    }
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
            public void Discard(List<Relics> playerHand, int count)
            {
                if (VisualMethods.SelectedCards.Count == 0)
                {
                    VisualMethods.selectVisually("Select to discard", playerHand, count, Discard, playerHand);
                }
                else
                {
                    for (int i = 0; i < VisualMethods.SelectedCards.Count; i++)
                    {
                        board.Game.GraveYard.Add(VisualMethods.SelectedCards[i]);
                        playerHand[0].Owner.hand.Remove(VisualMethods.SelectedCards[i]);
                    }
                    VisualMethods.SelectedCards = new List<Relics>();
                }
                UpdateVisualHand(visualHand1);
                UpdateVisualHand(visualHand2);
                VisualMethods.UpdatePlayersVisualProperties();
                visualGraveYard.Show();
            }

        }
       
}