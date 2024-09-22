using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
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

		public void MoveShape(Shape shape, Vector2 direction)
		{
			Block[] blocks = shape.Blocks;

			if (CanMove(shape, direction) == false)
				return;

			foreach (Block block in blocks)
			{
				block.transform.position += (Vector3)direction;
			}
		}

		private bool CanMove(Shape shape, Vector2 direction)
		{
			
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