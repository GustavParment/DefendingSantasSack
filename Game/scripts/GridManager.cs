using Godot;

// Håller koll på rutnätet: vilka rutor som har torn, var start och mål är,
// och ritar ut allt på skärmen.
public partial class GridManager : Node2D
{
    [Export] public int Columns = 20;
    [Export] public int Rows = 12;
    [Export] public int CellSize = 48;

    // Där fienderna kommer in och tomtens säck som de vill åt
    [Export] public Vector2I StartCell = new Vector2I(0, 6);
    [Export] public Vector2I GoalCell = new Vector2I(19, 6);

    // Färger (kan ändras i Granskaren)
    [Export] public Color SnowColor = new Color(0.92f, 0.96f, 1.0f);
    [Export] public Color TowerColor = new Color(0.75f, 0.1f, 0.15f);
    [Export] public Color StartColor = new Color(0.2f, 0.6f, 0.3f);
    [Export] public Color GoalColor = new Color(0.95f, 0.75f, 0.2f);
    [Export] public Color LineColor = new Color(0.7f, 0.8f, 0.9f);
    [Export] public Color HoverColor = new Color(1f, 1f, 1f, 0.35f);

    // true = ett torn står på rutan. _occupied[x, y]
    private bool[,] _occupied;

    // Rutan som musen är över just nu
    private Vector2I _hoverCell = new Vector2I(-1, -1);

    public override void _Ready()
    {
        _occupied = new bool[Columns, Rows];
    }

    public override void _Process(double delta)
    {
        // Rita bara om när musen flyttat till en ny ruta
        Vector2I cell = WorldToCell(GetGlobalMousePosition());
        if (cell != _hoverCell)
        {
            _hoverCell = cell;
            QueueRedraw();
        }
    }

    // Anropas när spelaren klickar, trycker på en tangent osv.
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventMouseButton mouse || !mouse.Pressed)
            return;

        Vector2I cell = WorldToCell(GetGlobalMousePosition());
        if (!IsInsideGrid(cell) || cell == StartCell || cell == GoalCell)
            return;

        if (mouse.ButtonIndex == MouseButton.Left)
            _occupied[cell.X, cell.Y] = true;   // Vänsterklick: bygg
        else if (mouse.ButtonIndex == MouseButton.Right)
            _occupied[cell.X, cell.Y] = false;  // Högerklick: riv

        QueueRedraw();
    }

    // Vilken ruta ligger en punkt i spelvärlden i?
    public Vector2I WorldToCell(Vector2 globalPosition)
    {
        Vector2 local = ToLocal(globalPosition);
        return new Vector2I(Mathf.FloorToInt(local.X / CellSize), Mathf.FloorToInt(local.Y / CellSize));
    }

    // Mitten av en ruta i spelvärlden (fienderna kommer gå hit)
    public Vector2 CellToWorld(Vector2I cell)
    {
        return ToGlobal(new Vector2(cell.X + 0.5f, cell.Y + 0.5f) * CellSize);
    }

    public bool IsInsideGrid(Vector2I cell)
    {
        return cell.X >= 0 && cell.X < Columns && cell.Y >= 0 && cell.Y < Rows;
    }

    public bool IsOccupied(Vector2I cell)
    {
        return _occupied[cell.X, cell.Y];
    }

    // Godot anropar _Draw när noden ska ritas, t.ex. efter QueueRedraw()
    public override void _Draw()
    {
        var cellSize = new Vector2(CellSize, CellSize);

        for (int x = 0; x < Columns; x++)
        {
            for (int y = 0; y < Rows; y++)
            {
                var cell = new Vector2I(x, y);
                var rect = new Rect2(new Vector2(x, y) * CellSize, cellSize);

                Color fill = SnowColor;
                if (cell == StartCell) fill = StartColor;
                else if (cell == GoalCell) fill = GoalColor;
                else if (_occupied[x, y]) fill = TowerColor;

                DrawRect(rect, fill);                    // Fylld ruta
                DrawRect(rect, LineColor, false, 1f);    // Kantlinje
            }
        }

        if (IsInsideGrid(_hoverCell))
        {
            DrawRect(new Rect2(new Vector2(_hoverCell.X, _hoverCell.Y) * CellSize, cellSize), HoverColor);
        }
    }
}
