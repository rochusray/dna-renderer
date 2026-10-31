using System;
using System.Collections.Generic;
using Godot;

public partial class AssetManager : Node
{
	[Export]
	public PackedScene sphere, line, doubleLine, zLine, textOverlay, empty;
	[Export]
	public BaseMaterial3D h, o, c, p, n;
	[Export(PropertyHint.MultilineText)]
	public string Molecules;

	public override void _EnterTree() => Init();

	public void Init()
	{
		Sphere = sphere;
		Line = line;
		DoubleLine = doubleLine;
		ZLine = zLine;
		TextOverlay = textOverlay;
		H = h;
		O = o;
		C = c;
		P = p;
		N = n;
		Empty = empty;

		var splitted = Molecules.Split("###");
		var molecules = new List<Molecule>();
		foreach (var part in splitted)
		{
			if (string.IsNullOrWhiteSpace(part)) continue;

			molecules.Add(new(part));
		}

		foreach (var mol in molecules)
		{
			if (mol.Name == "A") Amol = mol;
			else if (mol.Name == "B") Bmol = mol;
			else if (mol.Name == "C") Cmol = mol;
			else if (mol.Name == "D") Dmol = mol;
			else if (mol.Name == "Edge") Edgemol = mol;
		}

		AtomType.Init();
	}

	public static PackedScene Sphere, Line, DoubleLine, ZLine, TextOverlay, Empty;
	public static BaseMaterial3D H, O, C, P, N;
	public static Molecule Amol, Bmol, Cmol, Dmol, Edgemol;

	public static Molecule GetMolecule(DnaSegment segment)
	{
		return segment switch
		{
			DnaSegment.B => Amol,
			DnaSegment.A => Bmol,
			DnaSegment.C => Cmol,
			DnaSegment.D => Dmol,
			_ => throw new NotImplementedException()
		};
	}
}
