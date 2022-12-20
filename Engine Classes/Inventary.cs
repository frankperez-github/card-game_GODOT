using System.Collections.Generic;
using Newtonsoft.Json;
using Godot;
using System.IO;

namespace gameEngine
{
    public class inventory
    {
        
        public static string JSONcardsPath = "./Sprites/cards-inventory.json";
        public static string JSONcharactersPath = "./Sprites/characters-inventory.json";
        public List<CharacterProperties> CharactersInventory;
        public List<Relics> CardsInventory;
        public inventory()
        {
            Player defaultPlayer = new Player("default");
            CardsInventory = new List<Relics>()
            {
                //Espada del Destino
                //Te suma 15 de ataque
                new Relics(defaultPlayer, defaultPlayer, 1, "Espada del destino", 0, 3, "res://Sprites/Cards-images/DALL·E 2022-10-10 13.46.09 - Sword of destiny.png", false, "damage", "(Owner.Attack.15)", "Te suma 15 de ataque"),

                //Capsula del Tiempo
                //Roba una carta del cementerio
                new Relics(defaultPlayer, defaultPlayer, 2, "Capsula del Tiempo", 0, 1, "res://Sprites/Cards-images/DALL·E 2022-10-10 14.53.40 - antique time capsule in center of galaxy digital art.png", false, "draw", "(Owner.Draw.EnemyHand.1)", "Roba una carta del cementerio"),

                //Anillo de Zeus
                //Ganas 5 de vida por cada carta en tu mano
                new Relics(defaultPlayer, defaultPlayer, 3, "Anillo de Zeus", 0, 1, "res://Sprites/Cards-images/DALL·E 2022-10-10 14.55.09 - god's ring in zeus' hand digital art.png", false,  "cure", "(Owner.Cure.5.OwnerHand)", "Ganas 5 de vida por cada carta en tu mano"),

                //Escudo de la pobreza
                //Trap, evita el 50% del daño del enemigo
                new Relics(defaultPlayer, defaultPlayer, 4, "Escudo de la pobreza", 0, 1, "res://Sprites/Cards-images/DALL·E 2022-10-10 14.27.55 - the cursed shield digital art (4).png", true, "defense", "(Owner.Defense.1.0,5)", "Evita el 50% del daño del enemigo"),

                //Libro de los secretos 
                //Robas 2 cartas del deck
                new Relics(defaultPlayer, defaultPlayer, 5, "Libro de los secretos", 0, 1, "res://Sprites/Cards-images/DALL·E 2022-10-23 16.39.14 - The Book of the Dead in a temple digital art.png", false, "draw", "(Owner.Draw.Deck.random.5)", "Robas 2 cartas del deck"),
                
                //Caliz de la Venganza
                //Tu adversario descarta 2 cartas de su mano
                new Relics(defaultPlayer, defaultPlayer, 6, "Caliz de la Venganza", 0, 1, "res://Sprites/Cards-images/DALL·E 2022-10-10 14.34.49 -  chalice of blood digital art.png", false, "draw", "(Enemy.Remove.EnemyHand.2)", "Tu adversario descarta 2 cartas de su mano"),

                // //Resfriado
                // //El adversario queda congelado por 2 turnos
                // new Relics(defaultPlayer, defaultPlayer, 7, "Resfriado", 1, 2, "imgpath4", false, "state", "(Enemy.ChangeState.Freezed)", "El adversario queda congelado por 2 turnos"),

                //Objetivo enemigo
                //Destruye 1 reliquia que tenga activa enemigo
                new Relics(defaultPlayer, defaultPlayer, 8, "Objetivo Enemigo", 0, 1, "res://Sprites/Cards-images/DALL·E 2022-10-23 16.39.18 - The mohoho's  curse digital art.png", false, "trap", "(Enemy.Remove.EnemyBattlefield.Battlefield.random.1)", "Destruye 1 reliquia que tenga activa enemigo"),

                // El ojo blanco
                // Muestra todas las cartas en la mano del enemigo
                new Relics(defaultPlayer, defaultPlayer, 9, "El ojo blanco", 0, 2, "res://Sprites/Cards-images/DALL·E 2022-10-23 16.30.30 - the talisman invocation in a ritual digital art.png", false, "show", "(Enemy.Show.all)", "Muestra todas las cartas en la mano del enemigo"),

                // // El ojo negro
                // // Muestra 2 cartas de la mano del enemigo
                // new Relics(defaultPlayer, defaultPlayer, 10, "El ojo negro", 0, 2, "imgpath4", false, "show", "(Enemy.Show.2)", "Muestra 2 cartas de la mano del enemigo")

                // La lanza de la muerte
                // Suma 25 de ataque
                new Relics(defaultPlayer, defaultPlayer, 11, "La lanza de la muerte", 0, 1, "", false, "damage", "(Owner.Attack.25)", "Suma 25 de ataque"),

                // Token
                // Reunir 3 tokens activa el efecto especial de tu personaje
                new Relics(defaultPlayer, defaultPlayer, 12, "Token", 0, 1, "", false, "random", "", "Reunir 3 tokens activa el efecto especial de tu personaje"),

                // Espejo de impaciencia
                // Evita el ataque del enemigo
                new Relics(defaultPlayer, defaultPlayer, 13, "Espejo de impaciencia", 0, 1, "", true, "defense", "(Owner.Defense.1.1)", "Se activa cuando el adversario ataca y evita el ataque"),

                // El arco del último momento
                // Devuelve el ataque del enemigo
                new Relics(defaultPlayer, defaultPlayer, 14, "El arco del último momento", 0, 1, "", true, "defense", "(Owner.Defense.1.1)\n(Enemy.Cure.5)", "Devuelve el ataque del enemigo"),
            };
            
            CharactersInventory = new List<CharacterProperties>()
            {
                new CharacterProperties(1001, "El dragón indiferente", 1, 0, "res://Sprites/Cards-images/DALL·E 2022-10-10 15.21.44 - The lost dragon of Argea sleeping over a city digital art.png", "", "Intimida al oponente dando golpes en el pecho y lo hace perder 2 turnos", 10, 3),
                new CharacterProperties(1002, "El toro alado", 3, 0, "res://Sprites/Cards-images/DALL·E 2022-10-10 15.16.38 - The winged bull of Zeus digital art.png","", "Crea un tornado con sus alas que arrasa con el oponente dejándolo con 1 carta en su mano", 0, 5),
                new CharacterProperties(1003, "La serpiente truhana", 1, 0, "res://Sprites/Cards-images/DALL·E 2022-10-23 15.35.44 - medieval style snake in a nuclear war digital art.png", "", "No hay nada mas tóxico que ella (ni siquiera tu ex) Empieza un envenenamiento que le quita 5 de vida al enemigo en cada turno", 5, 0),
                new CharacterProperties(1004, "El tigre recursivo", 1, 0, "res://Sprites/Cards-images/DALL·E 2022-10-23 15.49.37 - Hercules' tiger in a nuclear war digital art.png", "", "Cada 3 turnos puedes robar una carta extra del deck", 8, 0),
                new CharacterProperties(1005, "El leon amistoso", 2, 0, "imgpath", "", "Tiene el don de rejuvenecer que le permite recuperar 10 de vida en cada turno", 0, 1)
            };

        }
        public void addToJson(Relics card)
        {
            CardsInventory.Add(card);
            string jsonContent = JsonConvert.SerializeObject(CardsInventory, Formatting.Indented);
            System.IO.File.WriteAllText(JSONcardsPath, jsonContent);
        }
        public void addToJson(CharacterProperties character)
        {
            CharactersInventory.Add(character);
            string jsonContent = JsonConvert.SerializeObject(CharactersInventory, Formatting.Indented);
            System.IO.File.WriteAllText(JSONcharactersPath, jsonContent);
        }
        public void ImportJsonContent()
        {
            if (System.IO.File.Exists(JSONcardsPath))
            {
                string jsonContent = System.IO.File.ReadAllText(JSONcardsPath);
                if (jsonContent != "")
                {
                    CardsInventory = JsonConvert.DeserializeObject<List<Relics>>(jsonContent);
                }

            }
            else
            {
                GD.Print("No cards-inventory.json FOUNDED");
            }

            if (System.IO.File.Exists(JSONcharactersPath))
            {
                string jsonContent = System.IO.File.ReadAllText(JSONcharactersPath);
                if (jsonContent != "")
                {
                    CardsInventory = JsonConvert.DeserializeObject<List<Relics>>(jsonContent);
                }

            }
            else
            {
                GD.Print("No characters-inventory.json FOUNDED");
            }
    }
}
}