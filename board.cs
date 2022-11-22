using Godot;
using System.Collections.Generic;
namespace gameVisual
{
    public class board : Godot.Node2D
    {   
        Visual Visual = new Visual();
        public override void _Ready()
        {

            // setting virtual player's character and nick
            if (mainMenu.gameType.ToLower() == "virtual")
            {
                gameEngine.Settings.PlayersInventary.Add(new gameEngine.Player(gameEngine.Settings.CharactersInventary[0], "el otro"));
            }

            gameEngine.Settings.player1 = gameEngine.Settings.PlayersInventary[0];
            gameEngine.Settings.player2 = gameEngine.Settings.PlayersInventary[1];

            
            gameEngine.Settings.player1.TakeFromDeck(gameEngine.Settings.player1, gameEngine.Settings.player2, 5, new List<gameEngine.Relics>());
            gameEngine.Settings.player2.TakeFromDeck(gameEngine.Settings.player2, gameEngine.Settings.player1, 5, new List<gameEngine.Relics>());

            Visual.RefreshBoard();
        }

        bool player1Attack = false;
        bool player2Attack = false;
        public override void _Process(float delta)
        {

            Label Turnlabel = GetNode<Label>("TurnLabel");

            // Checking end of game
            if (!(gameEngine.Settings.player1.life > 0 && gameEngine.Settings.player2.life > 0))
            {
                GetTree().ChangeScene("res://GameOver.tscn");
            }

            Button Attack = GetNode<Button>("Attack");
            Button endButton = GetNode<Button>("endButton");

            
            #region Updating visualy playerInfo
            Label PlayerNick = GetNode<Label>("PlayerInfo/Player's Nick");
            PlayerNick.Text = gameEngine.Settings.PlayersInventary[0].nick;
            Label PlayerLife = GetNode<Label>("PlayerInfo/Player's Life");
            PlayerLife.Text = "Life : "+ gameEngine.Settings.PlayersInventary[0].life.ToString();
            Label PlayerShield = GetNode<Label>("PlayerInfo/Player's Shield");
            PlayerShield.Text = "Shield : "+ gameEngine.Settings.PlayersInventary[0].defense.ToString();
            Label PlayerAttack = GetNode<Label>("PlayerInfo/Player's Attack");
            PlayerAttack.Text = "Attack : "+ gameEngine.Settings.PlayersInventary[0].attack.ToString();
            Label Player1State = GetNode<Label>("PlayerInfo/Player's State");
            Player1State.Text = "State : "+ gameEngine.Settings.PlayersInventary[0].state.ToString();

            Label Player2Nick = GetNode<Label>("Player2Info/Player2's Nick");
            Player2Nick.Text = gameEngine.Settings.PlayersInventary[1].nick;
            Label Player2Life = GetNode<Label>("Player2Info/Player2's Life");
            Player2Life.Text = "Life : "+ gameEngine.Settings.PlayersInventary[1].life.ToString();
            Label Player2Shield = GetNode<Label>("Player2Info/Player2's Shield");
            Player2Shield.Text = "Shield : "+ gameEngine.Settings.PlayersInventary[1].defense.ToString();
            Label Player2Attack = GetNode<Label>("Player2Info/Player2's Attack");
            Player2Attack.Text = "Attack : "+ gameEngine.Settings.PlayersInventary[1].attack.ToString();
            Label Player2State = GetNode<Label>("Player2Info/Player2's State");
            Player2State.Text = "State : "+ gameEngine.Settings.PlayersInventary[1].state.ToString();
            #endregion

            // ATTACK BUTTON
            Attack.Disabled = false;
            if (Attack.Pressed)
            {
                if (gameEngine.Settings.turn % 2 == 0 && !player1Attack) // Player1's Turn
                {
                    gameEngine.Settings.player2.life -= gameEngine.Settings.player1.attack;
                    player1Attack = true;
                    player2Attack = false; // Enabling attack button to next turn 
                    Attack.Disabled = true;
                }
                if (gameEngine.Settings.turn % 2 != 0 && !player2Attack)// Player2's Turn
                {
                    gameEngine.Settings.player1.life -= gameEngine.Settings.player2.attack;
                    player2Attack = true;
                    player1Attack = false; // Enabling attack button to next turn 
                    Attack.Disabled = true;
                }
            }


            //Change Turn (END BUTTON)
            if (endButton.Pressed)
            {
                if (gameEngine.Settings.turn % 2 == 0) // Player1's turn
                {
                    // Next Player takes a card
                    gameEngine.Settings.player1.TakeFromDeck(gameEngine.Settings.player2, gameEngine.Settings.player1, 1, new List<gameEngine.Relics>());
                    gameVisual.Visual.UpdateBattleField(gameEngine.Settings.player1);
                }
                else // Player2's turn
                {
                    // Next Player takes a card
                    gameEngine.Settings.player2.TakeFromDeck(gameEngine.Settings.player1, gameEngine.Settings.player2, 1, new List<gameEngine.Relics>());
                    Visual.UpdateBattleField(gameEngine.Settings.player2);
                }
                Visual.RefreshBoard();
                gameEngine.Settings.turn++;
                Turnlabel.Text = "Turno: " + gameEngine.Settings.turn;
                endButton.Disabled = true; // Disabling button, increment turn just one time
            }
            endButton.Disabled = false;
        }

        public override void _Input(InputEvent @event)
        {
            Visual.HandleEvent(@event);
        }
    }       
}