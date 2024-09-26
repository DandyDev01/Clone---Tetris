using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tetris
{
	public class ShapeManager : MonoBehaviour
	{
		private readonly Vector3Int _rotateAmount = new Vector3Int(0, 0, 90);

		[SerializeField] private Shape[] _shapes;

		private Shape _currentShape;
		private Shape _nextShape;
		private GridXY<bool> _grid;

		public Shape CurrentShape => _currentShape;
		public Shape NextShape => _nextShape;

		private void Start()
		{
			_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<SampleGridXY>().Grid;
			
			Vector3 worldPosition = _grid.GetWorldPosition(6, 23);
			
			_currentShape = Instantiate(_shapes.RandomElement(), worldPosition, Quaternion.identity);
			_currentShape.Init(_grid, 6, 23);

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
		/// <returns>Wheather or not the shape moved</returns>
		public bool MoveShape(Block[] blocks, Vector2 direction)
		{
			if (CanMove(blocks, direction) == false)
				return false;

			foreach (Block block in blocks)
			{
				block.Column += (int)direction.x;
				block.Row += (int)direction.y;
			}

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
		public void PlaceShape(Shape shape)
		{
			Block[] blocks = shape.Blocks;

			foreach (Block block in blocks) 
			{
				block.transform.parent = null;
				_grid.SetElement(block.Column, block.Row, true);
			}

			Vector3 worldPosition = _grid.GetWorldPosition(6, 23);

			_currentShape = Instantiate(_nextShape, worldPosition, Quaternion.identity);
			_currentShape.Init(_grid, 6, 23);

			_nextShape = _shapes.RandomElement();

			Destroy(shape);
		}
	}
}