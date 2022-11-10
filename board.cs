using Godot;
using System.Collections.Generic;
namespace card_gameEngine
{
    public class board : Godot.Node2D
    {   

        public static List<Player> PlayersInventary = new List<Player>();
        public static Dictionary<int, Relics> CardsInventary= new Dictionary<int, Relics>();
        public static Dictionary<int, Character> CharactersInventary = new Dictionary<int, Character>();
        public static List<int> GraveYard = new List<int>();
        public static int turn = 1;

        const int maxinHand = 6;
        static Sprite Relic = new Sprite();

        public override void _Ready()
        {

            #region Defining characters
            CharactersInventary.Add(1, new Character("El dragón indiferente", 1, 0, "imgpath1", 10, 3));
            CharactersInventary.Add(2, new Character("El toro alado", 3, 0, "imgpath2", 0, 5));
            CharactersInventary.Add(3, new Character("La serpiente truhana", 1, 0, "imgpath3", 5, 0));
            CharactersInventary.Add(4, new Character("El tigre recursivo", 1, 0, "imgpath4", 8, 0));
            CharactersInventary.Add(5, new Character("El leon amistoso", 2, 0, "imgpath", 0, 1));
            #endregion

            Player defaultPlayer = new Player(CharactersInventary[1], "defaultName");
            #region Defining cards
            //Espada del Destino
            //Te suma 15 de ataque
            Dictionary<int, ActionInfo> card1Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card1Info = new ActionInfo(relativePlayer.Owner, 15);
            card1Dict.Add(5, card1Info);
            CardsInventary.Add(1, new Relics(defaultPlayer, defaultPlayer, 1, "Espada del destino", 0, 3, "img", false, new Condition(), card1Dict));

            //Capsula del Tiempo
            //Roba una carta del cementerio
            Dictionary<int, ActionInfo> card2Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card2Info = new ActionInfo(relativePlayer.Owner, 1);
            card2Dict.Add(3, card2Info);
            CardsInventary.Add(2,new Relics(defaultPlayer, defaultPlayer, 2, "Capsula del Tiempo", 0, 1, "imgpath2", false,new Condition(), card2Dict));

            //Anillo de Zeus
            //Ganas 5 de vida por cada carta en tu mano
            Player defaultPlayer1 = new Player(CharactersInventary[1], "pepito");
            Dictionary<int, ActionInfo> card3Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card3Info = new ActionInfo(relativePlayer.Owner, 5, 1, relativeFactor.OwnerHand);
            card3Dict.Add(4, card3Info);
            CardsInventary.Add(3 ,new Relics(defaultPlayer1, defaultPlayer, 3, "Anillo de Zeus", 0, 1, "imgpath3", false,  new Condition(), card3Dict));

            //Escudo de la pobreza
            //Trap, evita el 50% del dano del enemigo
            Dictionary<int, ActionInfo> card4Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card4Info = new ActionInfo(relativePlayer.Owner, 1, 0.5, relativeFactor.Fixed);
            card4Dict.Add(6, card4Info);
            CardsInventary.Add(4,new Relics(defaultPlayer, defaultPlayer, 4, "Escudo de la pobreza", 0, 1, "imgpath", true, new Condition(), card4Dict));

            //Libro de los secretos 
            //Robas 2 cartas del deck
            Dictionary<int, ActionInfo> card5Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card5Info = new ActionInfo(2, new List<int>());
            card5Dict.Add(1, card5Info);
            CardsInventary.Add(5,new Relics(defaultPlayer, defaultPlayer, 5, "Libro de los secretos", 0, 1, "imgpath4", false, new Condition(), card5Dict));
            
            //Caliz de la Venganza
            //Tu adversario descarta 2 cartas de su mano
            Dictionary<int, ActionInfo> card6Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card6Info = new ActionInfo(relativePlayer.Enemy, 2, new List<int>());
            card6Dict.Add(7, card6Info);
            CardsInventary.Add(6,new Relics(defaultPlayer, defaultPlayer, 5, "Libro de los secretos", 0, 1, "imgpath4", false, new Condition(), card6Dict));
            #endregion

            Player player1 = new Player(CharactersInventary[1], "Pepe el macho");
            Player player2 = new Player(CharactersInventary[2], "Juan la sombra");
            PlayersInventary.Add(player1);
            PlayersInventary.Add(player2);

            player1.TakeFromDeck(player1, player2, 5, new List<int>());
            player2.TakeFromDeck(player2, player1, 5, new List<int>());

            // AddToHand(player1);
            // AddToEnemyHand(player2);

        }

        public override void _Process(float delta)
        {
            GetTree().Paused = false;
            Player player1 = PlayersInventary[0];
            Player player2 = PlayersInventary[1];

            Button Attack = GetNode<Button>("Attack");
            Button endButton = GetNode<Button>("endButton");

            Label Turnlabel = GetNode<Label>("TurnLabel");

            #region updating players' info
            Label PlayerNick = GetNode<Label>("PlayerInfo/Player's Nick");
            PlayerNick.Text = PlayersInventary[0].nick;
            Label PlayerLife = GetNode<Label>("PlayerInfo/Player's Life");
            PlayerLife.Text = "Life : "+ PlayersInventary[0].life.ToString();
            Label PlayerShield = GetNode<Label>("PlayerInfo/Player's Shield");
            PlayerShield.Text = "Shield : "+ PlayersInventary[0].defense.ToString();
            Label PlayerAttack = GetNode<Label>("PlayerInfo/Player's Attack");
            PlayerAttack.Text = "Attack : "+ PlayersInventary[0].attack.ToString();

            Label Player2Nick = GetNode<Label>("Player2Info/Player2's Nick");
            Player2Nick.Text = PlayersInventary[1].nick;
            Label Player2Life = GetNode<Label>("Player2Info/Player2's Life");
            Player2Life.Text = "Life : "+ PlayersInventary[1].life.ToString();
            Label Player2Shield = GetNode<Label>("Player2Info/Player2's Shield");
            Player2Shield.Text = "Shield : "+ PlayersInventary[1].defense.ToString();
            Label Player2Attack = GetNode<Label>("Player2Info/Player2's Attack");
            Player2Attack.Text = "Attack : "+ PlayersInventary[1].attack.ToString();
            #endregion

            // Checking end of game
            if (!(player1.life > 0 && player2.life > 0))
            {
                if (player1.life <= 0)
                {
                    Turnlabel.Text = player2.nick + " wins";
                }

                if (player2.life <= 0)
                {
                    Turnlabel.Text = player1.nick + " wins";
                }
            }
            
            Attack.Visible = true;
            if (turn % 2 == 0) // Player1's Turn
            {
                if (Attack.Pressed)
                {
                    player2.life -= player1.attack;
                    Attack.Visible = false;
                }
            }
            else // Player2's Turn
            {
                if (Attack.Pressed)
                {
                    player1.life -= player2.attack;
                    Attack.Visible = false;
                }
            }

            //Change Turn
            if (endButton.Pressed)
            {

                if (turn % 2 == 0)
                {
                    // Next Player takes a card
                    player2.TakeFromDeck(player2, player1, 1, new List<int>());
                    AddToEnemyHand(player2);
                    
                }
                else
                {
                    // Next Player takes a card
                    player1.TakeFromDeck(player1, player2, 1, new List<int>()); 
                    // CheckAndDiscard(player1); 
                    AddToHand(player1);
                }

                turn++;
                Turnlabel.Text = "Turno: " + turn;
                endButton.Disabled = true; // Disbling button, increment turn just one time
            }
            endButton.Disabled = false;
        }
  
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

