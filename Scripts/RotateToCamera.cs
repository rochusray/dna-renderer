using Godot;

public partial class RotateToCamera : Node3D
{
	public override void _Ready() => Rotate();

	public override void _Process(double delta) => Rotate();

	void Rotate()
	{
		LookAt(CameraBackend.CameraPosition);
	}
}
