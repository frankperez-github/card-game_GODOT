using System.Collections.Generic;
using Godot;
namespace gameEngine
{
    public class VirtualPlayer : Player
    {
        public VirtualPlayer(string nick) : base(nick)
        {

        }
    }

    public class RandomVirtPlayer : VirtualPlayer
    {
        public RandomVirtPlayer(string nick) : base(nick){}

        public void Play()
        {
            System.Random rnd = new System.Random();
            int random = rnd.Next(1, hand.Count-1);
            this.hand[random].Effect();
        }
    }
}