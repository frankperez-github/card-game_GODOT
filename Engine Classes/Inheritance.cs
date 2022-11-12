using System;
using System.Collections.Generic;
namespace card_gameEngine
{
    //PassiveDuration: Cada cuanto tiempo se activa el efecto
    //ActiveDuration: Cuanto tiempo dura el efecto
    
    public abstract class Expression
    {
        public abstract bool Evaluate();
    }

    public abstract class BinaryExpression : Expression
    {
        protected readonly Expression left;
        protected readonly Expression right;

        public BinaryExpression(Expression left, Expression right)
        {
            this.left = left;
            this.right = right;
        }

        public override bool Evaluate()
        {
            bool leftValue = this.left.Evaluate();
            bool rightValue = this.right.Evaluate();

            return this.Evaluate(leftValue, rightValue);
        }

        protected abstract bool Evaluate(bool left, bool right);
    }

    public class And : BinaryExpression
    {
        public And(Expression left, Expression right) : base(left, right)
        {

        }

        protected override bool Evaluate(bool left, bool right)
        {
            return (left && right);
        }
    }

    public class Or : BinaryExpression
    {
        public Or(Expression left, Expression right) : base(left, right)
        {

        }

        protected override bool Evaluate(bool left, bool right)
        {
            return (left || right);
        }
    }
    public class Condition
    {
        
        public bool condition;
        relativePlayer relativePlayer;
        relativePlayer relativePlayer1;
        Player player;
        Property property;
        int b = 0;
        int OperId;
        State state;

        public Condition(){
            this.condition = true;
        }

        public Condition(relativePlayer relativePlayer, Property property, int b, int operId)
        {
            this.relativePlayer = relativePlayer;
            this.property = property;
            this.b = b;
            this.OperId = operId;
        }
        
        public Condition (relativePlayer relativePlayer, State state)
        {
            this.relativePlayer = relativePlayer;
            this.state = state;
        }
        public Condition(relativePlayer relativePlayer, Property property, relativePlayer relativePlayer1, int operId)
        {
            this.relativePlayer = relativePlayer;
            this.property = property;
            this.relativePlayer1 = relativePlayer1;
            this.OperId = operId;
        }
        
        public bool Evaluate(Player Owner, Player Enemy)
        {
            // Comparing a player's property with a number
            if (this.b != 0)
            {
                Player affected = Relics.SetPlayer(Owner, Enemy, this.relativePlayer);
                switch (this.property)
                {
                    case Property.Attack:
                        switch (this.OperId)
                        {
                            case 1:
                                return affected.attack >= this.b;
                            case 2:
                                return affected.attack <= this.b;
                            case 3:
                                return affected.attack == this.b;
                        }
                        break;

                    case Property.BatteField:
                        switch (this.OperId)
                        {
                            case 1:
                                return affected.userBattleField.Count >= this.b;
                            case 2:
                                return affected.userBattleField.Count <= this.b;
                            case 3:
                                return affected.userBattleField.Count == this.b;
                        }
                        break;

                    case Property.Defense:
                        switch (this.OperId)
                        {
                            case 1:
                                return affected.defense >= this.b;
                            case 2:
                                return affected.defense <= this.b;
                            case 3:
                                return affected.defense == this.b;
                        }
                        break;

                    case Property.GraveYard:
                        switch (this.OperId)
                        {
                            case 1:
                                return card_gameEngine.board.GraveYard.Count >= this.b;
                            case 2:
                                return card_gameEngine.board.GraveYard.Count <= this.b;
                            case 3:
                                return card_gameEngine.board.GraveYard.Count == this.b;
                        }
                        break;
                    
                    case Property.Hand:
                        switch (this.OperId)
                        {
                            case 1:
                                return affected.hand.Count >= this.b;
                            case 2:
                                return affected.hand.Count <= this.b;
                            case 3:
                                return affected.hand.Count == this.b;
                        }
                        break;
                    
                    case Property.Life:
                        switch (this.OperId)
                        {
                            case 1:
                                return affected.life >= this.b;
                            case 2:
                                return affected.life <= this.b;
                            case 3:
                                return affected.life == this.b;
                        }
                        break;

                }

            }
            // Comparing only states
            if (this.state != State.NULL)
            {
                Player affected = Relics.SetPlayer(Owner, Enemy, this.relativePlayer);
                return affected.state == this.state;
            }
            // Comparing a player's property with another player's same property
            if(relativePlayer != relativePlayer.NULL)
            {
                Player player = Relics.SetPlayer(Owner, Enemy, this.relativePlayer);
                Player player2 = Relics.SetPlayer(Owner, Enemy, this.relativePlayer1);
                
                switch (this.property)
                {
                    case Property.Attack:
                        switch (this.OperId)
                        {
                            case 1:
                                return player.attack >= player2.attack;
                            case 2:
                                return player.attack <= player2.attack;
                            case 3:
                                return player.attack == player2.attack;
                        }
                        break;

                    case Property.Defense:
                        switch (this.OperId)
                        {
                            case 1:
                                return player.defense >= player2.defense;
                            case 2:
                                return player.defense <= player2.defense;
                            case 3:
                                return player.defense == player2.defense;
                        }
                        break;

                    case Property.Life:
                        switch (this.OperId)
                        {
                            case 1:
                                return player.life >= player2.life;
                            case 2:
                                return player.life <= player2.life;
                            case 3:
                                return player.life == player2.life;
                        }
                        break;

                    case Property.BatteField:
                        switch (this.OperId)
                        {
                            case 1:
                                return player.userBattleField.Count >= player2.userBattleField.Count;
                            case 2:
                                return player.userBattleField.Count <= player2.userBattleField.Count;
                            case 3:
                                return player.userBattleField.Count == player2.userBattleField.Count;
                        }
                        break;
                    
                    case Property.Hand:
                        switch (this.OperId)
                        {
                            case 1:
                                return player.hand.Count >= player2.hand.Count;
                            case 2:
                                return player.hand.Count <= player2.hand.Count;
                            case 3:
                                return player.hand.Count == player2.hand.Count;
                        }
                        break;
                }
            }

            this.condition = false;
            return false;
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

            foreach (var player in card_gameEngine.board.PlayersInventary)
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
            if (this.Condition.Evaluate(Owner, Enemy))
            {
                foreach(var Effect in EffectsOrder)
                {
                    
                    this.cardState = CardState.Activated;
                    this.Owner.userBattleField.Add(this);
                    Player affectedPlayer = SetPlayer(Owner, Enemy, Effect.Value.relativePlayer);
                    Player notAffectedPlayer = SetEnemy(affectedPlayer);
                    List<Relics> affectedCards = new List<Relics>();
                    double actualFactor;

                    switch (Effect.Key)
                    {
                        case 1:
                            TakeFromDeck(affectedPlayer, notAffectedPlayer, Effect.Value.affects, affectedCards);
                            break;

                        case 2:
                            TakeFromEnemyHand(affectedPlayer, notAffectedPlayer, Effect.Value.affects);
                            break;

                        case 3:
                            TakeFromGraveyard(affectedPlayer, notAffectedPlayer, Effect.Value.affects, affectedCards);
                            break;

                        case 4:
                            actualFactor = setFactor(Effect.Key, affectedPlayer, notAffectedPlayer);
                            Life(affectedPlayer, Effect.Value.affects, actualFactor);
                            break;

                        case 5:
                            actualFactor = setFactor(Effect.Key, affectedPlayer, notAffectedPlayer);
                            Attack(affectedPlayer, Effect.Value.affects,  actualFactor);
                            break;

                        case 6:
                            actualFactor = setFactor(Effect.Key, affectedPlayer, notAffectedPlayer);
                            Defense(affectedPlayer, Effect.Value.affects, actualFactor);
                            break;
                        
                        case 7:
                            actualFactor = setFactor(Effect.Key, affectedPlayer, notAffectedPlayer);
                            Discard(affectedPlayer, Effect.Value.affects, actualFactor, affectedCards);
                            break;

                        case 8:
                            ChangePlayerState(affectedPlayer, Effect.Value.state);
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
        public List<Relics> userBattleField = new List<Relics>();
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
            foreach (var card in this.userBattleField)
            {
                Console.Write(card.name+", ");
            }
            Console.WriteLine();
            Console.WriteLine("BatteField-"+this.name+": "+ this.userBattleField.Count);
            Console.WriteLine();

        }
        public int getCardType(CardState cardState)
        {
            switch (cardState)
            {
                case CardState.OnHand:
                    return this.hand.Count;
                case CardState.Activated:
                    return this.userBattleField.Count;
                case CardState.OnGraveyard:
                    return card_gameEngine.board.GraveYard.Count;
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
    public class FullList
    {
        public List<Relics> affectedIds = new List<Relics>();
        public FullList(List<Relics> Place, bool isTrap)
        {
            foreach (var card in Place)
            {
                if(isTrap)
                {
                    if (card_gameEngine.board.CardsInventary[card.id].isTrap)
                    {
                        this.affectedIds.Add(card);
                    }
                }
                if (!isTrap)
                {
                    if (!card_gameEngine.board.CardsInventary[card.id].isTrap)
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
                        foreach (var player in card_gameEngine.board.PlayersInventary)
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
                        foreach (var player in card_gameEngine.board.PlayersInventary)
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
                        foreach (var player in card_gameEngine.board.PlayersInventary)
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
        public double affects;
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