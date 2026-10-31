using Godot;
using System;
using System.Collections.Generic;

public partial class DnaBuilder : Node3D
{
	List<DnaSegment> segments = new();
	List<(Molecule center, Molecule edge, Node3D parent)> molecules = new();
	int lastLower;
	int lastUpper;
	int listOffset = 0;
	Random rnd = new();


	const float Offset = 3.35f;
	const float RotationalOffset = -26f;

	public override void _Process(double delta)
	{
		var index = Mathf.RoundToInt(CameraBackend.CameraPosition.Y / Offset);
		var distance = new Vector2(CameraBackend.CameraPosition.X, CameraBackend.CameraPosition.Z).Length();
		int upper = index + (int)Mathf.Ceil(CameraBackend.CameraUpRatio * distance / Offset * 2f) + 2;
		int lower = index - (int)Mathf.Floor(CameraBackend.CameraDownRatio * distance / Offset * 2f) - 2;
		Build(lower, upper);
	}

	void Build(int lower, int upper)
	{
		if (segments.Count == 0)
		{
			molecules.Add(Create(lower));
			lastLower = lower;
			lastUpper = lower;
		}

		if (lastLower < lower)
		{
			while (lastLower < lower)
			{
				Dispose(lastLower);
				lastLower++;
			}
		}
		else if (lastLower > lower)
		{
			while (lastLower > lower)
			{
				lastLower--;
				molecules.Insert(0, Create(lastLower));
			}
		}

		if (lastUpper > upper)
		{
			while (lastUpper > upper)
			{
				Dispose(lastUpper);
				lastUpper--;
			}
		}
		else if (lastUpper < upper)
		{
			while (lastUpper < upper)
			{
				lastUpper++;
				molecules.Add(Create(lastUpper));
			}
		}
	}
	(Molecule center, Molecule edge, Node3D parent) Create(int index)
	{
		var segment = GetSegment(index);
		var parent = AssetManager.Empty.Instantiate<Node3D>();
		AddChild(parent);
		parent.Position = new Vector3(0f, index * Offset, 0f);
		parent.RotationDegrees = new Vector3(0f, index * RotationalOffset, 0f);
		var edge = new Molecule(AssetManager.Edgemol);
		var center = new Molecule(AssetManager.GetMolecule(segment));

		edge.Create(parent, true);
		center.Create(parent, true);

		return (center, edge, parent);
	}
	void Dispose(int index)
	{
		index -= lastLower;
		molecules[index].center.Dispose();
		molecules[index].edge.Dispose();
		molecules[index].parent.Free();
		molecules.RemoveAt(index);
	}

	DnaSegment GetSegment(int index)
	{
		index += listOffset;
		if (index >= segments.Count) AddUpper();
		else if (index < 0)
		{
			AddLower();
			index += 256;
		}
		return segments[index];
	}

	void AddUpper()
	{
		segments.AddRange(GenerateChunk());
	}
	void AddLower()
	{
		segments.InsertRange(0, GenerateChunk());
		listOffset += 256;
	}
	DnaSegment[] GenerateChunk()
	{
		var buffer = new DnaSegment[256];
		for (int i = 0; i < buffer.Length; i++)
		{
			buffer[i] = (DnaSegment)rnd.Next(4);
		}
		return buffer;
	}
}
public enum DnaSegment : byte
{
	A,
	B,
	C,
	D
}
