using System;
using System.Collections.Generic;
using System.Globalization;
using Godot;

public class Molecule
{
	public Molecule(Molecule molecule)
	{
		Name = molecule.Name;
		Atoms = new Atom[molecule.Atoms.Length];
		Lines = new Line[molecule.Lines.Length];
		Markers = new Marker[molecule.Markers.Length];

		for (int i = 0; i < Atoms.Length; i++)
		{
			Atoms[i] = new Atom(molecule.Atoms[i]);
		}
		for (int i = 0; i < Lines.Length; i++)
		{
			Lines[i] = new Line(molecule.Lines[i]);
		}
		for (int i = 0; i < Markers.Length; i++)
		{
			Markers[i] = new Marker(molecule.Markers[i]);
		}
	}
	public Molecule(string text)
	{
		var textLines = text.Split('\n');

		var lines = new List<(int a, int b, LineType.Type type)>();
		var atoms = new List<Atom>();
		var markers = new List<Marker>();
		var indexes = new List<int>();

		var current = 0;
		foreach (var line in textLines)
		{
			if (string.IsNullOrWhiteSpace(line)) continue;

			var splitted = line.Split(';');
			if (splitted[0] == "Name")
			{
				Name = splitted[1];
			}
			else if (splitted[0] == "-")
			{
				lines.Add((int.Parse(splitted[1]), int.Parse(splitted[2]), LineType.Type.Single));
				indexes.Add(current);
			}
			else if (splitted[0] == "=")
			{
				lines.Add((int.Parse(splitted[1]), int.Parse(splitted[2]), LineType.Type.Double));
				indexes.Add(current);
			}
			else if (splitted[0] == "~")
			{
				lines.Add((int.Parse(splitted[1]), int.Parse(splitted[2]), LineType.Type.Dotted));
				indexes.Add(current);
			}
			else if (splitted[0] == "/")
			{
				lines.Add((int.Parse(splitted[1]), int.Parse(splitted[2]), LineType.Type.Neigbor));
				indexes.Add(current);
			}
			else if (splitted[0] == "#")
			{
				var markerText = splitted[1];
				var position = new Vector3(ParseFloat(splitted[2]), ParseFloat(splitted[3]), ParseFloat(splitted[4]));
				var scale = new Vector3(ParseFloat(splitted[5]), ParseFloat(splitted[6]), ParseFloat(splitted[7]));
				markers.Add(new(markerText, position, scale, new Color(1f, 1f, 1f), 30));
				indexes.Add(current);
			}
			else
			{
				var atomType = AtomType.Parse(splitted[0]);
				var pos = new Vector3(ParseFloat(splitted[1]), ParseFloat(splitted[2]), ParseFloat(splitted[3]));
				atoms.Add(new(atomType, pos));
				indexes.Add(current);
				current++;
			}
		}

		Atoms = atoms.ToArray();
		Markers = markers.ToArray();
		Lines = new Line[lines.Count];


		for (int i = 0; i < Lines.Length; i++)
		{
			Lines[i] = new Line(lines[i].type, Atoms[indexes[lines[i].a]].Position, Atoms[indexes[lines[i].b]].Position);
		}
		static float ParseFloat(string str)
		{
			return float.Parse(str.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
		}
	}
	~Molecule()
	{
		Dispose();
	}

	public readonly string Name;
	public readonly Atom[] Atoms;
	public readonly Line[] Lines;
	public readonly Marker[] Markers;

	public void Create(Node3D parent, bool createNeigbors)
	{
		foreach (var atom in Atoms)
		{
			atom.CreateObject(parent);
		}
		foreach (var line in Lines)
		{
			if (line.Type.type == LineType.Type.Neigbor && !createNeigbors) continue;
			line.CreateObject(parent);
		}
		foreach (var marker in Markers)
		{
			marker.CreateObject(parent);
		}
	}
	public void Dispose()
	{
		foreach (var atom in Atoms)
		{
			atom.Dispose();
		}
		foreach (var line in Lines)
		{
			line.Dispose();
		}
		foreach (var marker in Markers)
		{
			marker.Dispose();
		}
	}
}
