using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

namespace Tetris
{
	public class GameController : MonoBehaviour
	{
		private const float _tickRate = 1f;
		private const float _placeShapeTime = 0.4f;


		private Timer _tickTimer;
		private Timer _nextShapeTimer;
		private ShapeManager _shapeManager;
		private GridXY<bool> _grid;

		private void Awake()
		{
			_shapeManager = GameObject.FindAnyObjectByType<ShapeManager>();

			_nextShapeTimer = new Timer(_placeShapeTime, false);

			_nextShapeTimer.OnTimerEnd += PlaceShape;

			_tickTimer = new Timer(_tickRate, true);
			_tickTimer.OnTimerEnd += HandleTick;
		}

		private void Start()
		{
			_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<SampleGridXY>().Grid;
		}

		private void Update()
		{
			_tickTimer.Tick(Time.deltaTime);
			_nextShapeTimer.Tick(Time.deltaTime);
		}

		private void HandleTick()
		{
			_shapeManager.MoveShape(_shapeManager.CurrentShape.Blocks, Vector2.down);

			Block[] blocks = _shapeManager.CurrentShape.Blocks;
			float y = blocks.OrderBy(x => x.transform.position.y).Reverse().First().transform.position.y;
			blocks = blocks.Where(b => b.transform.position.y == y).ToArray();

			bool canMove = true;
			foreach (Block block in blocks)
			{
				if (_grid.IsInRange(block.Column, block.Row - 1) == false ||
					_grid.GetElement(block.Column, block.Row - 1))
				{
					canMove = false;
					break;
				}

			}

			_tickTimer.Reset(_tickRate, true);

			if (canMove)
				return;

			_nextShapeTimer.Play();
		}

		private void PlaceShape()
		{
			_nextShapeTimer.Stop();
			_nextShapeTimer.Reset(_placeShapeTime, false);
			_shapeManager.PlaceShape(_shapeManager.CurrentShape);
		}
	}
}