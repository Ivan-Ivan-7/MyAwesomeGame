using Godot;
using System.Linq;

public partial class Map : Node2D
{
	private TileMap TileMap => GetNode<TileMap>("TileMap");

	public MapCell[] GetCells => TileMap.GetUsedCells(0).Select(c => new MapCell(c.X, c.Y)).ToArray();

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void Test(double delta) 
	{
		
	}

}

public struct MapCell
{
    public MapCell(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }

	public int Y { get; set; }
}