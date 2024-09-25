using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
	public enum cellNeighbour { north, nEast, east, sEast, south, sWest, west, nWest };

    public abstract class Grid<TGridObject>
    {
		protected Vector3 oragin;
        protected int columns = 8;
        protected int rows = 8;
        protected float cellSize = 1;
        public TGridObject[,] Cells { get; protected set; }

		public int Columns { get { return columns; } }
		public int Rows { get { return rows; } }
		public float CellSize { get { return cellSize; } }
		public Vector3 Oragin => oragin;

		public Grid(Vector3 _oragin, int _cols, int _rows, float _cellSize)
		{
			oragin = _oragin;
			columns = _cols;
			rows = _rows;
			cellSize = _cellSize;
			Cells = new TGridObject[columns, rows];
		}

		/// <summary>
		/// get a cells collumn and row based on its world position
		/// </summary>
		/// <param name="worldPosition">position of the wanted cell in world space</param>
		/// <returns>vector containing the col and row of the cell</returns>
		public abstract Vector3Int GetCellPosition(Vector3 worldPosition);

		/// <summary>
		/// get the world position of a cell
		/// </summary>
		/// <param name="col">column of wanted cell</param>
		/// <param name="row">row of wanted cell</param>
		/// <returns>world position of cell</returns>
		/// <exception cref="Exception">collumn or row are out of range</exception>
		public abstract Vector3 GetWorldPosition(int col, int row);

		/// <summary>
		/// Determines if the target world position maps to a cell position
		/// </summary>
		/// <param name="targetPosition">target position in world space</param>
		/// <returns>true is the target position is inside the grid</returns>
		public bool IsInRange(Vector3 targetPosition)
		{
			Vector3Int cell = GetCellPosition(targetPosition);
			return IsInRange(cell.x, cell.y);
		}

		/// <summary>
		/// Determines if the target world position maps to a cell position
		/// </summary>
		/// <param name="col">Column to check</param>
		/// <param name="row">Row to Check</param>
		/// <returns>true is the target position is inside the grid</returns>
		public bool IsInRange(int col, int row)
		{
			return col >= 0 && col < columns && row >= 0
				&& row < rows;
		}

		/// <summary>
		/// Get the world position of a neighbouring cell via its world position
		/// </summary>
		/// <param name="worldPosition">world position of the cell whose neighbour you want</param>
		/// <param name="neighbourWanted">direction of the neighbour</param>
		/// <returns>world position of the wanted neighbour</returns>
		/// <exception cref="Exception">the neighbour wanted does not exist</exception>
		public Vector3 GetNeighbourWorldPosition(Vector3 worldPosition, cellNeighbour neighbourWanted)
		{
			switch (neighbourWanted)
			{
				case cellNeighbour.north:
					return GetWorldPosition((int)GetCellPosition(worldPosition).x, (int)GetCellPosition(worldPosition).y + 1);
				case cellNeighbour.nEast:
					return GetWorldPosition((int)GetCellPosition(worldPosition).x + 1, (int)GetCellPosition(worldPosition).y + 1);
				case cellNeighbour.east:
					return GetWorldPosition((int)GetCellPosition(worldPosition).x + 1, (int)GetCellPosition(worldPosition).y);
				case cellNeighbour.sEast:
					return GetWorldPosition((int)GetCellPosition(worldPosition).x + 1, (int)GetCellPosition(worldPosition).y-1);
				case cellNeighbour.south:
					return GetWorldPosition((int)GetCellPosition(worldPosition).x, (int)GetCellPosition(worldPosition).y-1);
				case cellNeighbour.sWest:
					return GetWorldPosition((int)GetCellPosition(worldPosition).x-1, (int)GetCellPosition(worldPosition).y-1);
				case cellNeighbour.west:
					return GetWorldPosition((int)GetCellPosition(worldPosition).x-1, (int)GetCellPosition(worldPosition).y);
				case cellNeighbour.nWest:
					return GetWorldPosition((int)GetCellPosition(worldPosition).x-1, (int)GetCellPosition(worldPosition).y+1);
				default:
					throw new Exception("no neighbours");
			}
		}

		/// <summary>
		/// Get the world position of a neighbouring cell via its world position 
		/// </summary>
		/// <param name="col">column of wanted cell</param>
		/// <param name="row">row of wanted cell</param>
		/// <param name="neighbourWanted"></param>
		/// <returns>world position of the wanted neighbour</returns>
		/// <exception cref="Exception">the neighbour wanted does not exist</exception>
		public Vector3 GetNeighbourWorldPositiion(int col, int row, cellNeighbour neighbourWanted)
		{
			switch (neighbourWanted)
			{
				case cellNeighbour.north:
					return GetWorldPosition(col, row + 1);
				case cellNeighbour.nEast:
					return GetWorldPosition(col + 1, row + 1);
				case cellNeighbour.east:
					return GetWorldPosition(col + 1, row);
				case cellNeighbour.sEast:
					return GetWorldPosition(col + 1, row - 1);
				case cellNeighbour.south:
					return GetWorldPosition(col, row - 1);
				case cellNeighbour.sWest:
					return GetWorldPosition(col - 1, row - 1);
				case cellNeighbour.west:
					return GetWorldPosition(col - 1, row);
				case cellNeighbour.nWest:
					return GetWorldPosition(col - 1, row + 1);
				default:
					throw new Exception("no neighbours");
			}
		}

		/// <summary>
		/// gets the specified neighbour
		/// </summary>
		/// <param name="worldPosition">position to search for neigbour from</param>
		/// <param name="neighbourWanted">direction of the neighbour wanted</param>
		/// <returns></returns>
		public TGridObject GetNeighbour(Vector3 worldPosition, cellNeighbour neighbourWanted)
		{
			Vector3 neighbourCell = GetCellPosition(GetNeighbourWorldPosition(worldPosition, neighbourWanted));
			return Cells[(int)neighbourCell.x, (int)neighbourCell.y];
		}

		public IEnumerable<Vector3Int> GetNeighboursCells(Vector3 worldPosition)
		{
			Vector3Int cell = GetCellPosition(worldPosition);
			Vector3Int[] neighbours = { new Vector3Int(0, 1), new Vector3Int(1, 1),
				new Vector3Int(1, 0), new Vector3Int(1, -1), new Vector3Int(0, -1),
				new Vector3Int(-1, -1), new Vector3Int(-1, 0), new Vector3Int(-1, 1) };

			List<Vector3Int> results = new();

			foreach (var item in neighbours)
			{
				if (IsInRange((cell + item).x, (cell + item).y))
					results.Add(cell + item);
			}

			return results;
		}

		public IEnumerable<Vector3Int> GetNeighbourCells(int col, int row)
		{
			Vector3Int[] neighbours = { new Vector3Int(0, 1),
				new Vector3Int(1, 0), new Vector3Int(0, -1),
				 new Vector3Int(-1, 0) };

			List<Vector3Int> results = new();

			foreach (var item in neighbours)
			{
				if (IsInRange(col + item.x, row + item.y))
					results.Add(new Vector3Int(col, row) + item);
			}

			return results;
		}
		
		/// <summary>
		/// Get the neighbours of a cell in world space
		/// </summary>
		/// <param name="col">col of cell whose neighbours you want</param>
		/// <param name="row">row of cell whose neighbours you want</param>
		/// <returns>IEnumerable of the cells neightbours in world space coords</returns>
		public IEnumerable<Vector3> GetNeighboursWorldPositions(int col, int row)
		{
			Vector3[] neighbours = { new Vector3(0, 1), new Vector3(1, 1), 
				new Vector3(1, 0), new Vector3(1, -1), new Vector3(0, -1), 
				new Vector3(-1, -1), new Vector3(-1, 0), new Vector3(-1, 1) };
			
			List<Vector3> results = new List<Vector3>();

			foreach (var item in neighbours)
			{
				results.Add(GetWorldPosition(col, row) + item);
			}

			return results;
		}

		public IEnumerable<Vector3> GetNeighboursWorldPositions(Vector3 worldPosition)
		{
			Vector3[] neighbours = { new Vector3(0, 1),
				new Vector3(1, 0),  new Vector3(0, -1)
				, new Vector3(-1, 0) };

			List<Vector3> results = new List<Vector3>();

			foreach (var item in neighbours)
			{
				results.Add(worldPosition + item);
			}

			return results;
		}

		/// <summary>
		/// Get the element at a specified cell
		/// </summary>
		/// <param name="col">col of cell wanted</param>
		/// <param name="row">row of cell wanted</param>
		/// <returns></returns>
		public TGridObject GetElement(int col, int row)
		{
			return Cells[col, row];
		}

		public TGridObject GetElement(Vector3 worldPosition)
		{
			Vector3 cell = GetCellPosition(worldPosition);
			return GetElement((int)cell.x, (int)cell.y);
		}

		/// <summary>
		/// Set the elemnt at a specified cell
		/// </summary>
		/// <param name="col">col of cell to set</param>
		/// <param name="row">row of cell to set</param>
		/// <param name="e">value to set cell</param>
		/// <returns></returns>
		public bool SetElement(int col, int row, TGridObject e)
		{
			if (IsInRange(col, row) == false) 
				return false;

			Cells[col, row] = e;
			return true;
		}

		/// <summary>
		/// Switch two elements positions in world space and their position in the grid
		/// </summary>
		/// <param name="pos1">vec2 containing the col & row of element 1</param>
		/// <param name="pos2">vec2 containing the col & row of element 2</param>
		/// <returns>wheatehr of not the switch was sucessful</returns>
		public bool Switch(int x1, int y1, int x2, int y2)
		{
			//TGridObject temp = Cells[x1, y1];

			//Cells[x1, y1] = Cells[x2, y2];
			//Cells[x2, y2] = temp;

			//if(Cells[x1, y1] != null)
			//{
			//	Cells[x1, y1].gameObject.transform.position = GetWorldPosition(x1, y1);
			//}
			//if (Cells[x2, y2] != null)
			//{
			//	Cells[x2, y2].gameObject.transform.position = GetWorldPosition(x2, y2);
			//}

			//return true;
			throw new NotImplementedException();
		}

		/// <summary>
		/// Switch to elements positions in world space and cell space
		/// </summary>
		/// <param name="worldPosition1">world position one</param>
		/// <param name="worldPosition2">world position two</param>
		/// <returns>wheather or not the two were switched</returns>
		public bool Switch(Vector3 worldPosition1, Vector3 worldPosition2)
		{
			Vector3Int cell1 = GetCellPosition(worldPosition1);
			Vector3Int cell2 = GetCellPosition(worldPosition2);
			return Switch(cell1.x, cell1.y, cell2.x, cell2.y);
		}

		private void OnDrawGizmos()
		{
			//oragin = transform.position;
			Gizmos.color = Color.white;
			for (int ii = 0; ii < columns; ii++)
			{
				float i = ii * cellSize;
				Vector2 offset = Vector2.zero;
				for (int jj = 0; jj < rows; jj++)
				{
					float j = jj * cellSize;
					// bottom
					Gizmos.DrawLine(new Vector2(oragin.x+i, oragin.y+j), 
						new Vector2(oragin.x+i+cellSize, oragin.y+j));
					// left
					Gizmos.DrawLine(new Vector2(oragin.x + i, oragin.y+j), 
						new Vector2(oragin.x+i, oragin.y+j+cellSize));
				}

				// top of topCell
				Gizmos.DrawLine(new Vector2(oragin.x+i, oragin.y+rows*cellSize), 
					new Vector2(oragin.x+i+cellSize, oragin.y+rows*cellSize));

			}

			// draw right of last column
			for (int i = 0; i < rows; i++)
			{
				Gizmos.DrawLine(new Vector2(oragin.x+columns*cellSize, oragin.y+i*cellSize), 
					new Vector2(oragin.x + columns*cellSize, oragin.y + (i*cellSize) + cellSize));
			}
		}
	}
}
