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
            CardsInventory = new List<Relics>();
           
            CharactersInventory = new List<CharacterProperties>()
            {
                new CharacterProperties(1001, "El dragón indiferente", 1, 0, "res://Sprites/Cards-images/DALL·E 2022-10-10 15.21.44 - The lost dragon of Argea sleeping over a city digital art.png", "", "Intimida al oponente dando golpes en el pecho y lo hace perder 2 turnos", 10, 3),
                new CharacterProperties(1002, "El toro alado", 3, 0, "res://Sprites/Cards-images/DALL·E 2022-10-10 15.16.38 - The winged bull of Zeus digital art.png", "(Enemy.Remove.EnemyHand.1.EnemyHand)", "Crea un tornado con sus alas que arrasa con el oponente y lo deja sin cartas en la mano", 0, 5),
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

            // if (System.IO.File.Exists(JSONcharactersPath))
            // {
            //     string jsonContent = System.IO.File.ReadAllText(JSONcharactersPath);
            //     if (jsonContent != "")
            //     {
            //         CharactersInventory = JsonConvert.DeserializeObject<List<CharacterProperties>>(jsonContent);
            //     }

            // }
            // else
            // {
            //     GD.Print("No characters-inventory.json FOUNDED");
            // }
    }
}
}