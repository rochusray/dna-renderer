using System;
using Godot;
using Godot.Collections;

public partial class LineRenderer : Node3D
{
	Vector3 target;
	public override void _Process(double delta) => Rotate();

	public void Move(Vector3 a, Vector3 b)
	{
		a = ToGlobal(a);
		b = ToGlobal(b);

		const float size = 0.25f;

		GlobalPosition = (a + b) / 2f;
		Scale = new(size, size, a.DistanceTo(b) / 2f);
		target = b;
		Rotate();
	}

	void Rotate()
	{
		LookAt(target, (CameraBackend.CameraPosition - GlobalPosition).Normalized());
	}
}
