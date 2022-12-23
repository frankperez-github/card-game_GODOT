using Godot;
using System.Collections.Generic;
using gameEngine;
using System.Diagnostics;
namespace gameVisual
{
    public class board : Godot.Node2D
    {   
        public static Game Game;
        public static Node child;
        public static Board VisualBoard;
        public static Button Attack;
        public static Button endButton;
        public Stopwatch watch;
        public bool VirtualPlay = true;

       
        public override void _Ready()
        {
            child = new Node();
            AddChild(child);
            Game = new Game(SelectPlayer.player1, SelectPlayer.player2);
            VisualBoard = new Board(Game, GetNode<Sprite>("GraveYard/Relic"), GetTree());

            GetNode<Sprite>("Preview/Relic").Visible = false;
            Attack = GetNode<Button>("Attack");
            endButton = GetNode<Button>("endButton");
            VisualBoard.Update();
            watch = new System.Diagnostics.Stopwatch();
        }
        public override void _Process(float delta)
        {
            // Checking end of game
            if (!(Game.player1.life > 0 && Game.player2.life > 0))
            {
                GetTree().ChangeScene("res://GameOver.tscn");
                GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
            }
            
            if (Game.turn % 2 == 0 && Game.player1 is VirtualPlayer) // Player's 1 trun (IA)
            {
                watch.Start();
                if(watch.ElapsedMilliseconds>750 && VirtualPlay)
                {
                    ((VirtualPlayer)(Game.player1)).Play();
                    VisualBoard.Update();

                    VisualMethods.AttackButtonFunction();
                    VirtualPlay = false;
                }
                if(watch.ElapsedMilliseconds > 2000)
                {
                    VirtualPlay = true;
                    VisualMethods.EndButtonFunction();
                    watch.Reset();
                }
            }

            //Wait for change Turn or Attack
            VisualMethods.ListenToVisualButtons();
        }
        public override void _Input(InputEvent @event)  
        {
            // Pause Menu
            if (@event is InputEventKey eventKey && eventKey.Scancode == (int)KeyList.Escape)
            {
                VisualMethods.ActiveEscapeMenu();
            }
            // Click
            if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
            {
                VisualMethods.ActiveClickAction(mouseEvent);
            }
            // Hover
            if (@event is InputEventMouse mouseMove)
            {
                // Preview player1 hand
                if (!(Game.player1 is VirtualPlayer))
                {
                    VisualMethods.PreviewHandCards(VisualBoard.visualHand1, mouseMove);
                }
                //Preview Player2 hand
                if (!(Game.player2 is VirtualPlayer))
                {
                    VisualMethods.PreviewHandCards(VisualBoard.visualHand2, mouseMove);
                }

                // Preview player1 Field
                VisualMethods.PreviewBattlefielCards(VisualBoard.visualBattleField1, mouseMove);
                // Preview player2 Field
                VisualMethods.PreviewBattlefielCards(VisualBoard.visualBattleField2, mouseMove);
            }
        
        }
        
    }       
}