using Godot;

public partial class CameraBackend : Camera3D
{
	public static Vector3 CameraPosition { get; private set; } = Vector3.Zero;
	public static CameraBackend Camera { get; private set; }
	public static Vector3 CameraDirection { get; private set; } = Vector3.Forward;
	public const float CameraRatio = 0.5f;
	public static float CameraUpRatio
	{
		get
		{
			if (Tilt > 0f)
				return Mathf.Lerp(CameraRatio, CameraTiltRatio, Tilt);
			else return CameraRatio;
		}
	}
	public static float CameraDownRatio
	{
		get
		{
			if (Tilt < 0f)
				return Mathf.Lerp(CameraRatio, CameraTiltRatio, -Tilt);
			else return CameraRatio;
		}
	}
	public const float CameraTiltRatio = 1.2f;
	public static float Tilt = 0f;

	public override void _Ready()
	{
		Camera = this;
		Update();
	}

	public override void _Process(double delta) => Update();

	void Update()
	{
		CameraPosition = GlobalPosition;
		CameraDirection = -GlobalBasis.Z;
	}
}
