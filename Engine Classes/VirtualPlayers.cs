using System.Collections.Generic;
using Godot;
using gameVisual;
namespace gameEngine
{
    public abstract class VirtualPlayer : Player
    {
        public VirtualPlayer(string nick) : base(nick)
        {

        }
        public abstract void Play();
    }

    public class RandomVirtPlayer : VirtualPlayer
    {
        public RandomVirtPlayer(string nick) : base(nick){}

        public override void Play()
        {
            System.Random rnd = new System.Random();
            int random = rnd.Next(1, hand.Count-1);
            VisualMethods.Effect(this.hand[random]);
        }
    }
}