using Godot;
using System;

public class Editor : Node
{
    string lastPressed = "";

    public override void _Ready()
    {
        GetTree().SetScreenStretch(SceneTree.StretchMode.Mode2d, SceneTree.StretchAspect.Keep, new Vector2(1920, 1080), 1);
    }
    public override void _Process(float delta)
    {
        Button Compile = GetNode<Button>("Code/Compile");
        Sprite PreviewRelic = GetNode<Sprite>("Preview/Relic");
        TextEdit Name = GetNode<TextEdit>("Code/Label/Name");
        TextEdit Description = GetNode<TextEdit>("Code/Label2/Description");
        Label name = (Label)PreviewRelic.GetChild(0);
        Label description = (Label)PreviewRelic.GetChild(1);

        if (Compile.Pressed)
        {

            name.Text = Name.Text;
            description.Text = Description.Text;
        }


        Button Save = GetNode<Button>("Code/Compile/Save");
        if (Save.Pressed)
        {
            GetTree().ChangeScene("res://mainMenu.tscn");
        }
        TextEdit Effect = GetNode<TextEdit>("Code/Label4/Effect");
        Label Options = GetNode<Label>("Options/Operators");
        for (int i = 0; i < Options.GetChildCount(); i++)
        {
            try  // Options has children that are not buttons
            {
                Button child = Options.GetChild<Button>(i);
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
                    else if (child.Text == ">" || child.Text == ">=" || child.Text == "<" || child.Text == "<=" || child.Text == "=="  || child.Text == "&&" || child.Text == "| |" ||child.Text == "+" || child.Text == "-" || child.Text == "*" || child.Text == "/" || child.Text == "%")
                    {
                        effect = Effect.Text +" "+child.Text+" ";
                    }
                    else
                    {
                        effect = Effect.Text + child.Text;
                    }
                    Effect.Text = effect;
                    lastPressed = child.Text;
                    child.Disabled = true;
                } 
            }
            catch (System.InvalidCastException){}
        }

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
                    else if (lastPressed == ">" || lastPressed == ">=" || lastPressed == "<" || lastPressed == "<=" || lastPressed == "=="  || lastPressed == "&&" || lastPressed == "| |" ||lastPressed == "+" || lastPressed == "-" || lastPressed == "*" || lastPressed == "/" || lastPressed == "%")
                    {
                        effect = Effect.Text +" "+ child.Text;
                    }
                    else if (lastPressed == "}")
                    {
                        effect = Effect.Text + child.Text;
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
    }
}
