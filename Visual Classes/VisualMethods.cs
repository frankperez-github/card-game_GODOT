using Godot;
using System.Collections.Generic;
using gameEngine;
using System.Diagnostics;
using System;
namespace gameVisual
{
    public class VisualMethods
    {
        #region Visual Objects

        public static Sprite Relic = new Sprite();
        public static bool selecting = false;
        public static List<Relics> SelectedCards = new List<Relics>();
<<<<<<< HEAD
        const int partitionLength = 5;
=======
        public const int partitionLength = 5;
>>>>>>> 8b7ae770f5b3bbb8db4a92b528a14f3b541f8bd5
        #endregion


        public static Node boardNode;
        public static void selectVisually(string sourceName, List<Relics> Source, int quant, Action<List<Relics>, int> Delegate, List<Relics> target)
        {
            SelectCards.SelectDelegate = Delegate;
            int partitions = GetPartitions(Source, partitionLength);
            SetSelectCardsProperties(sourceName, partitions, 0, Source, quant);
        }
        public static void selectVisually(string sourceName, List<Relics> Source, int quant, InterpretAction Delegate, List<Relics> target)
        {
            SelectCards.action = Delegate;
            int partitions = GetPartitions(Source, partitionLength);
            SetSelectCardsProperties(sourceName, partitions, 0, Source, quant);
        }
        private static int GetPartitions(List<Relics> Source, int partitionLength)
        {
            int partitions = 1;
            if (Source.Count > partitionLength)
            {
                partitions = Source.Count/partitionLength;
                if (Source.Count % partitionLength != 0) partitions++;
            }
            for(int i = 0; i < partitions; i++)
            {
                if (!SelectCards.SelectedIndexes.ContainsKey(i))
                {
                    SelectCards.SelectedIndexes.Add(i, new List<int>());
                }
            }
            return partitions;
        }
        public static void SetSelectCardsProperties(string tittle, int partitions, int actualSwipe, List<Relics> Source, int quant)
        {
            Node SelectMenu = null;
            if (SelectCards.partitions == 0)
            {
                SelectMenu = SelectCards.SelectCardsScene.Instance();
                board.child.AddChild(SelectMenu);
                SelectCards.SelectCardInstance = SelectMenu;
                SelectCards.selectCards = new List<Sprite>();
            }
            SelectCards.selectName = tittle;
            VisualMethods.selecting = true;
            VisualMethods.SelectedCards = new List<Relics>();
<<<<<<< HEAD
            SelectCards.selectCards = new List<Sprite>();
=======
            RemoveSprites(SelectCards.selectCards);
>>>>>>> 8b7ae770f5b3bbb8db4a92b528a14f3b541f8bd5
            SelectCards.Source = Source;
            SelectCards.selectQuant = quant;
            SelectCards.Source = Source;
            SelectCards.partitions = partitions;
            SelectCards.actualSwipe = actualSwipe;
            SelectCards.SelectLabel = SelectCards.SelectCardInstance.GetNode<Label>("Tree/DiscardLabel");
            SelectCards.SelectLabel.Text = tittle;
            board.child.GetTree().Paused = true;

            int quantCards = Source.Count;
            if(quantCards > partitionLength) quantCards = partitionLength;
            
            float InitialPosition = (1010) - ((200 * (quantCards-1))/2);
            Vector2 FirstPosition = new Vector2(InitialPosition, 470);


            // Showing cards to select
            int index = 0;
            SelectCards.selectCards = new List<Sprite>();
            for (int i = actualSwipe*partitionLength; i < (actualSwipe*partitionLength)+partitionLength; i++)
            {
                if (i < Source.Count && Source[i] != null)
                {
                    Sprite Card = VisualMethods.InstanciateVisualCard(Source[i]);
                    Card.ZIndex = 7;
                    SelectCards.selectCards.Add(Card);
                    SelectCards.SelectCardInstance.AddChild(Card);
                    // Remembering selected cards
                    if(SelectCards.SelectedIndexes[actualSwipe].Contains(i)) // Selected card
                    {
                        Card.Scale = new Vector2((float)0.20,(float)0.20);
                    }
                    else // Not selected card
                    {
                        Card.Scale = new Vector2((float)0.17,(float)0.17);
                    }
                    
                    // Adjusting position of cards to quantity in source
                    if(index==0)
                    {
                        SelectCards.selectCards[index].Position = new Vector2(FirstPosition.x, FirstPosition.y);
                    }
                    else
                    {
                        SelectCards.selectCards[index].Position = new Vector2(SelectCards.selectCards[index-1].Position.x + 200, FirstPosition.y);
                    }
                    
                    index++;
                }
            }
        }
        public static void ListenToVisualButtons(Player player)
        {
            if (board.endButton.Pressed)
            {
                EndButtonFunction(player);
                board.endButton.Disabled = true; // Disabling button, increment turn just one time
            }
            board.endButton.Disabled = false;
            if(player.state == State.Freezed)
            {
                board.Attack.Disabled = true;
            }
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
        public static void EndButtonFunction(Player player)
        {
            // AudioStreamPlayer2D click = boardNode.GetParent().GetNode<AudioStreamPlayer2D>("endButton/click");
            // click.Play();
            board.Game.turn++;
            boardNode.GetParent().GetNode<Label>("TurnLabel").Text = "Turno: " + board.Game.turn;
            board.Attack.Disabled = false;
            board.VisualBoard.UpdateBattleFields(board.Game.player2);
            board.VisualBoard.UpdateBattleFields(board.Game.player1);

            if(player.Enemy.state == State.Poisoned)
            {
                player.Enemy.life -= 10;
            }
            if(player.Enemy.state == State.Asleep)
            {
                board.VisualBoard.Update();
                EndButtonFunction(player.Enemy);
            }
            if (board.Game.turn % 2 != 0) // Player2's turn
            {
                // Next Player takes a card
                board.Game.player2.TakeFromDeck(1);
                for (int i = 0; i < board.Game.player2.BattleField.Length; i++)
                {
                    if(board.Game.player2.BattleField[i] != null && board.Game.player2.BattleField[i].passiveDuration == 0)
                    {
                        InterpretEffect effect = new InterpretEffect();
                        board.Game.player2.BattleField[i].Effect(effect);
                    }
                }
            }
            else
            {
                // Next Player takes a card
                board.Game.player1.TakeFromDeck(1);
                for (int i = 0; i < board.Game.player1.BattleField.Length; i++)
                {
                    if(board.Game.player1.BattleField[i] != null && board.Game.player1.BattleField[i].passiveDuration == 0)
                    {
                        InterpretEffect effect = new InterpretEffect();
                        board.Game.player1.BattleField[i].Effect(effect);
                    }
                }
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
            img.Scale = new Vector2((float)0.96, (float)0.74);


            description.Text = card.description;

            duration.Text = card.activeDuration.ToString();

            type.Texture = Type;
            type.Scale = new Vector2((float)0.25, (float)0.25);
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
            img.Scale = new Vector2((float)0.96, (float)0.77);
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
                ((Sprite)Preview.GetChild(1)).Scale = new Vector2((float)0.96, (float)0.74);
                ((Sprite)Preview.GetChild(3)).Scale = new Vector2((float)0.25, (float)0.25);
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
                    board.child.AddChild(field[i]);
                    field[i].Position = position[i];
                    break;
                }
            }
        }
        public static void resetVisualGame()
        {
            VisualMethods.selecting = false;
            SelectCards.selectCards = null;
            VisualMethods.SelectedCards = new List<Relics>();

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
        public static void Remove(Relics[] Battlefield, Relics relic)
        {
            foreach (var effect in relic.Actions)
            {
                if(effect is Attack)
                {
                    int temp = TotalActive(mainMenu.Inventory.CardsInventory[relic.id], relic);
                    effect.Affected.character.attack -= (((Attack)effect).damage * ((Attack)effect).factor) * temp;
                }
                else if(effect is ChangeState)
                {
                    effect.Affected.state = State.Safe;
                }
            }
            board.Game.GraveYard.Add(relic);
            for (int i = 0; i < Battlefield.Length; i++)
            {
                if(Battlefield[i] != null && Battlefield[i] == relic)
                    Battlefield[i] = null;
            }
        }
        static int TotalActive(Relics DefaultRelic, Relics relic)
        {
            int temp1 = 1;
            if(DefaultRelic.passiveDuration != 0)
                if(DefaultRelic.activeDuration != 1)
                    {
                        temp1 += ((((DefaultRelic.activeDuration * 2) - 1) - relic.activeDuration) / (DefaultRelic.passiveDuration * 2));
                        if(temp1 == 0)
                            temp1 = 1;
                    }
            return temp1;
        }

        /// <summary>
        /// Removes sprites and reset source to an empty list
        /// </summary>
        /// <param name="source"></param>
        static void RemoveSprites(List<Sprite> source)
        {
            foreach (var sprite in source)
            {
                sprite.QueueFree();
            }
            source = new List<Sprite>();
        }

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
                    if(board.Game.turn % 2 == 0) // Player 1 is clicking
                    {
                        if (!(board.Game.player1 is VirtualPlayer))
                        {
                            for(int i = 0; i < board.VisualBoard.visualHand1.visualHand.Count; i++)
                            {
                                if  (board.VisualBoard.visualHand1.visualHand[i].GetRect().HasPoint(board.VisualBoard.visualHand1.visualHand[i].ToLocal(mouseEvent.Position)))
                                {
                                    Effect(board.Game.player1.hand[i], false);
                                }
                            }
                        }
                    }
                    else // Player 2 is clicking
                    {
                        if (!(board.Game.player2 is VirtualPlayer))
                        {
                            if(!FullBattlefield(board.VisualBoard.visualBattleField2))
                            {
                                for(int i = 0; i < board.VisualBoard.visualHand2.visualHand.Count; i++)
                                {
                                    if  (board.VisualBoard.visualHand2.visualHand[i].GetRect().HasPoint(board.VisualBoard.visualHand2.visualHand[i].ToLocal(mouseEvent.Position)))
                                    {
                                        Effect(board.Game.player2.hand[i], false);
                                    }
                                }
                            }
                        }
                    }
                if(board.VisualBoard.visualGraveYard.GraveYardCard.GetRect().HasPoint(board.VisualBoard.visualGraveYard.GraveYardCard.ToLocal(mouseEvent.Position)))
                {
                    if(board.VisualBoard.visualGraveYard.graveYard.Count>0)
                    {
                        selectVisually("GraveYard: ", board.VisualBoard.visualGraveYard.graveYard, 0, (x, y)=>{}, new List<Relics>());
                    }
                }
                    break;
            }
        }     
        public static bool FullBattlefield(Board.VisualBattleField battleField)
        {
            foreach (var relic in battleField.BattleField)
            {
                if(relic == null) return false;
            }
            return true;
        }
        public static void Effect(Relics relic, bool ItsAlreadyActive)
        {
            InterpretEffect effect = new InterpretEffect();
            effect.Scan(relic);
            if(effect.Active)
            {
                board.Game.player2.AddtoBattleField(relic);
                AddtoVisualBattleField(relic.Owner, relic);
                board.VisualBoard.Update();
                relic.Effect(effect);
                board.VisualBoard.Update();
            }
        }
        public static void ActiveEscapeMenu()
        {
                PackedScene EscMenu = (PackedScene)GD.Load("res://PauseMenu.tscn");
                Node PauseMenu = (Node)(EscMenu.Instance());
                boardNode.AddChild(PauseMenu);
                boardNode.GetTree().Paused = true; 
        }

        #endregion
    }
}