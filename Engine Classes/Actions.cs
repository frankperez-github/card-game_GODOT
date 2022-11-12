using System;
using System.Collections.Generic;
namespace card_gameEngine
{   
    // Action extends Condition because we need Cards to extend Condition
    public class Actions
    {
        public void TakeFromDeck(Player Affected, Player Enemy, double cards, List<Relics> affectecards )
        {
            if (affectecards.Count == 0)
            {
                for (int i = 0; i < cards; i++)
                {
                    Random rnd = new Random();
                    int random = rnd.Next(1, card_gameEngine.board.CardsInventary.Count);
                    Relics relic = card_gameEngine.board.CardsInventary[random];
                    
                    Affected.hand.Add( new Relics(Affected, Enemy, relic.id, relic.name, relic.passiveDuration, relic.activeDuration, 
                                    relic.imgAddress, relic.isTrap, relic.Condition, relic.EffectsOrder));
                }
            }
            else
            {
                foreach (var card in affectecards)
                {
                    Relics relic = card_gameEngine.board.CardsInventary[card.id];
                    Affected.hand.Add( new Relics(Affected, Enemy, relic.id, relic.name, relic.passiveDuration, relic.activeDuration, 
                                    relic.imgAddress,relic.isTrap, relic.Condition, relic.EffectsOrder));
                }
            }
        }
        public void TakeFromEnemyHand(Player Affected, Player Enemy, double cards)
        {

            for (int i = 0; i < cards; i++)
            {
                if (Affected.hand.Count != 0)
                {
                    Random rnd = new Random();
                    int random = rnd.Next(0, Affected.hand.Count-1);
                    int cardId = Affected.hand[random].id;
                    Affected.hand.RemoveAt(random);
                    Relics relic = card_gameEngine.board.CardsInventary[random];
                    Enemy.hand.Add( new Relics(Affected, Enemy, relic.id, relic.name, relic.passiveDuration, relic.activeDuration, 
                                    relic.imgAddress,relic.isTrap, relic.Condition, relic.EffectsOrder));
                }
            }
        }
        public void TakeFromGraveyard(Player Affected, Player Enemy, double cards, List<Relics> affectecards)
        {
            if (affectecards.Count == 0)
            {
                for (int i = 0; i < cards; i++)
                {
                    try{
                        Random rnd = new Random();
                        int random = rnd.Next(0, card_gameEngine.board.GraveYard.Count);
                        int cardId = card_gameEngine.board.GraveYard[random];
                        Relics relic = card_gameEngine.board.CardsInventary[cardId];
                        Affected.hand.Add( new Relics(Affected, Enemy, relic.id, relic.name, relic.passiveDuration, relic.activeDuration, 
                                        relic.imgAddress,relic.isTrap, relic.Condition, relic.EffectsOrder));
                        card_gameEngine.board.GraveYard.RemoveAt(random);
                    }
                    catch(System.Exception)
                    {
                        Console.WriteLine("Intentaste añadir una carta que no esta ahi");
                    }
                }
            }
            else
            {
                foreach (var card in affectecards)
                {
                    foreach (var Relic in card_gameEngine.board.GraveYard)
                    {
                        if(Relic == card.id)
                        {
                            Relics relic = card_gameEngine.board.CardsInventary[card.id];
                            Affected.hand.Add( new Relics(Affected, Enemy, relic.id, relic.name, relic.passiveDuration, relic.activeDuration, 
                                            relic.imgAddress,relic.isTrap, relic.Condition, relic.EffectsOrder));
                            card_gameEngine.board.GraveYard.Remove(Relic);
                            break;
                        }
                    }
                }
            }
            
        }
        public void Life(Player Affected, double affects, double factor)
        {
            Affected.life += affects * factor;
        }
        public void Attack(Player Affected, double affects, double factor)
        {
            Affected.attack += affects * factor;
        }    
        public void Defense(Player Affected, double defense, double factor)
        {
            Affected.defense += defense*factor;
        }
        public void Discard(Player Affected, double affects, double factor, List<Relics> affectecards)
        {
            if (affectecards.Count == 0)
            {
                for (int i = 0; i < affects*factor; i++)
                {
                    if (Affected.hand.Count != 0)
                    {
                        Random rnd = new Random();
                        int randomPosition = rnd.Next(0, Affected.hand.Count-1);
                        Affected.hand.RemoveAt(randomPosition);
                    }
                }
            }
            else
            {
                foreach (var card in affectecards)
                {
                    try
                    {
                        Affected.hand.Remove(card_gameEngine.board.CardsInventary[card.id]);
                    }
                    catch(System.Exception)
                    {
                        Console.WriteLine("Intentaste descartar una carta que no esta ahi");
                    }
                }
            }
        }
        public void ChangePlayerState(Player Affected,  State state)
        {
            Affected.state = state;
        }
        public void RemoveFromBattleField(List<Relics> affectedcards)
        {
            foreach (var card in affectedcards)
            {
                card_gameEngine.board.GraveYard.Add(card_gameEngine.board.CardsInventary[card.id].id);
                card.Owner.userBattleField.Remove(card);
            }
        }

        public void EvitarDaño(Player Affected, double affects, double factor)
        {
            //  += affects * factor;
        }
    }
}