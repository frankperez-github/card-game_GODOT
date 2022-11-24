using Godot;
using System;

public class Editor : Node
{
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


        Button Save = GetNode<Button>("Code/Save");
        if (Save.Pressed)
        {
            GetTree().ChangeScene("res://mainMenu.tscn");
        }

        TextEdit Effect = GetNode<TextEdit>("Code/Label4/Effect");
        Tree Options = GetNode<Tree>("Options");
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
                    else
                    {
                        effect = Effect.Text + "." + child.Text;
                    }
                    Effect.Text = effect;
                    child.Disabled = true;
                } 
            }
            catch (System.InvalidCastException){}
        }
    }
}
