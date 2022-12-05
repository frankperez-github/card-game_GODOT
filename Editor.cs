using Godot;
using System;
using System.Collections.Generic;

public class Editor : Node
{
    string lastPressed = "";

    public override void _Ready()
    {
        GetTree().SetScreenStretch(SceneTree.StretchMode.Mode2d, SceneTree.StretchAspect.Keep, new Vector2(1920, 1080), 1);
    }
    public override void _Process(float delta)
    {
        List<string> operators = new List<string>()
        {
            ">",
            ">=",
            "<",
            "<=",
            "==",
            "&&",
            "| |",
            "+" ,
            "-" ,
            "*" ,
            "/" ,
            "%",
            "(",
            ")",
            "}",
        };
        
        TextEdit Effect = GetNode<TextEdit>("Code/Label4/Effect");
        
        //Operators buttons
        Label Operators = GetNode<Label>("Options/Operators");
        TextEdit number = GetNode<TextEdit>("Options/Operators/Button22/TextEdit");
        for (int i = 0; i < Operators.GetChildCount(); i++)
        {
            try  // Options has children that are not buttons
            {
                
                Button child = Operators.GetChild<Button>(i);
                child.Disabled = false;
                if (child.Pressed)
                {
                    string effect;
                    if (Effect.Text == "")
                    {
                        effect = child.Text;
                    }
                    else if (child.Text == "{" || child.Text == "}")
                    {
                        effect = Effect.Text +'\n'+child.Text+'\n';
                    }
                    else if (child.Text == "if" || child.Text == "else if" || child.Text == "else")
                    {
                        effect = Effect.Text +'\n'+child.Text;
                    }
                    else if (child.Text == "(")
                    {
                        effect = Effect.Text +" "+child.Text;
                    }
                    else if (child.Text == ")")
                    {
                        effect = Effect.Text + " " +child.Text;
                    }
                    else if (operators.Contains(child.Text))
                    {
                        effect = Effect.Text +" "+child.Text+" ";
                    }
                    else
                    {
                        effect = Effect.Text + child.Text;
                    }
                    if (child.Text == "type number:")
                    {
                        if (number.Text != "number")
                        {
                            effect = Effect.Text + " " +number.Text;
                        }
                        else
                        {
                            effect = Effect.Text;
                        }
                    }
                    Effect.Text = effect;
                    lastPressed = child.Text;
                    child.Disabled = true;
                } 
            }
            catch (System.InvalidCastException){}
        }

        // Properties buttons
        Label Properties = GetNode<Label>("Options/Properties");
        for (int i = 0; i < Properties.GetChildCount(); i++)
        {
            try  // Properties has children that are not buttons
            {
                Button child = Properties.GetChild<Button>(i);
                child.Disabled = false;
                if (child.Pressed)
                {
                    string effect;
                    if (Effect.Text == "")
                    {
                        effect = " "+child.Text;
                    }
                    else if (operators.Contains(lastPressed))
                    {
                        effect = Effect.Text +" "+ child.Text;
                    }
                    else if (lastPressed == "}")
                    {
                        effect = Effect.Text + child.Text;
                    }
                    else if (lastPressed == "{")
                    {
                        effect = Effect.Text + "   "+child.Text;
                    }
                    else 
                    {
                        effect = Effect.Text +"."+child.Text;
                    }
                    Effect.Text = effect;
                    lastPressed = child.Text;
                    child.Disabled = true;
                } 
            }
            catch (System.InvalidCastException){}
        }
        
        // Type of card
        Label Types = GetNode<Label>("Code/Types");
        string Type = "";
        for (int i = 0; i < Types.GetChildCount(); i++)
        {
            try
            {
                CheckBox type = Properties.GetChild<CheckBox>(i);
                if (type.Pressed)
                {
                    Type = type.Text;
                }
            }
            catch (System.InvalidCastException){}
        }

        // Trap Card
        CheckBox IsTrap = GetNode<CheckBox>("Code/IsTrap");

        // Select Image
        Button SelectFile = GetNode<Button>("Code/Label5");
        FileDialog SelectWindow = GetNode<FileDialog>("Code/Label5/FileDialog");
        string imgPath = "";
        if (SelectFile.Pressed)
        {
            SelectWindow.Popup_();
            imgPath = SelectWindow.CurrentPath;
        }


        // Compile Button
        Button Compile = GetNode<Button>("Code/Compile");
        Sprite PreviewRelic = GetNode<Sprite>("Preview/Relic");
        TextEdit Name = GetNode<TextEdit>("Code/Label/Name");
        TextEdit Description = GetNode<TextEdit>("Code/Label2/Description");

        if (Compile.Pressed)
        {
            // Updating preview
            Label name = (Label)PreviewRelic.GetChild(0);
            Sprite img = (Sprite)PreviewRelic.GetChild(1);
            img.Scale = new Vector2((float)0.40, (float)0.40);
            Label description = (Label)PreviewRelic.GetChild(2);

            ImageTexture image = new ImageTexture();
            image.Load(SelectWindow.CurrentPath);

            img.Texture = image;
            name.Text = Name.Text;
            description.Text = Description.Text;  
            Compile.Disabled = true;
        }
        Compile.Disabled = false;

        // Save Button
        Button Save = GetNode<Button>("Code/Compile/Save");
        if (Save.Pressed)
        {
            GetTree().ChangeScene("res://mainMenu.tscn");

            //Creating card in game's logic
            gameEngine.Player defaultplayer = new gameEngine.Player(default);
            gameEngine.Relics Relic =new gameEngine.Relics(defaultplayer, defaultplayer, gameVisual.mainMenu.Inventary.CardsInventary.Count, 
                                                            Name.Text, 0, 0, imgPath, IsTrap.Pressed, Type, Effect.Text, Description.Text);
            gameVisual.mainMenu.Inventary.CardsInventary.Add(Relic);
            GD.Print();
        }
    }
}
