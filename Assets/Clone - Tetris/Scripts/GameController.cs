using Grid;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
			bool wasPressedThisFrame = _playerInput.KeyboardMouse.Move.WasPressedThisFrame();

			if (direction != Vector2.up && direction != Vector2.zero && wasPressedThisFrame)
				_shapeManager.MoveShape(_shapeManager.CurrentShape.Blocks, direction);

			if (_playerInput.KeyboardMouse.Rotate.WasPressedThisFrame())
				_shapeManager.RotateShape(_shapeManager.CurrentShape);
		}

		private void HandleTick()
		{
			bool canMove = _shapeManager.MoveShape(_shapeManager.CurrentShape.Blocks, Vector2.down);

			_tickTimer.Reset(_tickRate, true);

			if (canMove == false)
				_nextShapeTimer.Play();
		}

		/// <summary>
		/// Places a shape into the game world.
		/// </summary>
		private void PlaceShape()
		{
			_nextShapeTimer.Stop();
			_nextShapeTimer.Reset(_placeShapeTime, false);
			_shapeManager.PlaceShape(_shapeManager.CurrentShape.Blocks);
			_shapeManager.SetCurrentShapeToNextShape();

			StartCoroutine(DestroyAndMoveBlocks());
		}

		/// <summary>
		/// Destroys all blocks on the a filled row and moves all rows down one.
		/// </summary>
		private IEnumerator DestroyAndMoveBlocks()
		{
			Block[] blocks = _shapeManager.CurrentShape.Blocks
				.GroupBy(b => b.Row)
				.Select(g => g.First())
				.OrderBy(x => x.Row)
				.Reverse()
				.ToArray();

			foreach (Block part in blocks)
			{
				if (RowCompleted(part.Row))
				{
					// Remove all blocks on that row.
					for (int column = 0; column < _grid.Columns; column++)
					{
						Vector3 worldPosition = _grid.GetWorldPosition(column, part.Row);
						Collider2D blockCollider = Physics2D.OverlapCircle(worldPosition, 0.3f);

						if (blockCollider is null)
							continue;

						Block block = blockCollider.transform.GetComponent<Block>();

						if (block is null)
							throw new System.Exception("The cell (" + column + ", " + part.Row + ") has a collider without a Block.");

						Destroy(block.gameObject);
						_grid.SetElement(column, part.Row, false);
					
						// TODO: give points
					}

					// Move all blocks above down a row.
					for (int column = 0; column < _grid.Columns; column++)
					{
						GameObject parent = new GameObject("blocksToMoveParent");
						parent.transform.position = _grid.GetWorldPosition(column, 10);

						for (int row = part.Row + 1; row < _grid.Rows; row++)
						{
							if (_grid.GetElement(column, row) == false)
								continue;

							Vector3 worldPosition = _grid.GetWorldPosition(column, row);
							Collider2D blockCollider = Physics2D.OverlapCircle(worldPosition, 0.3f);

							if (blockCollider is null)
								throw new System.Exception("Cell (" + column + ", " + row + ") value is true, but there is no collider.");

							Block block = blockCollider.transform.GetComponent<Block>();

							if (block is null)
								throw new System.Exception("The cell (" + column + ", " + row + ") has a collider without a Block.");

							block.transform.parent = parent.transform;
							_grid.SetElement(column, row, false);
						}

						Block[] blocksToMove = parent.GetComponentsInChildren<Block>();

						_shapeManager.MoveShape(blocksToMove, Vector2.down);
						_shapeManager.PlaceShape(blocksToMove);
						yield return new WaitForEndOfFrame();
					}
				}
			}
		}

		/// <summary>
		/// Check if all columns on the specified row are filled.
		/// </summary>
		/// <param name="row">Row to check the columns of.</param>
		/// <returns>Weather or not all columns on the specified row are filled.</returns>
		private bool RowCompleted(int row)
		{
			for (int column = 0; column < _grid.Columns; column++)
			{
				if (_grid.GetElement(column, row) == false)
				{
					return false;
				}
			}

			return true;
		}
	}
}