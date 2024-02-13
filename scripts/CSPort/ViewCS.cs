using Godot;
using System;

public partial class ViewCS : Node3D
{
	[ExportGroup("Properties")]
	[Export] public Node3D Target;
	
	[ExportGroup("Zoom")]
	[Export] public float MinZoom = 4f;
	[Export] public float MaxZoom = 16f;
	[Export] public float ZoomSpeed = 10f;
	
	[ExportGroup("Rotation")]
	[Export] public float RotationSpeed = 120f;

	private Vector3 _cameraRotation;
	private Camera3D _camera;
	private float _zoom = 10f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_camera = GetNode<Camera3D>("Camera");
		_cameraRotation = RotationDegrees;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (Target == null)
			return;

		Position = Position.Lerp(Target.Position, (float)delta * 4);

		RotationDegrees = RotationDegrees.Lerp(_cameraRotation, (float)delta * 6);

		_camera.Position = _camera.Position.Lerp(new Vector3(0, 0, _zoom), (float)delta * 8);
		
		HandleInput((float)delta);
	}

	private void HandleInput(float delta)
	{
		Vector3 input = Vector3.Zero;

		input.X = Input.GetAxis(GameConstants.INP_AXIS_CAM_UP, GameConstants.INP_AXIS_CAM_DOWN);
		input.Y = Input.GetAxis(GameConstants.INP_AXIS_CAM_LEFT, GameConstants.INP_AXIS_CAM_RIGHT);

		_cameraRotation += input.LimitLength(1.0f) * RotationSpeed * delta;

		_zoom += Input.GetAxis(GameConstants.INP_AXIS_ZOOM_IN, GameConstants.INP_AXIS_ZOOM_OUT) * ZoomSpeed * delta;

		_zoom = Mathf.Clamp(_zoom, MinZoom, MaxZoom);
	}
}
