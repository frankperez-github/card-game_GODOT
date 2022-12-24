using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using gameVisual;
namespace gameEngine
{
    public class Relics
    {
        public int id {get; set;}
        public string name{get; set;}
        public int activeDuration{get;set;}
        public int passiveDuration{get;set;}
        public string imgAddress{get; set;}
        public Player Owner{get; set;}
        public Player Enemy{get; set;}
        public string type{get; set;} 
        public bool isTrap{get; set;}
        public CardState cardState = CardState.OnDeck;
        public string effect{get; set;}
        public string description{get; set;}
        public List<InterpretAction> Actions;

        public Relics(Player Owner, Player Enemy, int id, string name, int passiveDuration, int activeDuration, string imgAddress, bool isTrap, string type, string effect, string description)
        {
            this.id = id;
            this.name = name;
            this.imgAddress = imgAddress;
            this.passiveDuration = passiveDuration;
            if(activeDuration == 1) this.activeDuration = 1;
            else this.activeDuration = activeDuration * 2;
            this.Owner = Owner;
            this.Enemy = Enemy;
            this.cardState = CardState.OnHand;
            this.description = description;
            this.effect = effect;
            this.isTrap = isTrap;
            this.type= type;
            this.Actions = new List<InterpretAction>();
        }
        
        public void Effect(InterpretEffect effect)
        {
            effect.Scan(this);

            foreach (InterpretAction action in Actions)
            {
                action.Effect();
            }
        }
        
        }
    public class Character : Relics
    {
        public double attack;
        public double defense;

        /// <returns>Construye un personaje</returns>
        public Character(int id, string Name, int passiveDuration, int activeDuration, string imgAddress, string effect, string description, double attack, double defense, Player Owner, Player Enemy) : base(Owner, Enemy, id, Name, passiveDuration, activeDuration, imgAddress, false, "character", effect, description)
        {
            this.attack = attack;
            this.defense = defense;
        }
    }
    public class Player
    {
        public Player Enemy{get;set;}
        public string nick{get;set;}
        public double life{get; set;}
        public List<Relics> hand{get;}
        public Character character;
        public State state{get;set;}
        public Relics[] BattleField {get; set;}

        public Player(string nick)
        {
            this.nick = nick;
            this.life = 100;
            this.hand = new List<Relics>();
            this.state = State.Safe;
            this.BattleField = new Relics[4];
        }
        public void SetCharacter(CharacterProperties character)
        {
            this.character = new Character(character.id, character.name, character.passiveDuration, character.activeDuration, character.imgAddress, character.effect, character.description, character.attack, character.defense, this, this.Enemy);
        }
        public void TakeFromDeck(double cards)
        {
            for (int i = 0; i < cards; i++)
            {
                Random rnd = new Random();
                int random = rnd.Next(0, mainMenu.Inventory.CardsInventory.Count());
                Relics card = mainMenu.Inventory.CardsInventory[random];
                this.hand.Add( new Relics(this, this.Enemy, card.id, card.name, card.passiveDuration, card.activeDuration, 
                            card.imgAddress,card.isTrap, card.type, card.effect, card.description));
            }
        }
        public void AddtoBattleField(Relics relics)
        {
            for (int i = 0; i < relics.Owner.BattleField.Length; i++)
            {
                if(relics.Owner.BattleField[i] == null)
                {
                    relics.Owner.BattleField[i] = relics;
                    relics.Owner.hand.Remove(relics);
                    break;
                }
            }
        } 
        public int getCardType(CardState cardState)
        {
            switch (cardState)
            {
                case CardState.OnHand:
                    return this.hand.Count();
                case CardState.Activated:
                    return this.BattleField.Length;
                case CardState.OnGraveyard:
                    return board.Game.GraveYard.Count();
            }
            return 0;
        }
        public bool Trap()
        {
            foreach (var card in hand)
            {
                if(card.cardState == CardState.OnHand && card.isTrap)
                {
                    return true;
                }
            }
            return false;
        }
    }
    #region auxiliar classes
    public class CharacterProperties
    {
        public int id;
        public string name;
        public int passiveDuration;
        public int activeDuration;
        public string imgAddress;
        public string effect;
        public string description;
        public double attack;
        public double defense;

        public CharacterProperties(int id, string Name, int passiveDuration, int activeDuration, string imgAddress, string effect, string description, double attack, double defense)
        {
            this.id = id;
            this.name = Name;
            this.passiveDuration = passiveDuration;
            if(activeDuration == 1) this.activeDuration = 1;
            else this.activeDuration = activeDuration * 2;
            this.imgAddress = imgAddress;
            this.effect = effect;
            this.description = description;
            this.attack = attack;
            this.defense = defense;
        }
    }
    public enum CardState
    {
        OnDeck,
        Activated,
        OnHand,
        OnGraveyard
    }
    public enum State
    {
        Safe,
        Poisoned,
        Freezed,
        Asleep,
        NULL
    }
    #endregion
}