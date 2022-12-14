using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridGeneration : MonoBehaviour
{
    /// <summary>
    /// 0 = can move through, green
    /// 1 = can't move through, red
    /// 2 = player, can't move through, blue
    /// 3 = enemy, can't move through, magenta
    /// </summary>
    public int[,] grid;

    public Vector2Int gridSize;

    [SerializeField]
    private Vector2 stepSize;

    [SerializeField]
    private List<Vector2Int> obsticles;

    [SerializeField]
    private bool drawGrid, numsOn;

    private void Awake()
    {
        MakeGrid();
    }

    public Vector3 GetWorldPosFromGrid(Vector2Int pos)
    {
        return new Vector3(transform.position.x + (stepSize.x / 2) + (stepSize.x * pos.x),
            transform.position.y,
            transform.position.z + (stepSize.y / 2) + (stepSize.y * pos.y));
    }

    public Vector2Int GetGridPosFromWorld(Vector3 pos)
    {
        pos = pos - transform.position;
        return new Vector2Int((int)((pos.x / stepSize.x) - (stepSize.x / 2)),
            (int)((pos.z / stepSize.y) - (stepSize.y / 2)));
    }

    private void MakeGrid()
    {
        grid = new int[gridSize.y, gridSize.x];
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                grid[y, x] = 0;
                foreach (Vector2Int i in obsticles)
                {
                    if(i.x == x && i.y == y)
                    {
                        grid[y, x] = 1;
                    }
                }
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (drawGrid)
        {
            if (grid == null || drawGrid)
                MakeGrid();
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    GUIStyle TextFieldStyles = new GUIStyle(EditorStyles.textField);

                    if (grid[y, x] == 0)
                    {
                        TextFieldStyles.normal.textColor = Color.green;
                        Gizmos.color = Color.green;
                        Gizmos.DrawWireCube(GetWorldPosFromGrid(new Vector2Int(x, y)), new Vector3(stepSize.x, 0, stepSize.y));
                    }
                    else if (grid[y, x] == 1)
                    {
                        TextFieldStyles.normal.textColor = Color.red;
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireCube(GetWorldPosFromGrid(new Vector2Int(x, y)), new Vector3(stepSize.x - 0.1f, 0, stepSize.y - 0.1f));
                    }
                    else if (grid[y, x] == 2)
                    {
                        TextFieldStyles.normal.textColor = Color.blue;
                        Gizmos.color = Color.blue;
                        Gizmos.DrawWireCube(GetWorldPosFromGrid(new Vector2Int(x, y)), new Vector3(stepSize.x - 0.1f, 0, stepSize.y - 0.1f));
                    }
                    else if (grid[y, x] == 3)
                    {
                        TextFieldStyles.normal.textColor = Color.magenta;
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawWireCube(GetWorldPosFromGrid(new Vector2Int(x, y)), new Vector3(stepSize.x - 0.1f, 0, stepSize.y - 0.1f));
                    }
                    else
                    {
                        TextFieldStyles.normal.textColor = Color.yellow;
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawWireCube(GetWorldPosFromGrid(new Vector2Int(x, y)), new Vector3(stepSize.x, 0, stepSize.y));
                    }
                    if (numsOn)
                        Handles.Label(GetWorldPosFromGrid(new Vector2Int(x, y)) + (Vector3.up * 0.3f), x + " " + y, TextFieldStyles);
                }
            }
        }
    }
}
