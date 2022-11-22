using System.Collections.Generic;
namespace gameEngine
{
    public class Settings
    {
        public static Player player1;
        public static Player player2;
        public static List<Player> PlayersInventary = new List<Player>();
        public static List<Character> CharactersInventary = new List<Character>();
        public static Dictionary<int, Relics> CardsInventary= new Dictionary<int, Relics>();
        public static List<Relics> GraveYard = new List<Relics>();
        public static int turn = 1;

        public static void SetConfig()
        {
            #region Defining characters
            CharactersInventary.Add(new Character("El dragón indiferente", 1, 0, "imgpath1", 10, 3));
            CharactersInventary.Add(new Character("El toro alado", 3, 0, "imgpath2", 0, 5));
            CharactersInventary.Add(new Character("La serpiente truhana", 1, 0, "imgpath3", 5, 0));
            CharactersInventary.Add(new Character("El tigre recursivo", 1, 0, "imgpath4", 8, 0));
            CharactersInventary.Add(new Character("El leon amistoso", 2, 0, "imgpath", 0, 1));
            #endregion

            gameEngine.Player defaultPlayer = new gameEngine.Player(gameEngine.Settings.CharactersInventary[1], "defaultName");

            #region Defining cards
            //Espada del Destino
            //Te suma 15 de ataque
            Dictionary<int, ActionInfo> card1Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card1Info = new ActionInfo(relativePlayer.Owner, 15);
            card1Dict.Add(5, card1Info);
            gameEngine.Settings.CardsInventary.Add(1, new Relics(defaultPlayer, defaultPlayer, 1, "Espada del destino", 0, 3, "img", false, "", "damage", card1Dict));

            //Capsula del Tiempo
            //Roba una carta del cementerio
            Dictionary<int, ActionInfo> card2Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card2Info = new ActionInfo(relativePlayer.Owner, 1,"deck");
            card2Dict.Add(3, card2Info);
            gameEngine.Settings.CardsInventary.Add(2,new Relics(defaultPlayer, defaultPlayer, 2, "Capsula del Tiempo", 0, 1, "imgpath2", false, "", "draw", card2Dict));

            //Anillo de Zeus
            //Ganas 5 de vida por cada carta en tu mano
            Player defaultPlayer1 = new Player(gameEngine.Settings.CharactersInventary[1], "pepito");
            Dictionary<int, ActionInfo> card3Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card3Info = new ActionInfo(relativePlayer.Owner, 5, 1, relativeFactor.OwnerHand);
            card3Dict.Add(4, card3Info);
            gameEngine.Settings.CardsInventary.Add(3 ,new Relics(defaultPlayer1, defaultPlayer, 3, "Anillo de Zeus", 0, 1, "imgpath3", false,  "", "cure", card3Dict));

            //Escudo de la pobreza
            //Trap, evita el 50% del daño del enemigo
            Dictionary<int, ActionInfo> card4Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card4Info = new ActionInfo(relativePlayer.Owner, 1, 0.5, relativeFactor.Fixed);
            card4Dict.Add(4, card4Info);
            gameEngine.Settings.CardsInventary.Add(4,new Relics(defaultPlayer, defaultPlayer, 4, "Escudo de la pobreza", 0, 1, "imgpath", true, "", "defense", card4Dict));

            //Libro de los secretos 
            //Robas 2 cartas del deck
            Dictionary<int, ActionInfo> card5Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card5Info = new ActionInfo(2, "deck");
            card5Dict.Add(1, card5Info);
            gameEngine.Settings.CardsInventary.Add(5,new Relics(defaultPlayer, defaultPlayer, 5, "Libro de los secretos", 0, 1, "imgpath4", false, "", "draw", card5Dict));
            
            //Caliz de la Venganza
            //Tu adversario descarta 2 cartas de su mano
            Dictionary<int, ActionInfo> card6Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card6Info = new ActionInfo(relativePlayer.Enemy, 2, "deck");
            card6Dict.Add(7, card6Info);
            gameEngine.Settings.CardsInventary.Add(6,new Relics(defaultPlayer, defaultPlayer, 6, "Caliz de la Venganza", 0, 1, "imgpath4", false, "", "draw", card6Dict));

            //Resfriado
            //El adversario queda congelado por 2 turnos
            Dictionary<int, ActionInfo> card7Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card7Info = new ActionInfo(relativePlayer.Enemy, State.Freezed);
            card7Dict.Add(8, card7Info);
            gameEngine.Settings.CardsInventary.Add(7,new Relics(defaultPlayer, defaultPlayer, 7, "Resfriado", 0, 2, "imgpath4", false, "", "state", card7Dict));

            //Objetivo enemigo
            //Destruye 1 reliquia que tenga activa el enemigo
            Dictionary<int, ActionInfo> card8Dict = new Dictionary<int, ActionInfo>();
            ActionInfo card8Info = new ActionInfo(relativePlayer.Enemy, 1, "deck");
            card8Dict.Add(9, card8Info);
            gameEngine.Settings.CardsInventary.Add(8,new Relics(defaultPlayer, defaultPlayer, 8, "Objetivo Enemigo", 0, 1, "imgpath4", false, "", "trap", card8Dict));
            #endregion

        }
        
    }
}