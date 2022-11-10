using System;
using System.Collections.Generic;
namespace card_gameEngine
{
    public class Game
    {
        public static Dictionary<int, Character> CharactersInventary = board.CharactersInventary;
        // public static Player player1;
        // public static Player player2;
        public static void game()
        {
            // Console.Clear();
            // Console.WriteLine("Elige el personaje que seras");
            // Character character1 = CharactersInventary[int.Parse(Console.ReadLine())];
            // Console.WriteLine("Elige el personaje que sera tu oponente");
            // Character character2 = CharactersInventary[int.Parse(Console.ReadLine())];



            Dictionary<int, Relics> CardsInventary = board.CardsInventary;
            // List<int> Deck = CargarDeck(CardsInventary);
            
            Console.Clear();
        // while (player1.life > 0 && player2.life > 0)
        //     {
        //         Console.WriteLine("Turn: "+turn);
                

        //         if (turn % 2 != 0) //Impar
        //         {
                    
        //             //All activity of player 1 goes here 
        //             // int Option = 0;
        //             // while(Option != 3)
        //             // {
        //             //     Console.Clear();
        //             //     player1.printInfo();
        //             //     Console.WriteLine("Presione 1: Para activar cartas");
        //             //     Console.WriteLine("Presione 2: Para atacar");
        //             //     Console.WriteLine("Presione 3: Para Pasar turno");
        //             //     try
        //             //     {
        //             //         Option = int.Parse(Console.ReadLine());
        //             //     }
        //             //     catch (System.Exception){}
        //             //     switch (Option)
        //             //     {
        //             //         case 1: ActiveCards(player1);
        //             //         break;
        //             //         case 2: Attack(player1, player2);
        //             //         break;
        //             //         default:
        //             //         break;
        //             //     }
        //             // }
        //             // UpdateBattleField(player1);
        //             // Console.Clear();
        //         }

        //         if (turn % 2 == 0) //Par
        //         {
        //             player2.TakeFromDeck(player2, player1, 1, new List<int>());
        //             player2.printInfo();

        //             //All activity of player 2 goes here 
        //             // int Option = 0;
        //             // while(Option != 3)
        //             // {
        //             //     Console.Clear();
        //             //     player2.printInfo();
        //             //     Console.WriteLine("Presione 1: Para activar cartas");
        //             //     Console.WriteLine("Presione 2: Para atacar");
        //             //     Console.WriteLine("Presione 3: Para Pasar turno");
        //             //     try
        //             //     {
        //             //         Option = int.Parse(Console.ReadLine());
        //             //     }
        //             //     catch (System.Exception){}
        //             //     switch (Option)
        //             //     {
        //             //         case 1: ActiveCards(player2);
        //             //         break;
        //             //         case 2: Attack(player2, player1);
        //             //         break;
        //             //         default:
        //             //         break;
        //             //     }
        //             // }
        //             // UpdateBattleField(player2);
        //             // Console.Clear();
        //         }
        //         turn++; 
        //     }
        }

        public static List<int> CargarDeck(Dictionary<int, Relics> CardsInventary)
        {
            List<int> result = new List<int>();
            foreach (var card in CardsInventary)
            {
                result.Add(card.Value.id);    
            }
            //Console.WriteLine(string.Join(" ", result));
            return result;
        }
        public static void UpdateBattleField(Player player)
        {
            for (int index = 0; index < player.hand.Count; index++)
            {
                if (player.hand[index].cardState == CardState.Activated)
                {

                    if (player.hand[index].activeDuration == 1)
                    {
                        foreach (var effect in player.hand[index].EffectsOrder)
                        {
                            if(effect.Key == 5)
                            {
                                effect.Value.affects = effect.Value.affects*(-1); 
                                player.hand[index].Effect();
                                effect.Value.affects = effect.Value.affects*(-1);
                            }
                        }
                        player.hand[index].cardState = CardState.OnGraveyard; // Removing card from battelfield
                    }
                    else
                    {
                        if (player.hand[index].passiveDuration != 0)
                        {
                            player.hand[index].passiveDuration--;
                        }
                        else
                        {
                            int Defaultpassive = board.CardsInventary[player.hand[index].id].passiveDuration;
                            player.hand[index].passiveDuration = Defaultpassive;
                            player.hand[index].activeDuration--;
                        }
                    }
                }
            }
        }
        public static void ActiveCards(Player player)
        {
            do
            {
                Console.WriteLine("Elige la carta que quieres activar");
                ActiveEffect(player, int.Parse(Console.ReadLine()));
                Console.WriteLine("Si quiere activar otra carta presione: 1, si no presione 2");
            } while (int.Parse(Console.ReadLine()) != 2);
            
            
        }
        public static void ActiveEffect(Player player, int HandPossition)
        {
            
            foreach (var card in player.hand)
            {
                if(card.cardState==CardState.OnHand)
                {
                    if(HandPossition==0)
                    {
                        card.Effect();
                        card.cardState = CardState.Activated;     
                        break;
                    }
                    HandPossition--;
                }
            }
            
        }
        public static void Attack(Player player, Player enemy)
        {
            enemy.life = enemy.life - player.attack;
        }
    }
}