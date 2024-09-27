using Grid;
using System.Linq;
using UnityEngine;

namespace Tetris
{
	public class ShapeManager : MonoBehaviour
	{
		private readonly Vector3Int _rotateAmount = new Vector3Int(0, 0, 90);

		[SerializeField] private Vector2Int _spawnCell = new Vector2Int(6, 23);
		[SerializeField] private Shape[] _shapes;

		private Shape _currentShape;
		private Shape _nextShape;
		private GridXY<bool> _grid;

		public Shape CurrentShape => _currentShape;
		public Shape NextShape => _nextShape;

		private void Start()
		{
			_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<SampleGridXY>().Grid;
			
			Vector3 worldPosition = _grid.GetWorldPosition(_spawnCell.x, _spawnCell.y);
			
			_currentShape = Instantiate(_shapes.RandomElement(), worldPosition, Quaternion.identity);
			_currentShape.Init(_grid, _spawnCell.x, _spawnCell.y);

			_nextShape = _shapes.RandomElement();
		}

		/// <summary>
		/// Rotate a shape.
		/// </summary>
		/// <param name="shape">The shape to be rotated.</param>
		public void RotateShape(Shape shape)
		{
			shape.transform.Rotate(_rotateAmount);

			Block[] blocks = shape.Blocks;

			foreach (Block block in blocks) 
			{
				Vector3Int check = _grid.GetCellPosition(block.transform.position);

				// rotating the shape can move a block out of the grid, 
				// this moves the block back into the grid.
				if (_grid.IsInRange(block.transform.position) == false)
				{
					if (block.transform.position.x < _grid.GetWorldPosition(_grid.Columns / 2, 0).x)
					{
						shape.transform.position += Vector3.right * Mathf.Abs((block.transform.position.x - _grid.GetWorldPosition(0, 0).x));
					}
					else if (block.transform.position.x >= _grid.GetWorldPosition(_grid.Columns / 2, 0).x) 
					{
						shape.transform.position += Vector3.left * Mathf.Abs((block.transform.position.x - _grid.GetWorldPosition(_grid.Columns-1, 0).x));
					}
					else if (block.transform.position.y < _grid.GetWorldPosition(0, _grid.Rows / 2).y)
					{
						shape.transform.position += Vector3.up * Mathf.Abs(block.transform.position.y - _grid.GetWorldPosition(0, 0).y);
					}
					else if (block.transform.position.y >= _grid.GetWorldPosition(0, _grid.Rows / 2).y)
					{
						shape.transform.position += Vector3.down * Mathf.Abs(block.transform.position.y - _grid.GetWorldPosition(0, 0).y);
					}
				}

				Vector3Int cell = _grid.GetCellPosition(block.transform.position);
				block.Column = cell.x;
				block.Row = cell.y;
			}
		}

		/// <summary>
		/// Moves the blocks that make up a shape in the specified direction.
		/// </summary>
		/// <param name="blocks">Blocks that make up the shape to move.</param>
		/// <param name="direction">Direction to move the shape.</param>
		/// <exception cref="System.Exception">Throws when the blocks passed do not have a parent. 
		/// It is expected that all blocks share the same parent</exception>
		/// <returns>Wheather or not the shape moved</returns>
		public bool MoveShape(Block[] blocks, Vector2 direction)
		{
			if (CanMove(blocks, direction) == false || blocks.Length == 0)
				return false;

			foreach (Block block in blocks)
			{
				block.Column += (int)direction.x;
				block.Row += (int)direction.y;
			}

			if (blocks[0].transform.parent is null)
				throw new System.Exception("Blocks are missing a parent.");

			blocks[0].transform.parent.position += (Vector3)direction * _grid.CellSize;

			return true;
		}

		/// <summary>
		/// Checks if all blocks can move in the specified direction.
		/// </summary>
		/// <param name="blocks">Blocks to to check for valid movement</param>
		/// <param name="direction">Direction the blocks are trying to move.</param>
		/// <returns>Weather or not all blocks can move in the specified direction.</returns>
		private bool CanMove(Block[] blocks, Vector2 direction)
		{
			foreach (Block block in blocks)
			{
				if (_grid.IsInRange(block.Column + (int)direction.x, block.Row + (int)direction.y) == false)
					return false;

				if (_grid.GetElement(block.Column + (int)direction.x, block.Row + (int)direction.y))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Places the shape into the playspace.
		/// </summary>
		/// <param name="shape">Shape to place in the playspave</param>
		public void PlaceShape(Block[] blocks)
		{
			if (blocks.Length == 0) 
				return;

			GameObject parent = blocks.First().transform.parent.gameObject;
			
			foreach (Block block in blocks)
			{
				block.transform.parent = null;
				_grid.SetElement(block.Column, block.Row, true);
			}

			Destroy(parent);
		}

		public void SetCurrentShapeToNextShape()
		{
			Vector3 worldPosition = _grid.GetWorldPosition(_spawnCell.x, _spawnCell.y);

			_currentShape = Instantiate(_nextShape, worldPosition, Quaternion.identity);
			_currentShape.Init(_grid, _spawnCell.x, _spawnCell.y);

			_nextShape = _shapes.RandomElement();
		}
	}
}