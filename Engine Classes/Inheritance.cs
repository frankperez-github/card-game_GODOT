using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
namespace gameEngine
{
    public class Cards
    {
        public string name{get;}
        public int activeDuration{get;set;}
        public int passiveDuration{get;set;}
        public string imgAddress{get;}

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
        public int id{get;}
        public Player Owner{get;}
        public Player Enemy{get;}
        public string type{get;} 
        public string effect{get;}
        public bool isTrap{get;}
        public string description{get;}
        public List<InterpretAction> Actions;
        public CardState cardState = CardState.OnDeck;

        public Relics(Player Owner, Player Enemy, int id, string Name, int passiveDuration, int activeDuration, string imgAddress, bool isTrap, string type, string effect, string description)
                      : base (Name, passiveDuration, activeDuration, imgAddress)
        {
            this.Owner = Owner;
            this.Enemy = Enemy;
            this.id = id;
            this.cardState = CardState.OnHand;
            this.isTrap = isTrap;
            this.type= type;
            this.effect = effect;
            this.description = description;
            this.Actions = new List<InterpretAction>();
        }
        public void Effect()
        {
            AddtoBattleField();
            AddtoVisualBattleField();
            Scan(effect);
        }

        public void AddtoBattleField()
        {
            for (int i = 0; i < Owner.battleField.userBattleField.Length; i++)
            {
                if(Owner.battleField.userBattleField[i] == null)
                {
                    this.Owner.battleField.userBattleField[i] = this;
                    this.Owner.hand.Remove(this);
                    break;
                }
            }
        }       
        public void AddtoVisualBattleField()
        {
            for (int i = 0; i < Owner.battleField.userVisualBattleField.Length; i++)
            {
                if(Owner.battleField.userVisualBattleField[i] == null)
                {
                    this.Owner.battleField.userVisualBattleField[i] = gameVisual.board.InstanciateVisualCard(this);
                    break;
                }
            }
        } 
        public void Scan(string effect)
        {
            string[] expression = effect.Split('\n');
            Scan(expression, 0);
        }
        //Metodo Recursivo
        public void Scan(string[] expression, int index)
        {
            if(index==expression.Length)
            {
                return;
            }
            if (expression[index].Contains("if ("))
            {
                string condition = expression[index].Substring(expression[index].IndexOf("("), expression[index].Length -2 - expression[index].IndexOf("("));
                if (new BoolEx(condition, Owner, Enemy, this).Evaluate())
                {
                    Scan(expression, index+1);
                }
                else
                {
                    //Si la condicion es falsa revisará hasta encontrar la llave de cierre correspondiente al if
                    for (int i = index+1; i < expression.Length; i++)
                    {
                        int key = 0;
                        if(expression[i].Contains("{"))
                        {
                            key++;
                        }
                        else if(expression[i].Contains("}"))
                        {
                            key--;
                        }
                        if(key == 0)
                        {
                            if(expression[i].Contains("else"))
                            {
                                Scan(expression, i+1);
                                break;
                            }
                            if(expression[i].Contains("else if ("))
                            {
                                expression[i] = expression[i].Replace("else ", "");
                                Scan(expression, i);
                                break;
                            }
                        }
                    }
                }
            }
            else if (!expression[index].Contains("{") && !expression[index].Contains("}"))
            {
                InterpretActionExpression(expression[index]);
                Scan(expression, index+1);
            }    
            //Si no es un if ni una accion es una llave y nos la saltamos
            else Scan(expression, index+1); 
        }
        public void InterpretActionExpression(string action)
        {
            EditExpression Edit = new EditExpression();
            int start = action.IndexOf("(");
            int end = action.IndexOf(")");
            string actualAction = action.Substring(start, end - start);
            
            Player Affected = SetAffected(Edit.NextWord(actualAction));
            Player NotAffected = SetNotAffected(Affected);
            Action(actualAction, Affected, NotAffected);
        
        }
        public Player SetAffected(string player)
        {
            if (player == "Owner")
            {
                return this.Owner;
            }
            else
            {
                return this.Enemy;
            }
        }
        public Player SetNotAffected(Player Affected)
        {
            return Affected.Enemy;
        }
        public void Action(string expression, Player Affected, Player NotAffected)
        {
            EditExpression Edit = new EditExpression();
            expression = Edit.CutExpression(expression);
            switch (Edit.NextWord(expression))
            {
                case "Attack":
                    Attack Attack = new Attack(Edit.CutExpression(expression), this, Affected, NotAffected, this.Owner, this.Enemy);
                    Attack Negate = new Attack("-" + Edit.CutExpression(expression), this, Affected, NotAffected, this.Owner, this.Enemy);
                    this.Actions.Add(Negate);
                    Attack.Effect();
                    break;
                case "Cure":
                    Cure Cure = new Cure(Edit.CutExpression(expression), this, Affected, NotAffected, this.Owner, this.Enemy);
                    this.Actions.Add(Cure);
                    Cure.Effect();
                    break;
                case "Draw":
                    Draw Draw = new Draw(Edit.CutExpression(expression), this, Affected, NotAffected, this.Owner, this.Enemy);
                    this.Actions.Add(Draw);
                    Draw.Effect();
                    break;
                case "Remove":
                    Remove Remove = new Remove(Edit.CutExpression(expression), this, Affected, NotAffected, this.Owner, this.Enemy);
                    this.Actions.Add(Remove);
                    Remove.Effect();
                    break;
                case "Defense":
                    Defense Defense = new Defense(Edit.CutExpression(expression), this, Affected, NotAffected, this.Owner, this.Enemy);
                    this.Actions.Add(Defense);
                    Defense.Effect();
                    break;
                case "ChangeState":
                    ChangeState ChangeState = new ChangeState(Edit.CutExpression(expression), this, Affected, NotAffected, this.Owner, this.Enemy);
                    this.Actions.Add(ChangeState);
                    break;
                case "Show":
                    Show Show = new Show(Edit.CutExpression(expression), this, Affected, NotAffected, this.Owner, this.Enemy);
                    this.Actions.Add(Show);
                    Show.Effect();
                    break;
            }
        }
    }
    public class Character : Relics
    {
        public double attack;
        public double defense;

        /// <returns>Construye un personaje</returns>
        public Character(int id, string Name, int passiveDuration, int activeDuration, string imgAddress, string effect, string description, double attack, double defense) : base(null, null, id, Name, passiveDuration, activeDuration, imgAddress, false, "character", effect, description)
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
        public BattleField battleField{get; set;}

        public Player(Character character, string nick)
        {
            this.nick = nick;
            this.life = 100;
            this.hand = new List<Relics>();
            this.battleField.userBattleField = new Relics[4];
            this.character = character;
            this.state = State.Safe;
        }
        public void SetAux(Player Enemy)
        {

        }
        public void TakeFromDeck(double cards)
        {
            for (int i = 0; i < cards; i++)
            {
                Random rnd = new Random();
                int random = rnd.Next(1, gameVisual.SelectPlayer.Inventary.CardsInventary.Count()+1);
                foreach (var card in gameVisual.SelectPlayer.Inventary.CardsInventary)
                {
                    if(card.id == random)
                    {
                        this.hand.Add( new Relics(this, this.Enemy, card.id, card.name, card.passiveDuration, card.activeDuration, 
                                        card.imgAddress,card.isTrap, card.type, card.effect, card.description));
                        break;
                    }
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
                    return this.battleField.userBattleField.Length;
                case CardState.OnGraveyard:
                    return gameVisual.SelectPlayer.Inventary.GraveYard.Count();
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
    public class BattleField
    {
        public Sprite[] userVisualBattleField;
        public Relics[] userBattleField {get; set;}
    }
    #region auxiliar classes
    
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