using System.Collections.Generic;
using Godot;
using System;
using gameVisual;
namespace gameEngine
{
    public abstract class VirtualPlayer : Player
    {
        public VirtualPlayer(string nick) : base(nick)
        {

        }
        public abstract void Play();
        public abstract void Discard(int count);
        public abstract List<Relics> FullList(List<Relics> Place, int count);
    }

    public class RandomVirtPlayer : VirtualPlayer
    {
        public RandomVirtPlayer(string nick) : base(nick){}

        public override void Play() //////////////////AGREGAR QUE ACTIVE UN NUMERO RANDOM DE CARTAS RANDOM
        {
            System.Random rnd = new System.Random();
            if(this.hand.Count > 1)
            {
                int random = rnd.Next(1, hand.Count-1);
                VisualMethods.Effect(this.hand[random]);
            }
            else if(this.hand.Count == 1)
            {
                VisualMethods.Effect(this.hand[0]);
            }
        }
        public override void Discard(int count)
        {
            for (var i = 0; i < count; i++)
            {
                Random rnd = new Random();
                int card = rnd.Next(0, hand.Count-1);
                board.Game.GraveYard.Add(hand[card]);
                hand.RemoveAt(card);
            }
            board.VisualBoard.UpdateVisualHand(board.VisualBoard.visualHand2);
            board.VisualBoard.UpdateVisualHand(board.VisualBoard.visualHand1);
            VisualMethods.UpdatePlayersVisualProperties();
            board.VisualBoard.visualGraveYard.Show();
        }

        public override List<Relics> FullList(List<Relics> Place, int count)
        {
            throw new NotImplementedException();
        }
    }

    public class OffensiveVirtPlayer : VirtualPlayer
    {
        public OffensiveVirtPlayer(string nick) : base(nick){}

        public override void Play()
        {
            // Counting possible moves
            int possible = 4;
            foreach (var item in this.BattleField) 
            {
                if (item != null)
                {
                    possible--;
                }
            }
            Random rnd = new Random();
            int moves = rnd.Next(1, possible);

            // Selecting Cards by attack points
            List<Relics> cardsToPlay = new List<Relics>();
            foreach (var item in hand)
            {
                // Checking if it can activate more cards
                if(possible > 0)
                {
                    // Positive and on it's favor attack
                    if(item.effect.Contains("Owner") && item.effect.Contains("Attack"))
                    {
                        // Getting Attack value
                        string[] words = item.effect.Split('.', ' ', ')');
                        for (int i = 0; i < words.Length; i++)
                        {
                            if(words[i] == "Attack" )
                            {
                                
                                // If condition of card is true applying effect (don't waste a chance)
                                InterpretEffect effect = new InterpretEffect();
                                effect.Scan(item);
                                if (effect.Active) 
                                {
                                    cardsToPlay.Add(item);
                                }
                                break;
                            }    
                        }
                    }
                    possible--;
                }
            }

            //Playing all selected cards
            foreach (var item in cardsToPlay)
            {
                VisualMethods.Effect(item);
            }
        }
        public override void Discard(int count)
        {
            for (var i = 0; i < count; i++)
            {
                Random rnd = new Random();
                int card = rnd.Next(0, hand.Count-1);
                board.Game.GraveYard.Add(hand[card]);
                hand.RemoveAt(card);
            }
            board.VisualBoard.UpdateVisualHand(board.VisualBoard.visualHand2);
            board.VisualBoard.UpdateVisualHand(board.VisualBoard.visualHand1);
            VisualMethods.UpdatePlayersVisualProperties();
            board.VisualBoard.visualGraveYard.Show();
        }

        public override List<Relics> FullList(List<Relics> Place, int count)
        {
            throw new NotImplementedException();
        }
    }
}