using Godot;

public class Atom
{
	public Atom(Atom atom)
	{
		Type = new AtomType(atom.Type.Type);
		Position = atom.Position;
		Scale = atom.Scale;
	}
	public Atom(AtomType.AtomTypes type, Vector3 position)
	{
		Type = new(type);
		Position = position;
	}
	~Atom()
	{
		Dispose();
	}

	public readonly AtomType Type;
	public readonly Vector3 Position;
	public readonly float Scale;
	Node3D obj;

	public void CreateObject(Node3D parent)
	{
		if (Type.Type == AtomType.AtomTypes.Virtual) return;

		obj = AssetManager.Sphere.Instantiate<Node3D>();
		parent.AddChild(obj);
		obj.Position = Position;
		obj.Scale *= Type.Scale;
		var child = obj.GetChild<MeshInstance3D>(0);
		child.MaterialOverride = Type.Material;
	}
	public void Dispose()
	{
		obj?.Free();
		obj = null;
	}
}
