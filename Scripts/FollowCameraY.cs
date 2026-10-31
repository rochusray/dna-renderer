using Godot;

public partial class FollowCameraY : Node3D
{
	public override void _Process(double delta)
	{
		Position = new Vector3(Position.X, CameraBackend.CameraPosition.Y, Position.Z);
	}
}
