using Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
	public class NextShapePreview : MonoBehaviour
	{
		[SerializeField] private Transform _preivewPosition;
		[SerializeField] private SampleGridXY _grid;

		private Shape _preview;

		public void UpdatePreview(Shape shape)
		{
			if (_preview is null)
			{
				_preview = Instantiate(shape, _preivewPosition.position, Quaternion.identity);
				_preview.Init(_grid.Grid, 2, 3);
			}

			if (_preview.gameObject is not null)
				Destroy(_preview.gameObject);

			_preview = Instantiate(shape, _preivewPosition.position, Quaternion.identity);
			_preview.Init(_grid.Grid, 2, 3);
		}
	}
}