using UnityEngine;

namespace Grid
{
	public class SampleGridXY : MonoBehaviour
	{
		[SerializeField] private Vector3 oragin = Vector3.zero;
		[SerializeField] private int columns = 1;
		[SerializeField] private int rows = 1;

		[SerializeField]
		[Min(1f)]
		private float cellSize = 1;

		private GridXY<bool> grid;

		public int Columns { get { return columns; } }
		public int Rows { get { return rows; } }
		public float CellSize { get { return cellSize; } }
		public Vector3 Oragin { get { return oragin; } }

		public GridXY<bool> Grid => grid;

		public void Awake()
		{
			grid = new GridXY<bool>(oragin, columns, Rows, CellSize);
		}

		/// <summary>
		/// Gets the world position of the center of the grid
		/// </summary>
		/// <returns></returns>
		public Vector3 GetCenterWordPosition()
		{
			return new Vector3((grid.Oragin.x + grid.Columns * grid.CellSize) / 2,
			(grid.Oragin.y + grid.Rows * grid.CellSize) / 2);
		}

		public Vector3 WorldPosition(int row, int col)
		{
			return grid.GetWorldPosition(col, row);
		}

	}
}
