using Grid;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Tetris
{
	public class GameController : MonoBehaviour
	{
		private const float _tickRate = 0.3f;
		private const float _placeShapeTime = 0.4f;
		private const int _gameOverRow = 23;

		[SerializeField] private TextMeshProUGUI _pointsTextBox;
		[SerializeField] private GameObject _gameOverPanel;
		[SerializeField] private GameObject _mainMenuPanel;

		private NextShapePreview _nextShapePreview;
		private PlayerInput _playerInput;
		private Timer _tickTimer;
		private Timer _nextShapeTimer;
		private ShapeManager _shapeManager;
		private GridXY<bool> _grid;
		private int _points = 0;

		private void Awake()
		{
			_shapeManager = GameObject.FindObjectOfType<ShapeManager>();
			_nextShapePreview = GameObject.FindObjectOfType<NextShapePreview>();

			_nextShapeTimer = new Timer(_placeShapeTime, false);

			_nextShapeTimer.OnTimerEnd += PlaceShape;

			_tickTimer = new Timer(_tickRate, false);
			_tickTimer.OnTimerEnd += HandleTick;

			_playerInput = new PlayerInput();
			_playerInput.Enable();

			_shapeManager.OnPlaceShape += _nextShapePreview.UpdatePreview;

			_pointsTextBox.text = "Score: " + _points.ToString();

			_gameOverPanel.SetActive(false);
			_mainMenuPanel.SetActive(true);
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
			
			//if (canMove && _nextShapeTimer.IsPlaying)
			//	_nextShapeTimer.Reset(_placeShapeTime, false);

			if (canMove == false)
				_nextShapeTimer.Play();
		}

		/// <summary>
		/// Places a shape into the game world.
		/// </summary>
		private void PlaceShape()
		{
			Block[] blocks = _shapeManager.CurrentShape.Blocks;
			
			_nextShapeTimer.Stop();
			_nextShapeTimer.Reset(_placeShapeTime, false);
			_shapeManager.PlaceShape(_shapeManager.CurrentShape.Blocks);
			_shapeManager.SetCurrentShapeToNextShape();

			StartCoroutine(DestroyAndMoveBlocks(blocks));

			for (int i = 0; i < _grid.Columns; i++) 
			{
				if (_grid.GetElement(i, _gameOverRow))
					EndGame();
			}
		}

		private void EndGame()
		{
			_tickTimer.Stop();
			_gameOverPanel.SetActive(true);
		}

		/// <summary>
		/// Destroys all blocks on the a filled row and moves all rows down one.
		/// </summary>
		private IEnumerator DestroyAndMoveBlocks(Block[] blocks)
		{
			if (blocks.Length == 0)
				throw new System.Exception("Blocks cannot be empty");

			// get unique items ordered by desending row
			blocks = blocks
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
						_points += 100;
						_pointsTextBox.text = "Score: " + _points.ToString();
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

		public void Play()
		{
			_tickTimer.Play();
			_mainMenuPanel.SetActive(false);
		}

		public void Reload()
		{
			SceneManager.LoadScene(0);
		}

		public void Quit()
		{
			Application.Quit();
		}
	}
}