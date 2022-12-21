using Godot;
using System.Collections.Generic;
using gameEngine;
using System.Diagnostics;
using gameVisual;
namespace gameVisual
{
    public class VisualMethods
    {
        #region Visual Objects

        public static Sprite Relic = new Sprite();
<<<<<<< HEAD
        public static Node boardNode = board.child;

        #endregion

=======

        public static bool selecting = false;
        // public static Player discardPlayer = new Player("default");
        // public static List<Button> discardButtons = new List<Button>();
        public static List<Sprite> selectCards;
        public static int selectQuant = 1;
        public static List<Relics> SelectedCards;
        public static List<Relics> SourceToSelect;

        #endregion

        public static Node boardNode;
>>>>>>> f1610c5e98bfb29019461e649bc64ededd148943

        public static void ListenToVisualButtons()
        {
            if (board.endButton.Pressed)
            {
                EndButtonFunction();
                board.endButton.Disabled = true; // Disabling button, increment turn just one time
            }
            board.endButton.Disabled = false;

            if (board.Attack.Pressed)
            {
                AttackButtonFunction();
            }
        }
        public static void UpdatePlayersVisualProperties()
        {
            boardNode = board.child;
            Label PlayerNick = boardNode.GetParent().GetNode<Label>("PlayerInfo/Player's Nick");
            PlayerNick.Text = board.Game.player2.nick;
            Label PlayerLife = boardNode.GetParent().GetNode<Label>("PlayerInfo/Player's Life");
            PlayerLife.Text = "Life : "+ board.Game.player2.life.ToString();
            Label PlayerShield = boardNode.GetParent().GetNode<Label>("PlayerInfo/Player's Shield");
            PlayerShield.Text = "Shield : "+ board.Game.player2.character.defense.ToString();
            Label PlayerAttack = boardNode.GetParent().GetNode<Label>("PlayerInfo/Player's Attack");
            PlayerAttack.Text = "Attack : "+ board.Game.player2.character.attack.ToString();
            Label Player1State = boardNode.GetParent().GetNode<Label>("PlayerInfo/Player's State");
            Player1State.Text = "State : "+ board.Game.player2.state.ToString();

            Label Player2Nick = boardNode.GetParent().GetNode<Label>("Player2Info/Player2's Nick");
            Player2Nick.Text = board.Game.player1.nick;
            Label Player2Life = boardNode.GetParent().GetNode<Label>("Player2Info/Player2's Life");
            Player2Life.Text = "Life : "+ board.Game.player1.life.ToString();
            Label Player2Shield = boardNode.GetParent().GetNode<Label>("Player2Info/Player2's Shield");
            Player2Shield.Text = "Shield : "+ board.Game.player1.character.defense.ToString();
            Label Player2Attack = boardNode.GetParent().GetNode<Label>("Player2Info/Player2's Attack");
            Player2Attack.Text = "Attack : "+ board.Game.player1.character.attack.ToString();
            Label Player2State = boardNode.GetParent().GetNode<Label>("Player2Info/Player2's State");
            Player2State.Text = "State : "+ board.Game.player1.state.ToString();
        }
        public static void EndButtonFunction()
        {
            board.Game.turn++;
            boardNode.GetParent().GetNode<Label>("TurnLabel").Text = "Turno: " + board.Game.turn;
            board.Attack.Disabled = false;
            if (board.Game.turn % 2 != 0) // Player2's turn
            {
                // Next Player takes a card
                board.Game.player2.TakeFromDeck(1);
                board.VisualBoard.UpdateBattleFields(board.Game.player2);
                board.VisualBoard.UpdateBattleFields(board.Game.player1);
            }
            else
            {
                // Next Player takes a card
                board.Game.player1.TakeFromDeck(1);
                board.VisualBoard.UpdateBattleFields(board.Game.player1);
                board.VisualBoard.UpdateBattleFields(board.Game.player2);
            }
                board.VisualBoard.Update();
        }
        public static void AttackButtonFunction()
        {
            if (board.Game.turn % 2 == 0) // Player1's Turn
            {
                board.Game.player2.life -= board.Game.player1.character.attack;
            }
            if (board.Game.turn % 2 != 0)// Player2's Turn
            {
                board.Game.player1.life -= board.Game.player2.character.attack;
            }
            board.Attack.Disabled = true;
            board.VisualBoard.Update();

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
        public static Sprite InstanciateVisualBackCard(Relics card)
        {
            PackedScene relic = (PackedScene)GD.Load("res://Back-relic.tscn");
            Relic = (Sprite)relic.Instance();
            Label name = (Label)Relic.GetChild(0);
            name.Text = card.name;

            return Relic;
        }
        public static void PreviewPropierties(Relics relic)
        {
            Sprite Preview = boardNode.GetParent().GetNode<Sprite>("Preview/Relic");
            if (((Label)Preview.GetChild(0)).Text != relic.name)
            {
                Sprite Relic = InstanciateVisualCard(relic);
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
        public static void AddtoVisualBattleField(Player Owner, Relics relic)
        {
            Sprite[] field = new Sprite[4];
            Vector2[] position = new Vector2[4];

            if (Owner == board.Game.player1)
            {
                field = board.VisualBoard.visualBattleField1.visualBattleField;
                position = board.VisualBoard.visualBattleField1.FieldPositions;
            }
            else
            {
                field = board.VisualBoard.visualBattleField2.visualBattleField;
                position = board.VisualBoard.visualBattleField2.FieldPositions;
            }

            for (int i = 0; i < field.Length; i++)
            {
                if(field[i] == null)
                {
                    field[i] = InstanciateVisualCard(relic);
                    break;
                }
            }
            
            // Fulling (visually) battlefield
            for (int slot = 0; slot < field.Length; slot++)
            {
                if (field[slot] != null)
                {
                    board.child.AddChild(field[slot]);
                    field[slot].Position = position[slot];
                }
            }
        }
<<<<<<< HEAD
       
=======
        public static void resetVisualGame()
        {
            VisualMethods.selecting = false;
            VisualMethods.selectCards = null;
            VisualMethods.selectQuant = 1;
            VisualMethods.SelectedCards = null;
            VisualMethods.SourceToSelect = null;

            board.child.GetParent().QueueFree();
            board.child.QueueFree();
        }
        public static void RemoveVisualCards(Board.VisualHand VisualHand)
        {
            int HandCount = 0;
            for(int i = 0; i < VisualHand.visualHand.Count; i++)
            {
                try
                {
                    if (((Label)VisualHand.visualHand[i].GetChild(0)).Text != VisualHand.Hand[HandCount].name)
                    {
                        VisualHand.visualHand[i].QueueFree();
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
                        VisualHand.visualHand[i].QueueFree();
                        VisualHand.visualHand.RemoveAt(i);
                        i--;
                    }
                }                        
            }
        }
        public static void CreateNewCards(Board.VisualHand VisualHand)
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
        public static void RefreshPositionCards(Board.VisualHand VisualHand)
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
        
        
>>>>>>> f1610c5e98bfb29019461e649bc64ededd148943
        #region Input Methods
        public static void PreviewHandCards(Board.VisualHand Hand, InputEventMouse mouseMove)
        {
            for (int i = 0; i < Hand.visualHand.Count; i++)
            {
                if(Hand.visualHand[i].GetRect().HasPoint(Hand.visualHand[i].ToLocal(mouseMove.Position)))
                {
                    VisualMethods.PreviewPropierties(Hand.Hand[i]);
                }
            }
        }
        public static void PreviewBattlefielCards(Board.VisualBattleField BattleField, InputEventMouse mouseMove)
        {
            for (int i = 0; i < BattleField.visualBattleField.Length; i++)
            {
                if (BattleField.visualBattleField[i] != null)
                {
                    if(BattleField.visualBattleField[i].GetRect().HasPoint(BattleField.visualBattleField[i].ToLocal(mouseMove.Position)))
                    {
                        VisualMethods.PreviewPropierties(BattleField.BattleField[i]);
                    }
                }
            }
        }
        public static void ActiveClickAction(InputEventMouseButton mouseEvent)
        {
            switch ((ButtonList)mouseEvent.ButtonIndex)
                {
                    case ButtonList.Left:

                        if (VisualMethods.selecting)
                        {
                            for(int i = 0; i < VisualMethods.selectCards.Count; i++)
                            {
                                if (VisualMethods.selectCards[i].GetRect().HasPoint(VisualMethods.selectCards[i].ToLocal(mouseEvent.Position)))
                                {
                                    if (VisualMethods.selectCards[i].Scale == new Vector2((float)0.30,(float)0.30))
                                    {
                                        VisualMethods.selectQuant++;
                                        VisualMethods.SelectedCards.Remove(VisualMethods.SourceToSelect[i]);
                                        VisualMethods.selectCards[i].Scale = new Vector2((float)0.25,(float)0.25);
                                    }
                                    else if (VisualMethods.selectQuant != 0)
                                    {
                                        VisualMethods.selectQuant--;
                                        VisualMethods.SelectedCards.Add(VisualMethods.SourceToSelect[i]);
                                        VisualMethods.selectCards[i].Scale = new Vector2((float)0.30,(float)0.30);
                                    }
                                    board.VisualBoard.Update();
                                }
                            }
                        }

                        if(board.Game.turn % 2 == 0)
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
                            for(int i = 0; i < board.VisualBoard.visualHand2.visualHand.Count; i++)
                            {
                                if  (board.VisualBoard.visualHand2.visualHand[i].GetRect().HasPoint(board.VisualBoard.visualHand2.visualHand[i].ToLocal(mouseEvent.Position)))
                                {
                                    Effect(board.Game.player2.hand[i]);
                                }
                            }
                        }
                        break;
                }
        }     
        public static void Effect(Relics relic)
        {
            InterpretEffect effect = new InterpretEffect();
            effect.Scan(relic);
            if(effect.Active)
            {

                board.Game.player2.AddtoBattleField(relic);
                AddtoVisualBattleField(relic.Owner, relic);
                board.VisualBoard.Update();
                effect.Scan(relic);
                board.VisualBoard.Update();
            }
        }
        public static void ActiveEscapeMenu()
        {
            PackedScene EscMenu = (PackedScene)GD.Load("res://PauseMenu.tscn");
            Node PauseMenu = (Node)EscMenu.Instance();
            boardNode.AddChild(PauseMenu);
        }

        #endregion
    }
}