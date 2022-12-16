using Godot;
using System.Collections.Generic;

namespace gameEngine
{
    public class Game
    {
        public Player player1{get; set;}
        public Player player2{get; set;}
        public int turn{get; set;}
        public List<Relics> GraveYard{get; set;}

        public Game(Player player1, Player player2)
        {
            this.player1 = player1;
            this.player2 = player2;
            player1.Enemy = player2;
            player2.Enemy = player1;
            turn = 1;
            GraveYard = new List<Relics>();
            player1.TakeFromDeck(5);
            player2.TakeFromDeck(5);
        }
        public void resetGame()
        {
            player1 = null;
            player2 = null;
            turn = 1;
            GraveYard = null;
        }
    }
}