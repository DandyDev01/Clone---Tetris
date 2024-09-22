using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class GridXY<TGridObject> : Grid<TGridObject>
    {
		public GridXY(Vector2 _oragin, int _cols, int _rows, float _cellSize) : base(_oragin, _cols, _rows, _cellSize)
		{
			
		}

		/// <summary>
		/// get a cells collumn and row based on its world position
		/// </summary>
		/// <param name="worldPosition">position of the wanted cell in world space</param>
		/// <returns>vector containing the col and row of the cell</returns>
		public override Vector3Int GetCellPosition(Vector3 worldPosition)
		{
			//Vector2 vector = new Vector2(((worldPosition.x / cellSize) - cellSize / 2), ((worldPosition.y / cellSize) - cellSize / 2)) - oragin;
			Vector3 vector = new Vector3(((worldPosition.x / cellSize)), ((worldPosition.y / cellSize))) - oragin;
			if (vector.x < 0 || vector.x >= columns) return Vector3Int.up * -1;
			if (vector.y < 0 || vector.y >= rows) return Vector3Int.up * -1;

			vector.x = Mathf.FloorToInt(vector.x);
			vector.y = Mathf.FloorToInt(vector.y);

			return vector.ToVec3Int();
		}

		/// <summary>
		/// get the world position of a cell
		/// </summary>
		/// <param name="col">column of wanted cell</param>
		/// <param name="row">row of wanted cell</param>
		/// <returns>world position of cell</returns>
		/// <exception cref="Exception">collumn or row are out of range</exception>
		public override Vector3 GetWorldPosition(int col, int row)
		{
			if (col < 0 || col >= columns) return Vector2.up * -100;
			if (row < 0 || row >= rows) return Vector2.up * -100;
			return new Vector3((col * cellSize + cellSize / 2), (row * cellSize + cellSize / 2)) + oragin;
		}
	}
}
