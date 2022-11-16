using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    [SerializeField]
    private GridGeneration grid;

    [SerializeField]
    private Vector2Int startPos;

    private Vector2Int currentPos;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float height;

    [SerializeField]
    private int type;

    private Vector3 heightAddition;

    private bool move = false;

    private Vector3 sPos, ePos, direction;

    private int d = 1;

    private void Awake()
    {
        heightAddition = Vector3.up * height;
        currentPos = startPos;
    }

    // Start is called before the first frame update
    void Start()
    {        
        transform.position = grid.GetWorldPosFromGrid(startPos) + heightAddition;
        grid.grid[startPos.y, startPos.x] = type;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        if (move)
        {
            transform.position += (direction).normalized * speed * Time.deltaTime;
            if (Vector3InverseLerp(sPos, ePos, transform.position) >= 0.95)
            {
                transform.position = ePos;
                move = false;               
            }
        }
    }

    //source: https://answers.unity.com/questions/1271974/inverselerp-for-vector3.html
    public static float Vector3InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }

    public void moveInDir(Vector2Int dir)
    {
        if (!move)
        {
            if (grid.gridSize.x>currentPos.x+dir.x && 0 <= currentPos.x + dir.x 
                && dir.x != 0 && grid.grid[currentPos.y, currentPos.x + dir.x]==0)
            {
                move = true;
                sPos = transform.position;
                ePos = grid.GetWorldPosFromGrid(new Vector2Int(currentPos.x + dir.x, currentPos.y)) + heightAddition;
                direction = ePos - transform.position;

                grid.grid[currentPos.y, currentPos.x] = 0;
                currentPos = new Vector2Int(currentPos.x + dir.x, currentPos.y);
                grid.grid[currentPos.y, currentPos.x] = type;
                d = 1;
            }
            else if (grid.gridSize.y > currentPos.y + dir.y && 0 <= currentPos.y + dir.y && 
                dir.y != 0 && grid.grid[currentPos.y + dir.y, currentPos.x] == 0)
            {
                move = true;
                sPos = transform.position;
                ePos = grid.GetWorldPosFromGrid(new Vector2Int(currentPos.x, currentPos.y + dir.y)) + heightAddition;
                direction = ePos - transform.position;

                grid.grid[currentPos.y, currentPos.x] = 0;
                currentPos = new Vector2Int(currentPos.x, currentPos.y + dir.y);
                grid.grid[currentPos.y, currentPos.x] = type;
                d = 2;
            }
        }
    }

    public bool IsMoving()
    {
        return move;
    }

    public Vector2Int getCurrentPos()
    {
        return currentPos;
    }

    public int getMoveAxis()
    {
        if (move)
        {
            return d;
        }
        else
        {
            return 0;
        }
    }

}
