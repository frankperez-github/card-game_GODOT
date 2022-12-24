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
                new CharacterProperties(1000, "El dragón indiferente", 0, 2, "res://Sprites/Cards-images/DALL·E 2022-10-10 15.21.44 - The lost dragon of Argea sleeping over a city digital art.png", "(Enemy.ChangeState.Asleep)", "Intimida al oponente dando golpes en el pecho y lo hace perder 2 turnos", 10, 3),
                new CharacterProperties(1001, "El toro alado", 0, 1, "res://Sprites/Cards-images/DALL·E 2022-10-10 15.16.38 - The winged bull of Zeus digital art.png", "(Enemy.Remove.EnemyHand.1.EnemyHand)", "Crea un tornado con sus alas que arrasa con el oponente y lo deja sin cartas en la mano", 0, 5),
                new CharacterProperties(1002, "La serpiente truhana", 0, 2, "res://Sprites/Cards-images/DALL·E 2022-10-23 15.35.44 - medieval style snake in a nuclear war digital art.png", "(Enemy.ChangeState.Poisoned)", "No hay nada mas tóxico que ella (ni siquiera tu ex) Empieza un envenenamiento que le quita 10 de vida al enemigo en cada turno", 5, 0),
                new CharacterProperties(1003, "El tigre recursivo", 2, 4, "res://Sprites/Cards-images/DALL·E 2022-10-23 15.49.37 - Hercules' tiger in a nuclear war digital art.png", "(Owner.Draw.Deck.1)", "Cada 2 turnos puedes robar una carta extra del deck, este efecto se activa solo 3 veces", 8, 0),
                new CharacterProperties(1004, "El leon amistoso", 1, 5, "res://Sprites/Cards-images/DALL·E 2022-12-23 19.49.07 - un  leon guerrero estilo medieval con una cicatriz en la cara, arte digital.png", "(Owner.Cure.10)", "Tiene el don de rejuvenecer que le permite recuperar 10 de vida en cada turno, durante 5 turnos", 0, 1)
            };
            ResetActiveDuration(CharactersInventory);
        }
        public void addToJson(Relics card)
        {
            CardsInventory.Add(card);
            string jsonContent = JsonConvert.SerializeObject(CardsInventory, Formatting.Indented);
            System.IO.File.AppendAllText(JSONcardsPath, jsonContent);
        }
        public void addToJson(CharacterProperties character)
        {
            CharactersInventory.Add(character);
            string jsonContent = JsonConvert.SerializeObject(CharactersInventory, Formatting.Indented);
            System.IO.File.AppendAllText(JSONcharactersPath, jsonContent);
        }
        public void OverrideJson()
        {
            if (System.IO.File.Exists(JSONcardsPath))
            {
                string jsonContent = JsonConvert.SerializeObject(CardsInventory, Formatting.Indented);
                System.IO.File.WriteAllText(JSONcharactersPath, jsonContent);
            }
            else
            {
                GD.Print("No cards-inventory.json FOUNDED");
            }
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

    }
        public void ResetActiveDuration()
        {
            foreach (Relics relic in CardsInventory)
            {
                if(relic.activeDuration != 1)
                    relic.activeDuration = relic.activeDuration / 2;
            }
        }
        public void ResetActiveDuration(List<CharacterProperties> Character)
        {
            foreach (CharacterProperties character in Character)
            {
                if(character.activeDuration != 1)
                    character.activeDuration = character.activeDuration / 2;
            }
        }
        
}
/*
    CARTAS ESPECIALES
  {
    "cardState": 2,
    "Actions": [],
    "id": 13,
    "name": "Espejo de impaciencia",
    "activeDuration": 1,
    "passiveDuration": 0,
    "imgAddress": "res://Sprites/Cards-images/DALL·E 2022-10-10 14.47.44 - magic mirror reflecting powerfull attack of sauron.png",
    "Owner": {
      "character": null,
      "Enemy": null,
      "nick": "default",
      "life": 100.0,
      "hand": [],
      "state": 0,
      "BattleField": [
        null,
        null,
        null,
        null
      ]
    },
    "Enemy": {
      "character": null,
      "Enemy": null,
      "nick": "default",
      "life": 100.0,
      "hand": [],
      "state": 0,
      "BattleField": [
        null,
        null,
        null,
        null
      ]
    },
    "type": "defense",
    "isTrap": true,
    "effect": "(Owner.Defense.1.1)",
    "description": "Se activa cuando el adversario ataca y evita el ataque"
  },
  */
}