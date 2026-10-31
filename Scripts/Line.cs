using Godot;

public class Line
{
	public Line(Line line)
	{
		Type = new LineType(line.Type.type);
		PosA = line.PosA;
		PosB = line.PosB;
	}
	public Line(LineType.Type type, Vector3 posA, Vector3 posB)
	{
		Type = new(type);
		PosA = posA;
		PosB = posB;
	}
	~Line()
	{
		Dispose();
	}
	public readonly LineType Type;
	public readonly Vector3 PosA, PosB;

	private Node3D obj;

	public void CreateObject(Node3D parent)
	{
		obj = Type.Obj.Instantiate<Node3D>();
		parent.AddChild(obj);
		var script = (LineRenderer)obj;
		script.Move(PosA, PosB);
	}
	public void Dispose()
	{
		obj?.Free();
		obj = null;
	}
}
