using System;
using System.Collections.Generic;
namespace card_gameEngine
{
    //PassiveDuration: Cada cuanto tiempo se activa el efecto
    //ActiveDuration: Cuanto tiempo dura el efecto
    public class Condition
    {
        public bool condition;

        public Condition()
        {
            this.condition = true;
        }
        public Condition(int a, int b, int operId)
        {
            switch (operId)
            {
                case 1: // = operator
                    if (a == b)
                    {
                        this.condition = true;
                    }
                    break; 

                case 2: // < operator
                    if (a < b)
                    {
                        this.condition = true;
                    }
                    break; 

                case 3: // > operator
                    if (a > b)
                    {
                        this.condition = true;
                    }
                    break;

                case 4: // <= operator
                    if (a <= b)
                    {
                        this.condition = true;
                    }
                    break; 

                case 5: // >= operator
                    if (a >= b)
                    {
                        this.condition = true;
                    }
                    break;  

            }
        }
        public Condition(State a, State b)
        {
            if (a == b)
            {
                this.condition = true;
            }
        }
        public Condition(CardState a, CardState b)
        {
            if (a == b)
            {
                this.condition = true;
            }
        }


    }
    public class Cards : Actions
    {
        public string name = "";
        public int activeDuration;
        public int passiveDuration;
        public string imgAddress = "";
        Condition condition;

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
        public Dictionary<int, ActionInfo> EffectsOrder = new Dictionary<int, ActionInfo>();
        public Condition Condition;
        public bool isTrap;
        public CardState cardState = CardState.OnDeck;

        public Relics(Player Owner, Player Enemy, int id, string Name, int passiveDuration, int activeDuration, string imgAddress, bool isTrap, Condition condition, Dictionary<int, ActionInfo> EffectsOrder) 
                      : base (Name, passiveDuration, activeDuration, imgAddress)
        {
            this.Owner = Owner;
            this.Enemy = Enemy;
            this.id = id;
            this.EffectsOrder = EffectsOrder;
            this.Condition = condition;
            this.cardState = CardState.OnHand;
            this.isTrap = isTrap;
        }

        public static Player SetPlayer(Player Owner, Player Enemy, relativePlayer relativePlayer)
        {
            Player player;
            if (relativePlayer == relativePlayer.Owner)
            {
                player = Owner;
            }
            else
            {
                player = Enemy;
            }
            return player;
        }
        public static Player SetEnemy(Player Owner)
        {
            Player Enemy;

            foreach (var player in board.PlayersInventary)
            {
                if (player != Owner)
                {
                    Enemy = player;
                    return Enemy;
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
                
        public void Effect()
        {
            if (Condition.condition)
            {
                foreach(var Effect in EffectsOrder)
                {
                    
                    Player player = SetPlayer(board.PlayersInventary[0], board.PlayersInventary[1], Effect.Value.relativePlayer);
                    Player enemy = SetEnemy(player);
                    double actualFactor;

                    switch (Effect.Key)
                    {
                        case 1:
                            TakeFromDeck(player, enemy, Effect.Value.affects, Effect.Value.affectedIds);
                            break;

                        case 2:
                            TakeFromEnemyHand(player, enemy, Effect.Value.affects);
                            break;

                        case 3:
                            TakeFromGraveyard(player, enemy, Effect.Value.affects, Effect.Value.affectedIds);
                            break;

                        case 4:
                            actualFactor = setFactor(Effect.Key, player, enemy);
                            Life(player, enemy, Effect.Value.affects, actualFactor);
                            break;

                        case 5:
                            actualFactor = setFactor(Effect.Key, player, enemy);
                            Attack(player, enemy, Effect.Value.affects,  actualFactor);
                            break;

                        case 6:
                            actualFactor = setFactor(Effect.Key, player, enemy);
                            Defense(player, enemy, Effect.Value.affects, actualFactor);
                            break;
                        
                        case 7:
                            actualFactor = setFactor(Effect.Key, player, enemy);
                            Discard(player, enemy, Effect.Value.affects, actualFactor, Effect.Value.affectedIds);
                            break;

                        case 8:
                            ChangeState(player, enemy, Effect.Value.state);
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
            foreach (var card in hand)
            {
                if(card.cardState == CardState.OnHand)
                {
                    Console.WriteLine("Id: "+ card.id + " Name: "+ card.name);            
                }
            }
        }
        public void PrintBattleField()
        {
            Console.WriteLine();
            int quant = 0;
            foreach (var card in this.hand)
            {
                if (card.cardState == CardState.Activated)
                {
                    quant++;
                    Console.Write(card.name+", ");
                }
            }
            Console.WriteLine();
            Console.WriteLine("BatteField-"+this.name+": "+quant);
            Console.WriteLine();

        }
        public void PrintGraveYard()
        {
            int quant = 0;
            foreach (var card in this.hand)
            {
                if (card.cardState == CardState.OnGraveyard)
                {
                    quant++;
                    Console.Write(card.name+", ");
                }
                
            }
            Console.WriteLine();
            Console.WriteLine("Graveyard-"+this.name+": "+quant);

        }
        public int getCardType(CardState cardState)
        {
            int total = 0;
            foreach (var card in this.hand)
            {
                if (card.cardState == cardState)
                {
                    total++;
                }
            }
            return total;
        }
    }

    #region auxiliar classes
    public class FullList
    {
        public List<Relics> affectedIds = new List<Relics>();
        public FullList(List<Relics> Place, bool isTrap)
        {
            foreach (var card in Place)
            {
                if(isTrap)
                {
                    if (board.CardsInventary[card.id].isTrap)
                    {
                        this.affectedIds.Add(card);
                    }
                }
                if (!isTrap)
                {
                    if (!board.CardsInventary[card.id].isTrap)
                    {
                        this.affectedIds.Add(card);
                    }
                }
            }
        }

        public FullList(List<Relics> Place, Dictionary<int, ActionInfo> EffectsOrder, int ActionId, Player affectedPlayer, string condicion)
        {
            char oper = condicion[0];
            string conditionClean = condicion.Remove(0,1);
            if(oper == '+')
            {
                foreach (var card in Place)
                {

                    if (card.EffectsOrder[ActionId].relativePlayer == relativePlayer.Owner)
                    {
                            if (card.Owner == affectedPlayer && 
                                card.EffectsOrder[ActionId].affects >= int.Parse(conditionClean))
                            {
                                this.affectedIds.Add(card);
                            }
                    }
                    else
                    {
                        Player Enemy;
                        foreach (var player in board.PlayersInventary)
                        {
                            if (player != card.Owner)
                            {
                                Enemy = player;

                                if (Enemy == affectedPlayer && 
                                card.EffectsOrder[ActionId].affects >= int.Parse(conditionClean))
                                {
                                    this.affectedIds.Add(card);
                                }
                                break;
                            }
                        }
                        
                    }
                }
            }
            if(oper == '-')
            {
                foreach (var card in Place)
                {

                    if (card.EffectsOrder[ActionId].relativePlayer == relativePlayer.Owner)
                    {
                            if (card.Owner == affectedPlayer && 
                                card.EffectsOrder[ActionId].affects <= int.Parse(conditionClean))
                            {
                                this.affectedIds.Add(card);
                            }
                    }
                    else
                    {
                        Player Enemy;
                        foreach (var player in board.PlayersInventary)
                        {
                            if (player != card.Owner)
                            {
                                Enemy = player;

                                if (Enemy == affectedPlayer && 
                                card.EffectsOrder[ActionId].affects <= int.Parse(conditionClean))
                                {
                                    this.affectedIds.Add(card);
                                }
                                break;
                            }
                        }
                        
                    }
                }
            }
            if(oper == '=')
            {
                foreach (var card in Place)
                {

                    if (card.EffectsOrder[ActionId].relativePlayer == relativePlayer.Owner)
                    {
                            if (card.Owner == affectedPlayer && 
                                card.EffectsOrder[ActionId].affects == int.Parse(conditionClean))
                            {
                                this.affectedIds.Add(card);
                            }
                    }
                    else
                    {
                        Player Enemy;
                        foreach (var player in board.PlayersInventary)
                        {
                            if (player != card.Owner)
                            {
                                Enemy = player;

                                if (Enemy == affectedPlayer && 
                                card.EffectsOrder[ActionId].affects == int.Parse(conditionClean))
                                {
                                    this.affectedIds.Add(card);
                                }
                                break;
                            }
                        }
                        
                    }
                }
            }
        }
    }
    public class ActionInfo
    {
        public relativePlayer relativePlayer;
        public State state;
        public double factor = 1;
        public int affects;
        public List<int> affectedIds = new List<int>();
        public relativeFactor relativeFactor = relativeFactor.Fixed;

        public ActionInfo(int affects, List<int> affectedIds)
        {
            this.affects = affects;
            this.affectedIds = affectedIds;
        }
        public ActionInfo(relativePlayer relativePlayer, int affects)
        {
            this.relativePlayer = relativePlayer;
            this.affects = affects;
        }

        public ActionInfo(relativePlayer relativePlayer, int affects, List<int> affectedIds)
        {
            this.relativePlayer = relativePlayer;
            this.affects = affects;
            this.affectedIds = affectedIds;
        }

        public ActionInfo(relativePlayer relativePlayer, int affects, double factor, relativeFactor relativeFactor)
        {
            this.relativeFactor = relativeFactor;
            this.relativePlayer = relativePlayer;
            this.affects = affects;
            this.factor = factor;
        }

        public ActionInfo(relativePlayer relativePlayer, int affects, List<int> affectedIds, double factor, relativeFactor relativeFactor)
        {
            this.relativeFactor = relativeFactor;
            this.relativePlayer = relativePlayer;
            this.affects = affects;
            this.affectedIds = affectedIds;
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
        Enemy
    }
    
    #endregion
}