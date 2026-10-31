using Godot;

public partial class CameraController : Node3D
{
	[Export]
	public bool IsMobile;
	[Export]
	public Vector2 Speed;
	[Export]
	public Vector2 MSpeed;
	[Export]
	public Vector2 MaxSpeed;
	[Export]
	public float MaxZoom;
	[Export]
	public float MinZoom;
	[Export]
	public float ZoomSpeed;
	[Export]
	public float MZoomSpeed;
	[Export]
	public Vector2 Drag;
	[Export]
	public float ZoomDrag;
	[Export(PropertyHint.Range)]
	public float SpeedZoomFactor;
	[Export]
	public float DefaultRotation;

	[Export]
	public float TiltSpeed;
	[Export]
	public float MaxTilt;
	[Export]
	public float TiltDrag;
	[Export]
	public float TiltToZeroSpeed;
	Vector2 mouseDelta = Vector2.Zero;
	float scrollDelta = 0f;
	float tiltDelta = 0f;
	float lastTouch = 0f;

	Vector2 currentVel = Vector2.Zero;
	float currentZoomVel = 0f;
	float currentZoom = 1f;
	float currentTilt = 0f;
	float currentTiltSpeed = 0f;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Process(double delta)
	{
		CameraBackend.Tilt = currentTilt / MaxTilt;

		var deltaF = (float)delta;
		var isTouching = lastTouch > 0f;
		if (isTouching)
		{
			lastTouch -= deltaF;
			lastTouch = Mathf.Max(lastTouch, 0f);
		}

		currentVel *= Vector2.One - (Drag * deltaF).Min(Vector2.One);
		currentZoomVel *= 1f - Mathf.Min(ZoomDrag * deltaF, 1f);
		currentTiltSpeed *= 1f - Mathf.Min(TiltDrag * deltaF, 1f);

		Vector2 speed = mouseDelta * 0.0001f;
		float zoom = scrollDelta * 0.0001f;
		var tilt = tiltDelta * 0.001f;

		speed *= IsMobile ? MSpeed : Speed;
		zoom *= IsMobile ? MZoomSpeed : ZoomSpeed;
		tilt *= TiltSpeed;

		currentVel += speed;
		currentZoomVel += zoom;
		currentTiltSpeed += tilt;

		var zoomFactor = Mathf.Remap(currentZoom, MinZoom, MaxZoom, 0f, 1f);
		var zoomCurve = Mathf.Pow(zoomFactor, 2f);

		currentVel = currentVel.Clamp(-MaxSpeed, MaxSpeed);
		currentZoomVel = Mathf.Min(currentZoomVel, MaxZoom);
		currentZoom += currentZoomVel * zoomCurve * 0.7f + currentZoomVel * 0.3f;
		currentZoom = Mathf.Clamp(currentZoom, MinZoom, MaxZoom);

		currentTilt += currentTiltSpeed;
		currentTilt = Mathf.Clamp(currentTilt, -MaxTilt, MaxTilt);

		Position = new Vector3(0f, Position.Y + currentVel.Y * (1f - SpeedZoomFactor) + currentVel.Y * SpeedZoomFactor * zoomFactor, 0f);
		RotationDegrees = new Vector3(currentTilt, RotationDegrees.Y + Mathf.RadToDeg(currentVel.X + (isTouching ? DefaultRotation * deltaF : 0f)), RotationDegrees.Z);
		Scale = new Vector3(currentZoom, 1f, currentZoom);

		if (!isTouching)
			currentTilt *= 1f - (TiltToZeroSpeed * deltaF);

		mouseDelta = Vector2.Zero;
		scrollDelta = 0f;
		tiltDelta = 0f;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			mouseDelta = mouseMotion.Relative;
			lastTouch = 0.1f;
		}
		if (@event is InputEventMouseButton mouseButton)
		{
			if (!IsMobile && mouseButton.IsPressed())
			{
				if (mouseButton.ButtonIndex == MouseButton.WheelUp)
				{
					scrollDelta--;
				}
				else if (mouseButton.ButtonIndex == MouseButton.WheelDown)
				{
					scrollDelta++;
				}
			}
		}
		if (IsMobile && @event is InputEventMagnifyGesture zoomEvent)
		{
			scrollDelta += 1f - zoomEvent.Factor;
			lastTouch = 0.1f;
		}
		if (IsMobile && @event is InputEventPanGesture tiltEvent)
		{
			var angle = tiltEvent.Delta.AngleTo(Vector2.Up) % 180f;
			if (angle < 40f || angle > 140f)
			{
				tiltDelta += tiltEvent.Delta.Y;
			}
			lastTouch = 0.1f;
		}
		if (IsMobile && @event is InputEventScreenTouch touch)
		{
			lastTouch = 0.1f;
		}
		if (!IsMobile && @event is InputEventKey key)
		{
			if (key.CtrlPressed)
			{
				tiltDelta -= 100;
			}
			else if (key.ShiftPressed)
			{
				tiltDelta += 100;
			}
		}
	}
}
