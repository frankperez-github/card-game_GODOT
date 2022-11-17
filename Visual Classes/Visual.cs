using Godot;
using System.Collections.Generic;
namespace card_gameVisual
{
    public class Visual
    {
          #region Visual Board elements
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

        static int Player1emptySlots = 4;
        static int Player2emptySlots = 4;

        static Sprite[] Player1Activated = new Sprite[4];
        static Sprite[] Player2Activated = new Sprite[4];

        #endregion

        public static void RefreshBoard()
        {
            card_gameEngine.Player player1 = card_gameEngine.board.PlayersInventary[0];
            card_gameEngine.Player player2 = card_gameEngine.board.PlayersInventary[1];

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
                CheckAndDiscard(player1);

            }

            int enemyIndex = 1;
            // Updating enemy's cards in board
            foreach (var card in player2.hand)
            {
                Relic = InstanciateVisualCard(card);
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

    }
}