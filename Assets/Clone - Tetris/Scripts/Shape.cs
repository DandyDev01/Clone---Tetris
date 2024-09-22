using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris 
{
	public class Shape : MonoBehaviour
	{
		[SerializeField] private Block _blockPrefab;
		[SerializeField] private Vector2Int[] _parts;
		[SerializeField] private Color _color;

		private bool _canMove = true;
	}
}