using System;
using Godot;

public class LineType
{
    public LineType(Type type)
    {
        this.type = type;
    }

    public readonly Type type;
    public PackedScene Obj
    {
        get
        {
            return type switch
            {
                Type.Single => AssetManager.Line,
                Type.Double => AssetManager.DoubleLine,
                Type.Neigbor => AssetManager.Line,
                Type.Dotted => AssetManager.ZLine,
                _ => throw new NotImplementedException()
            };
        }
    }

    public enum Type
    {
        Single,
        Double,
        Neigbor,
        Dotted
    }
}
