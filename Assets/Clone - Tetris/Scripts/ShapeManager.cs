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
		
		private Shape _currentShape;
		private Shape _nextShape;
		private GridXY<bool> _grid;

		private void Awake()
		{
			_grid = GameObject.FindObjectOfType<SampleGridXY>().GetComponent<GridXY<bool>>();
		}

		public void RotateShape(Shape shape)
		{
			Transform transformToRotate = shape.GetComponent<Transform>();
			transformToRotate.Rotate(_rotateAmount);
		}

		public void MoveShape(Block[] blocks, Vector2 direction)
		{
			if (CanMove(blocks, direction) == false)
				return;

			foreach (Block block in blocks)
			{
				block.transform.position += (Vector3)direction;
				block.Column = (int)transform.position.x;
				block.Row = (int)transform.position.y;
			}
		}

		private bool CanMove(Block[] blocks, Vector2 direction)
		{
			if (direction == Vector2.down)
			{
				float y = blocks.OrderBy(x => x.transform.position.y).Reverse().First().transform.position.y;
				blocks = blocks.Where(b => b.transform.position.y == y).ToArray();
			}
			else if (direction == Vector2.left)
			{
				float x = blocks.OrderBy(x => x.transform.position.x).First().transform.position.x;
				blocks = blocks.Where(b => b.transform.position.x == x).ToArray();
			}
			else if (direction == Vector2.right)
			{
				float x = blocks.OrderBy(b => b.transform.position.x).Reverse().First().transform.position.x;
				blocks = blocks.Where(b => b.transform.position.x == x).ToArray();
			}
			else
			{
				return false;
			}

			foreach (Block block in blocks)
			{
				if (_grid.GetElement(block.Column + (int)direction.x, block.Row + (int)direction.y))
					return false;
			}

			return true;
		}

		public void PlaceShape(Shape shape)
		{
			Block[] blocks = shape.Blocks;

			foreach (Block block in blocks) 
			{
				block.transform.parent = null;
				_grid.SetElement(block.Column, block.Row, true);
			}

			Destroy(shape);
		}
	}
}