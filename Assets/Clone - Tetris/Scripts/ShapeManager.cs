using Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
	public class ShapeManager : MonoBehaviour
	{
		private Shape _currentShape;
		private Shape _nextShape;
		private GridXY<bool> _grid;

		private void Awake()
		{
			_grid = GameObject.FindObjectOfType<SampleGridXY>().GetComponent<GridXY<bool>>();
		}

		public void RotateShape(Shape shape)
		{
			throw new System.NotImplementedException();
		}

		public void MoveShape(Shape shape)
		{
			throw new System.NotImplementedException();
		}
	}
}