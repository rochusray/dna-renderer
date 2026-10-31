using Godot;

public partial class Test : Node3D
{
	[Export]
	public DnaSegment moleculeType = DnaSegment.A;
	[Export]
	public bool createNeigbors = false;

	public override void _Ready()
	{
		var a = AssetManager.GetMolecule(moleculeType);
		var edge = AssetManager.Edgemol;

		a.Create(this, createNeigbors);
		edge.Create(this, createNeigbors);
	}
}
