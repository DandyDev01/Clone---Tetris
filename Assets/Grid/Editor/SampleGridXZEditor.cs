using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grid
{
    [CustomEditor(typeof(SampleGridXZ))]
    public class SampleGridXZEditor : Editor
    {
        private void OnSceneGUI()
        {
            SampleGridXZ grid = (SampleGridXZ)target;

            // draw the grid
            Handles.color = Color.white;

            for (int ii = 0; ii < grid.Columns; ii++)
            {
                float i = ii * grid.CellSize;
                Vector2 offset = Vector2.zero;
                for (int jj = 0; jj < grid.Rows; jj++)
                {
                    float j = jj * grid.CellSize;
                    // bottom
                    Handles.DrawLine(new Vector3(grid.Oragin.x + i, grid.Oragin.y, grid.Oragin.z + j),
                        new Vector3(grid.Oragin.x + i + grid.CellSize, grid.Oragin.y, grid.Oragin.z + j));
                    // left
                    Handles.DrawLine(new Vector3(grid.Oragin.x + i, grid.Oragin.y, grid.Oragin.z + j),
                        new Vector3(grid.Oragin.x + i, grid.Oragin.y, grid.Oragin.z + j + grid.CellSize));
                }

                // top of topCell
                Handles.DrawLine(new Vector3(grid.Oragin.x + i, grid.Oragin.y, grid.Oragin.z + grid.Rows * grid.CellSize),
                    new Vector3(grid.Oragin.x + i + grid.CellSize, grid.Oragin.y, grid.Oragin.z + grid.Rows * grid.CellSize));
            }

            // draw right of last column
            for (int i = 0; i < grid.Rows; i++)
            {
                Handles.DrawLine(new Vector3(grid.Oragin.x + grid.Columns * grid.CellSize, grid.Oragin.y, grid.Oragin.z + i * grid.CellSize),
                    new Vector3(grid.Oragin.x + grid.Columns * grid.CellSize, grid.Oragin.y, grid.Oragin.z + (i * grid.CellSize) + grid.CellSize));
            }
        }
    }
}
