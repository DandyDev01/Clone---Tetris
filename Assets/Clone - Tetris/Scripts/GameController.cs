using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
	public class GameController : MonoBehaviour
	{
		private Timer _tickTimer;
		private ShapeManager _shapeManager;

		private void Awake()
		{
			_shapeManager = GameObject.FindAnyObjectByType<ShapeManager>();

			_tickTimer = new Timer(1f, true);
			_tickTimer.OnTimerEnd += HandleTick;
		}

		private void Update()
		{
			_tickTimer.Tick(Time.deltaTime);
		}

		private void HandleTick()
		{
			_shapeManager.MoveShape(_shapeManager.CurrentShape.Blocks, Vector2.down);

			// TODO: check if shape can move down again. If not, start a timer for placing the shape. After the shape is placed move to the next shape.
		}
	}
}