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

		private void Awake()
		{
			_blocks = new Block[_parts.Length];

			for (int i = 0; i < _parts.Length; i++)
			{
				Block newPart = Instantiate(_blockPrefab, _parts[i], Quaternion.identity);
				newPart.transform.parent = transform;
				_blocks[i] = newPart;
			}
		}
	}
}