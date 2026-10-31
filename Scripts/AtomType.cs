using System;
using Godot;

public class AtomType
{
	public static void Init()
	{
		colors = new Color[]
		{
			new(0.5f, 0.5f, 1f),
			new(1f, 0.1f, 0.1f),
			new(0.1f, 0.1f, 0.1f),
			new(1f, 0.7f, 0.1f),
			new(0.1f, 0.1f, 1f)
		};

		isDark = new bool[]
		{
			false,
			true,
			true,
			false,
			false
		};

		scales = new float[]
		{
			0.7f,
			1f,
			1f,
			1f,
			1f
		};

		materials = new BaseMaterial3D[]
		{
			AssetManager.H,
			AssetManager.O,
			AssetManager.C,
			AssetManager.P,
			AssetManager.N
		};
	}

	public AtomType(AtomTypes type)
	{
		Type = type;
	}
	public readonly AtomTypes Type;
	public Color Color => colors[(int)Type];
	public bool IsDark => isDark[(int)Type];
	public float Scale => scales[(int)Type];
	public BaseMaterial3D Material => materials[(int)Type];

	const int Count = 5;
	public enum AtomTypes : byte
	{
		H,
		O,
		C,
		P,
		N,
		Virtual
	}
	static Color[] colors;
	static bool[] isDark;
	static float[] scales;
	static BaseMaterial3D[] materials;

	public static AtomTypes Parse(string name)
	{
		if (name == "H") return AtomTypes.H;
		else if (name == "O") return AtomTypes.O;
		else if (name == "C") return AtomTypes.C;
		else if (name == "P") return AtomTypes.P;
		else if (name == "N") return AtomTypes.N;
		else if (name == "V") return AtomTypes.Virtual;
		else throw new NotImplementedException(name);
	}
}
