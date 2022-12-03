using Godot;
using System.Collections.Generic;

namespace gameEngine
{
    public class Inventary
    {
        public List<CharacterProperties> CharactersInventary;
        public List<Relics> CardsInventary;
        public Inventary()
        {
            Player defaultPlayer = new Player("default");
            CardsInventary = new List<Relics>()
            {
                //Espada del Destino
                //Te suma 15 de ataque
                new Relics(defaultPlayer, defaultPlayer, 1, "Espada del destino", 0, 3, "img", false, "damage", "(Owner.Attack.15)", "Te suma 15 de ataque"),

                //Capsula del Tiempo
                //Roba una carta del cementerio
                new Relics(defaultPlayer, defaultPlayer, 2, "Capsula del Tiempo", 0, 1, "imgpath2", false, "draw", "(Owner.Draw.EnemyHand.1)", "Roba una carta del cementerio"),

                //Anillo de Zeus
                //Ganas 5 de vida por cada carta en tu mano
                new Relics(defaultPlayer, defaultPlayer, 3, "Anillo de Zeus", 0, 1, "imgpath3", false,  "cure", "(Owner.Cure.5.OwnerHand)", "Ganas 5 de vida por cada carta en tu mano"),

                //Escudo de la pobreza
                //Trap, evita el 50% del daño del enemigo
                new Relics(defaultPlayer, defaultPlayer, 4, "Escudo de la pobreza", 0, 1, "imgpath", true, "defense", "(Owner.Defense.1.0,5)", "Evita el 50% del daño del enemigo"),

                //Libro de los secretos 
                //Robas 2 cartas del deck
                new Relics(defaultPlayer, defaultPlayer, 5, "Libro de los secretos", 0, 1, "imgpath4", false, "draw", "(Owner.Draw.Deck.random.2)", "Robas 2 cartas del deck"),
                
                //Caliz de la Venganza
                //Tu adversario descarta 2 cartas de su mano
                new Relics(defaultPlayer, defaultPlayer, 6, "Caliz de la Venganza", 0, 1, "imgpath4", false, "draw", "(Enemy.Remove.EnemyHand.2)", "Tu adversario descarta 2 cartas de su mano"),

                //Resfriado
                //El adversario queda congelado por 2 turnos
                new Relics(defaultPlayer, defaultPlayer, 7, "Resfriado", 1, 2, "imgpath4", false, "state", "(Enemy.ChangeState.Freezed)", "El adversario queda congelado por 2 turnos"),

                //Objetivo enemigo
                //Destruye 1 reliquia que tenga activa enemigo
                new Relics(defaultPlayer, defaultPlayer, 8, "Objetivo Enemigo", 0, 1, "imgpath4", false, "trap", "(Enemy.Remove.EnemyBattlefield.Battlefield.random.1)", "Destruye 1 reliquia que tenga activa enemigo"),

                // El ojo blanco
                // Muestra todas las cartas en la mano del enemigo
                new Relics(defaultPlayer, defaultPlayer, 9, "El ojo blanco", 0, 2, "imgpath4", false, "show", "(Enemy.Show.all)", "Muestra todas las cartas en la mano del enemigo"),

                // El ojo negro
                // Muestra 2 cartas de la mano del enemigo
                new Relics(defaultPlayer, defaultPlayer, 10, "El ojo negro", 0, 2, "imgpath4", false, "show", "(Enemy.Show.2)", "Muestra 2 cartas de la mano del enemigo")
            };
            
            CharactersInventary = new List<CharacterProperties>()
            {
                new CharacterProperties(1001, "El dragón indiferente", 1, 0, "imgpath1", "", "", 10, 3),
                new CharacterProperties(1002, "El toro alado", 3, 0, "imgpath2","", "", 0, 5),
                new CharacterProperties(1003, "La serpiente truhana", 1, 0, "imgpath3", "", "", 5, 0),
                new CharacterProperties(1004, "El tigre recursivo", 1, 0, "imgpath4", "", "", 8, 0),
                new CharacterProperties(1005, "El leon amistoso", 2, 0, "imgpath", "", "", 0, 1)
            };

        }
    }
}