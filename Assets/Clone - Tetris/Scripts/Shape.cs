using Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris 
{
	public class Shape : MonoBehaviour
	{
		[SerializeField] private Block _blockPrefab;
		[SerializeField] private Vector2[] _parts;
		[SerializeField] private Color _color;

		private Block[] _blocks;

		public Block[] Blocks => _blocks;

		public void Init(Grid<bool> grid, int cellX, int cellY)
		{
			_blocks = new Block[_parts.Length];

			for (int i = 0; i < _parts.Length; i++)
			{
				Vector2Int cell = new Vector2Int((int)_parts[i].x + cellX, (int)_parts[i].y + cellY);
				Block newPart = Instantiate(_blockPrefab, grid.GetWorldPosition(cell.x, cell.y), Quaternion.identity);
				newPart.transform.parent = transform;
				_blocks[i] = newPart;
			}
		}
	}
}