using System;
using Godot;

public partial class LOD : MeshInstance3D
{
	[Export]
	public Mesh[] Meshes;
	[Export]
	public float[] Distances;
	int last = -2;

	public override void _Ready()
	{
		if (Meshes.Length != Distances.Length + 1) throw new Exception();
		for (int i = 0; i < Distances.Length; i++)
		{
			Distances[i] *= Distances[i];
		}
	}

	public override void _Process(double delta)
	{
		var distance = CameraBackend.CameraPosition.DistanceSquaredTo(GlobalPosition);
		for (int i = 0; i < Distances.Length; i++)
		{
			if (Distances[i] > distance)
			{
				if (last != i)
				{
					Mesh = Meshes[i];
					last = i;
				}
				return;
			}
		}
		if (last != -1)
		{
			last = -1;
			Mesh = Meshes[^1];
		}
	}
}
