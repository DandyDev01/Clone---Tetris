using Grid;
using UnityEngine;

namespace Tetris 
{
	public class Shape : MonoBehaviour
	{
		[SerializeField] private Block _blockPrefab;
		[SerializeField] private Vector2[] _parts;
		[SerializeField] private Color _color;

		public Block[] Blocks => transform.GetComponentsInChildren<Block>();
		public Vector2[] Parts => _parts;

		public void Init(Grid<bool> grid, int cellX, int cellY)
		{
			for (int i = 0; i < _parts.Length; i++)
			{
				Vector2Int cell = new Vector2Int((int)_parts[i].x + cellX, (int)_parts[i].y + cellY);
				Block newPart = Instantiate(_blockPrefab, grid.GetWorldPosition(cell.x, cell.y), Quaternion.identity);
				
				newPart.transform.parent = transform;
				newPart.Column = cell.x;
				newPart.Row = cell.y;

				newPart.GetComponent<SpriteRenderer>().color = _color;
			}
		}
	}
}