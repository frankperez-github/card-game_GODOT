using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
namespace gameEngine
{
    //PassiveDuration: Cada cuanto tiempo se activa el efecto
    //ActiveDuration: Cuanto tiempo dura el efecto
    
    public class Cards : Actions
    {
        public string name = "";
        public int activeDuration;
        public int passiveDuration;
        public string imgAddress = "";

        public Cards(string name, int passiveDuration, int activeDuration, string imgAddress)
        {
            this.name = name;
            this.imgAddress = imgAddress;
            this.passiveDuration = passiveDuration;
            this.activeDuration = activeDuration;
        }
    } 
    public class Relics : Cards
    {
        public int id;
        public Player Owner;
        public Player Enemy;
        public Player Affected;
        public Player notAffected;
        public string condition = "";
        public string type = ""; 
        public Dictionary<int, ActionInfo> EffectsOrder = new Dictionary<int, ActionInfo>();
        public bool isTrap;
        public CardState cardState = CardState.OnDeck;

        public Relics(Player Owner, Player Enemy, int id, string Name, int passiveDuration, int activeDuration, string imgAddress, bool isTrap, string condition, string type, Dictionary<int, ActionInfo> EffectsOrder) 
                      : base (Name, passiveDuration, activeDuration, imgAddress)
        {
            this.Owner = Owner;
            this.Enemy = Enemy;
            this.id = id;
            this.EffectsOrder = EffectsOrder;
            this.cardState = CardState.OnHand;
            this.isTrap = isTrap;
            this.type= type;
            this.condition = condition;
        }

        public void SetPlayer(Player Owner, Player Enemy, relativePlayer relativePlayer)
        {
            if (relativePlayer == relativePlayer.Owner)
            {
                this.Affected = Owner;
            }
            else
            {
                this.Affected = Enemy;
            }
        }
        public void SetEnemy()
        {
            foreach (var player in Settings.PlayersInventary)
            {
                if (player != this.Affected)
                {
                    this.notAffected = player;
                    return;
                }
            }
            throw(new Exception("Error in Actions.SetEnemy()"));
        }
    
        public double setFactor(int effect, Player player, Player enemy)
        {
            double factor = 1;
            switch (this.EffectsOrder[effect].relativeFactor)
            {
                case relativeFactor.EnemyHand:
                    return  this.EffectsOrder[effect].factor * enemy.getCardType(CardState.OnHand);

                case relativeFactor.OwnerHand:
                    return this.EffectsOrder[effect].factor * player.getCardType(CardState.OnHand);

                case relativeFactor.EnemyBattleField:
                    return this.EffectsOrder[effect].factor * enemy.getCardType(CardState.Activated);

                case relativeFactor.OwnerBattleField:
                    return this.EffectsOrder[effect].factor * player.getCardType(CardState.Activated);
                
                case relativeFactor.Graveyard:
                    return this.EffectsOrder[effect].factor * (player.getCardType(CardState.OnGraveyard) + enemy.getCardType(CardState.OnGraveyard));
                
                default:
                    factor = this.EffectsOrder[effect].factor;
                    break;
            }
            return factor;
        } 
        public  void AddtoBattleField()
        {
            for (int i = 0; i < Owner.userBattleField.Length; i++)
            {
                if(Owner.userBattleField[i] == null)
                {
                    this.Owner.userBattleField[i] = this;
                    this.Owner.hand.Remove(this);
                    break;
                }
            }
            
        }       
        public void AddtoVisualBattleField()
        {
            for (int i = 0; i < Owner.userVisualBattleField.Length; i++)
            {
                if(Owner.userVisualBattleField[i] == null)
                {
                    this.Owner.userVisualBattleField[i] = gameVisual.Visual.InstanciateVisualCard(this);
                    break;
                }
            }
            
        }  
        public void Effect()
        {
            if (condition == "" || new BoolEx(this.condition, Owner, Enemy).ScanExpression())
            {
                foreach(var Effect in EffectsOrder)
                {
                    
                    this.cardState = CardState.Activated;
                    if (Effect.Value.affects >= 0)
                    {
                        AddtoBattleField();
                        AddtoVisualBattleField();
                    }
                    SetPlayer(Owner, Enemy, Effect.Value.relativePlayer);
                    SetEnemy();
                    List<Relics> affectedCards = new InterpreterList().FullList(Effect.Value.CardList, this.Affected);
                    double actualFactor;

                    switch (Effect.Key)
                    {
                        case 1:
                            TakeFromDeck(this.Affected, this.notAffected, Effect.Value.affects, affectedCards);
                            break;

                        case 2:
                            TakeFromEnemyHand(this.Affected, this.notAffected, Effect.Value.affects);
                            break;

                        case 3:
                            TakeFromGraveyard(this.Affected, this.notAffected, Effect.Value.affects, affectedCards);
                            break;

                        case 4:
                            actualFactor = setFactor(Effect.Key, this.Affected, this.notAffected);
                            Life(this.Affected, Effect.Value.affects, actualFactor);
                            break;

                        case 5:
                            actualFactor = setFactor(Effect.Key, this.Affected, this.notAffected);
                            Attack(this.Affected, Effect.Value.affects,  actualFactor);
                            break;

                        case 6:
                            actualFactor = setFactor(Effect.Key, this.Affected, this.notAffected);
                            Defense(this.Affected, Effect.Value.affects, actualFactor);
                            break;
                        
                        case 7:
                            actualFactor = setFactor(Effect.Key, this.Affected, this.notAffected);
                            Discard(this.Affected, Effect.Value.affects, actualFactor, affectedCards);
                            break;

                        case 8:
                            ChangePlayerState(this.Affected, Effect.Value.state);
                            break;
                        case 9:
                            RemoveFromBattleField(this.Affected, affectedCards);
                            break;
                    }
                }

            }
        }
    }
    public class Character : Cards
    {
        public double attack;
        public double defense;

        /// <returns>Construye un personaje</returns>
        public Character(string name, int passiveDuration, int activeDuration, string imgAddress, double attack, double defense) : base(name, passiveDuration, activeDuration, imgAddress)
        {
            this.attack = attack;
            this.defense = defense;
        }


        public static void SpecialAttack(Player player, double attack, int defense, int life, State state)
        {
            player.attack += attack;
            player.defense += defense;
            player.state = state;
            player.life = life;
        }
    }
    public class Player : Character 
    {
        Player Enemy;
        public string nick = "";
        public double life;
        public List<Relics> hand = new List<Relics>();
        public Relics[] userBattleField = new Relics[4];
        public Sprite[] userVisualBattleField = new Sprite[4];

        public State state = State.Safe;

        public Player(Character character, string nick): base(character.name, character.passiveDuration, character.activeDuration, character.imgAddress, character.attack, character.defense) 
        {
            this.nick = nick;
            this.life = 100;
        }

        public void printInfo()
        {
            Console.WriteLine("Nick: " + this.nick);
            Console.WriteLine("Life: " + this.life);
            Console.WriteLine("Attack: " + this.attack);
            Console.WriteLine("Defense: " + this.defense);
            Console.WriteLine("State: " + this.state);
            this.PrintHand();
            this.PrintBattleField();
            this.PrintGraveYard();
        }
        public void PrintHand()
        {
            Console.WriteLine();
            Console.WriteLine("Hand: ");
            int index = 0;
            foreach (var card in hand)
            {
                Console.WriteLine("CardPossition: "+ index + " Id: "+ card.id + " Name: "+ card.name);            
                index++;
            }
        }
        public void PrintBattleField()
        {
            Console.WriteLine();
            for (int i = 0; i < this.userBattleField.Length; i++)
            {
                if(this.userBattleField[i] != null)
                {
                    Console.Write(userBattleField[i].name+", ");
                }
            }
            Console.WriteLine();
            Console.WriteLine("BatteField-"+this.name+": "+ this.userBattleField.Length);
            Console.WriteLine();

        }
        public void PrintGraveYard()
        {
            foreach (var card in Settings.GraveYard)
            {
                Console.Write(card.name+", ");
            }
            Console.WriteLine();
            Console.WriteLine("Graveyard-"+this.name+": "+ Settings.GraveYard.Count());

        }
        public int getCardType(CardState cardState)
        {
            switch (cardState)
            {
                case CardState.OnHand:
                    return this.hand.Count();
                case CardState.Activated:
                    return this.userBattleField.Length;
                case CardState.OnGraveyard:
                    return Settings.GraveYard.Count();
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
    
    public class ActionInfo
    {
        public relativePlayer relativePlayer;
        public State state;
        public double factor = 1;
        public double affects;
        public string CardList = "deck"; 
        public relativeFactor relativeFactor = relativeFactor.Fixed;

        public ActionInfo(int affects, string CardCondition)
        {
            this.affects = affects;
            this.CardList = CardCondition;
        }
        public ActionInfo(relativePlayer relativePlayer, int affects)
        {
            this.relativePlayer = relativePlayer;
            this.affects = affects;
        }

        public ActionInfo(relativePlayer relativePlayer, int affects, string CardCondition)
        {
            this.relativePlayer = relativePlayer;
            this.affects = affects;
            this.CardList = CardCondition;
        }

        public ActionInfo(relativePlayer relativePlayer, int affects, double factor, relativeFactor relativeFactor)
        {
            this.relativeFactor = relativeFactor;
            this.relativePlayer = relativePlayer;
            this.affects = affects;
            this.factor = factor;
        }

        public ActionInfo(relativePlayer relativePlayer, int affects, string CardCondition, double factor, relativeFactor relativeFactor)
        {
            this.relativeFactor = relativeFactor;
            this.relativePlayer = relativePlayer;
            this.affects = affects;
            this.CardList = CardCondition;
            this.factor = factor;
        }

        public ActionInfo(relativePlayer relativePlayer, State state)
        {
            this.relativePlayer = relativePlayer;
            this.state = state;
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
    public enum relativeFactor
    {
        Fixed,
        EnemyHand,
        OwnerHand,
        EnemyBattleField,
        OwnerBattleField,
        Graveyard
    }
    public enum relativePlayer
    {
        Owner,
        Enemy,
        NULL
    }
    public enum Property
    {
        State,
        Life,
        Defense,
        Attack,
        Hand,
        BatteField,
        GraveYard
    }
    
    #endregion
}