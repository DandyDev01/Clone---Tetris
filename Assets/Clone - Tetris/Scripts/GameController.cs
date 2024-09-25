using Grid;
using UnityEngine;

namespace Tetris
{
	public class GameController : MonoBehaviour
	{
		private const float _tickRate = 0.3f;
		private const float _placeShapeTime = 0.4f;

		private PlayerInput _playerInput;
		private Timer _tickTimer;
		private Timer _nextShapeTimer;
		private ShapeManager _shapeManager;
		private GridXY<bool> _grid;

		private void Awake()
		{
			_shapeManager = GameObject.FindAnyObjectByType<ShapeManager>();

			_nextShapeTimer = new Timer(_placeShapeTime, false);

			_nextShapeTimer.OnTimerEnd += PlaceShape;

			_tickTimer = new Timer(_tickRate, true);
			_tickTimer.OnTimerEnd += HandleTick;

			_playerInput = new PlayerInput();
			_playerInput.Enable();
		}

		private void Start()
		{
			_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<SampleGridXY>().Grid;
		}

		private void Update()
		{
			_tickTimer.Tick(Time.deltaTime);
			_nextShapeTimer.Tick(Time.deltaTime);

			Vector2 direction = _playerInput.KeyboardMouse.Move.ReadValue<Vector2>();

			if (direction != Vector2.up && direction != Vector2.zero)
				_shapeManager.MoveShape(_shapeManager.CurrentShape.Blocks, direction);

			if (_playerInput.KeyboardMouse.Rotate.ReadValue<float>() != 0)
				_shapeManager.RotateShape(_shapeManager.CurrentShape);
		}

		private void HandleTick()
		{
			bool canMove = _shapeManager.MoveShape(_shapeManager.CurrentShape.Blocks, Vector2.down);

			_tickTimer.Reset(_tickRate, true);

			if (canMove)
				return;

			_nextShapeTimer.Play();
		}

		private void PlaceShape()
		{
			_nextShapeTimer.Stop();
			_nextShapeTimer.Reset(_placeShapeTime, false);
			_shapeManager.PlaceShape(_shapeManager.CurrentShape);
		}
	}
}