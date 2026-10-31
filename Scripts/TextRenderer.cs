using Godot;

public partial class TextRenderer : Label
{
	public void Create(Vector3 globalPosition, string text, Color color, int size)
	{
		pos = globalPosition;
		Text = text;
		LabelSettings.FontColor = color;
		LabelSettings.FontSize = size;
		ResetSize();
	}

	Vector3 pos;

	Vector2 center;
	float maxDistance2D;
	const float maxDistance3D = 20f;

	public override void _Ready()
	{
		center = GetViewportRect().Size / 2f;
		maxDistance2D = center.Length();
	}
	public override void _Process(double delta)
	{
		var pos2D = CameraBackend.Camera.UnprojectPosition(pos);
		Position = pos2D - (Size / 2f);
		var dot = CameraBackend.CameraDirection.Dot(pos - CameraBackend.CameraPosition);
		if (dot < 0f)
		{
			Visible = false;
			return;
		}
		else Visible = true;

		var ratio = Mathf.Min(pos2D.DistanceTo(center) / maxDistance2D, 1f);
		ratio += Mathf.Min(pos.DistanceTo(CameraBackend.CameraPosition) / maxDistance3D, 1f);
		LabelSettings.FontColor = new Color(LabelSettings.FontColor, ratio);
	}
}
