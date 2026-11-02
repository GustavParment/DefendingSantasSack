using Godot;

public partial class Player : Sprite2D
{
    // [Export] gör att värdet syns och kan ändras i Granskaren
    [Export] public float Speed = 300f;

    // _Process körs en gång per bildruta. delta = sekunder sedan förra bildrutan
    public override void _Process(double delta)
    {
        // Ger en riktning (-1 till 1) utifrån piltangenterna
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Position += direction * Speed * (float)delta;
    }
}