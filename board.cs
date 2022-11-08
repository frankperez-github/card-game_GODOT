using Godot;
using System.Collections.Generic;
namespace card_gameEngine
{
    public class board : Godot.Node2D
    {   


        const int maxinHand = 6;
        static Sprite Relic = new Sprite();

        public void AddToHand(Player player)
        {


            PackedScene relic = (PackedScene)GD.Load("res://Relic.tscn");
            
            Vector2 PlayerHandPosition = new Vector2(175 - (player.hand.Count * 10), 532);

            int index = 1;
            foreach (var card in player.hand)
            {
                // Max Cards in hand is 6
                if (index <= maxinHand)
                {
                    Relic = InstanciateRelic();
                    Relic.Position = new Vector2(PlayerHandPosition.x + 115*index, PlayerHandPosition.y);
                    AddChild(Relic);
                    Label name = (Label)Relic.GetChild(1);
                    name.Text = card.name;
                    index++; 
                }
            }
        }
        public void AddToEnemyHand(Player player)
        {
            
            Vector2 PlayerHandPosition = new Vector2(175 - (player.hand.Count * 10), 12);

            int index = 1;
            foreach (var card in player.hand)
            {
                // Max Cards in hand is 6
                if (index <= maxinHand)
                {
                    Relic = InstanciateRelic();
                    Relic.Position = new Vector2(PlayerHandPosition.x + 115*index, PlayerHandPosition.y);
                    AddChild(Relic);
                    Label name = (Label)Relic.GetChild(1);
                    Label nickPlay = (Label)Relic.GetChild(1);
                    name.Text = card.name;
                    nickPlay.Text = player.nick;
                    index++; 
                }
            }
        }
        public void CheckAndDiscard(Player player)
        {
            PackedScene DiscardScene = (PackedScene)GD.Load("res://DiscardLabel.tscn");

            if (player.hand.Count > 6)
            {
                Tree DiscardTscn = (Tree)DiscardScene.Instance();
                Label label = DiscardTscn.GetNode<Label>("DiscardLabel");
                label.Text = "Select "+(player.hand.Count - 6).ToString()+" to discard:";
                AddChild(DiscardTscn);
            }
        }
        public static Sprite InstanciateRelic()
        {
            PackedScene relic = (PackedScene)GD.Load("res://Relic.tscn");
            Relic = (Sprite)relic.Instance();
            return Relic;
        }
    
    }       
}

