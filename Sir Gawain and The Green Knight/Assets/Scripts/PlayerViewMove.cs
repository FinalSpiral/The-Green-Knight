using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewMove : MonoBehaviour
{
    [SerializeField]
    private Transform left, right;

    [SerializeField]
    private GridGeneration grid;

    [SerializeField]
    private GridMovement viewMove;

    [SerializeField]
    private GridMovement player;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (player.getCurrentPos().x >= grid.GetGridPosFromWorld(right.position).x)
        {
            viewMove.moveInDir(new Vector2Int(1, 0));
        }

        if (player.getCurrentPos().x <= grid.GetGridPosFromWorld(left.position).x)
        {
            viewMove.moveInDir(new Vector2Int(-1, 0));
        }
    }

}
