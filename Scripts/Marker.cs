using Godot;
public class Marker
{
	public Marker(Marker marker)
	{
		Text = marker.Text;
		Position = marker.Position;
		Color = marker.Color;
		Size = marker.Size;
		Scale = marker.Scale;
	}
	public Marker(string text, Vector3 position, Vector3 scale, Color color, int size)
	{
		Text = text;
		Position = position;
		Scale = scale;
		Color = color;
		Size = size;
	}

	public readonly string Text;
	public readonly Vector3 Position;
	public readonly Color Color;
	public readonly int Size;
	public readonly Vector3 Scale;
	Label obj;

	public void CreateObject(Node3D parent)
	{
#warning
		return;
		obj = AssetManager.TextOverlay.Instantiate<Label>();
		parent.AddChild(obj);
		var script = (TextRenderer)obj;
		script.Create(parent.ToGlobal(Position), Text, Color, Size);
	}
	public void Dispose()
	{
		obj?.Free();
		obj = null;
	}
}
