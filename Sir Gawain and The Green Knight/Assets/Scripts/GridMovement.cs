using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    private Vector3 Offset;

    [SerializeField]
    private int type;

    [SerializeField]
    private bool noclip;

    [SerializeField]
    private bool drawPath;
    [SerializeField]
    private Transform drawPathPos;

    private bool move = false, pmove = false;

    private Vector3 sPos, ePos, direction;

    private Vector2Int movingTo;

    private void Awake()
    {
        currentPos = startPos;
    }

    // Start is called before the first frame update
    void Start()
    {        
        transform.position = grid.GetWorldPosFromGrid(startPos) + Offset;
        if (!noclip)
            grid.grid[startPos.y, startPos.x] = type;

    }

    // Update is called once per frame
    void Update()
    {
        //this will be removed
        MoveTo(grid.GetGridPosFromWorld(drawPathPos.position));

        if (pmove)
        {
            Debug.Log(movingTo - currentPos);
            moveInDir(movingTo-currentPos);
            if((movingTo - currentPos).x == 0 && (movingTo - currentPos).y == 0)
            {
                pmove = false;
            }
        }

        //Movement
        if (move)
        {
            transform.position += (direction).normalized * speed * Time.deltaTime;
            if (Vector3InverseLerp(sPos, ePos, transform.position) >= 0.95)
            {
                transform.position = ePos;
                move = false;
                pmove = false;
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

    //Movement
    public void moveInDir(Vector2Int dir)
    {
        if (!move)
        {
            if (grid.gridSize.x > currentPos.x + dir.x && 0 <= currentPos.x + dir.x &&
                grid.gridSize.y > currentPos.y + dir.y && 0 <= currentPos.y + dir.y &&
                (grid.grid[currentPos.y + dir.y, currentPos.x + dir.x] == 0 &&
                ((dir.x != 0) ? (grid.grid[currentPos.y, currentPos.x + dir.x] == 0) : true) &&
                ((dir.y != 0) ? (grid.grid[currentPos.y + dir.y, currentPos.x] == 0) : true)) || noclip)
            {
                Debug.Log(currentPos);
                move = true;
                sPos = transform.position;
                ePos = grid.GetWorldPosFromGrid(new Vector2Int(currentPos.x + dir.x, currentPos.y + dir.y)) + Offset;
                direction = ePos - transform.position;

                if(!noclip)
                    grid.grid[currentPos.y, currentPos.x] = 0;
                currentPos = new Vector2Int(currentPos.x + dir.x, currentPos.y + dir.y);
                if (!noclip)
                    grid.grid[currentPos.y, currentPos.x] = type;
                Debug.Log(currentPos);
            }
            /*else if (grid.gridSize.y > currentPos.y + dir.y && 0 <= currentPos.y + dir.y && 
                dir.y != 0 && grid.grid[currentPos.y + dir.y, currentPos.x] == 0)
            {
                move = true;
                sPos = transform.position;
                ePos = grid.GetWorldPosFromGrid(new Vector2Int(currentPos.x, currentPos.y + dir.y)) + Offset;
                direction = ePos - transform.position;

                grid.grid[currentPos.y, currentPos.x] = 0;
                currentPos = new Vector2Int(currentPos.x, currentPos.y + dir.y);
                grid.grid[currentPos.y, currentPos.x] = type;
                d = 2;
            }*/
        }
    }

    public void MoveTo(Vector2Int target)
    {
        if (!pmove && ((target-currentPos).x!=0 || (target - currentPos).y != 0))
        {
            pmove = true;
            List<Vector2Int> v = AStarPathFinding(target);
            if (v.Count > 1)
            {
                movingTo = v[1];
            }
            else
            {
                movingTo = v[0];
            }
        }
    }

    //Path Finding
    private class Node
    {
        public Vector2Int pos,t;

        public int gCost, hCost;

        public bool closed;

        public Node previousNode;

        public Node(Vector2Int np, Vector2Int target, int g)
        {
            closed = false;
            t = target;
            pos = np;
            hCost = (Mathf.Abs(target.x-np.x)*10) + (Mathf.Abs(target.y-np.y)*10);
            gCost = g;
        }

        public Node(Vector2Int np, Vector2Int target, int g, Node p)
        {
            closed = false;
            t = target;
            pos = np;
            hCost = Mathf.Abs(target.x - np.x) + Mathf.Abs(target.y - np.y);
            gCost = g;
            previousNode = p;
        }

        public int fCost()
        {
            return gCost + hCost;
        }
    }

    private void CheckIfInOpen(List<Node> o, List<Node> c, Node n, int xi, int yi, int ga)
    {
        bool cls = true, opn = false;
        foreach (Node ci in c)
        {
            if(ci.pos.x == n.pos.x + xi && ci.pos.y == n.pos.y + yi)
            {
                cls = false;
            }
        }
        if (cls)
        {
            foreach (Node oi in o)
            {
                if (oi.pos.x == (n.pos.x + xi) && oi.pos.y == (n.pos.y + yi))
                {
                    opn = true;
                }
            }
            if (opn)
            {
                foreach (Node i in o)
                {
                    if (i.pos.x == (n.pos.x + xi) && i.pos.y == (n.pos.y + yi))
                    {
                        if (i.gCost > n.gCost + ga)
                        {
                            i.gCost = n.gCost + ga;
                            i.previousNode = n;
                        }
                    }
                }
            }
            else
            {
                o.Add(new Node(new Vector2Int(n.pos.x + xi, n.pos.y + yi), n.t, n.gCost + ga, n));
            }
        }
    }   
   
    private Node closeNode(List<Node> o, List<Node> c, Node n)
    {
        n.closed = true;
        c.Add(n);
        o.Remove(n);
        //Cross
        if (n.pos.x + 1 < grid.gridSize.x && grid.grid[n.pos.y, n.pos.x + 1] == 0)
        {
            CheckIfInOpen(o,c, n, 1, 0, 10);
        }
        if (n.pos.x - 1 >= 0 && grid.grid[n.pos.y, n.pos.x - 1] == 0)
        {
            CheckIfInOpen(o, c, n, -1, 0, 10);
        }
        if (n.pos.y + 1 < grid.gridSize.y && grid.grid[n.pos.y + 1, n.pos.x] == 0)
        {
            CheckIfInOpen(o, c, n, 0, 1, 10);
        }
        if (n.pos.y - 1 >= 0 && grid.grid[n.pos.y - 1, n.pos.x] == 0)
        {
            CheckIfInOpen(o, c, n, 0, -1, 10);
        }

        //Diagonal
        if (n.pos.x + 1 < grid.gridSize.x && n.pos.y + 1 < grid.gridSize.y &&
            grid.grid[n.pos.y + 1, n.pos.x + 1] == 0 &&
            grid.grid[n.pos.y, n.pos.x + 1] == 0 &&
            grid.grid[n.pos.y + 1, n.pos.x] == 0)
        {
            CheckIfInOpen(o, c, n, 1, 1, 14);
        }
        if (n.pos.x + 1 < grid.gridSize.x && n.pos.y - 1 >= 0 &&
            grid.grid[n.pos.y - 1, n.pos.x + 1] == 0 &&
            grid.grid[n.pos.y, n.pos.x + 1] == 0 &&
            grid.grid[n.pos.y - 1, n.pos.x] == 0)
        {
            CheckIfInOpen(o, c, n, 1, -1, 14);
        }
        if (n.pos.x - 1 >= 0 && n.pos.y + 1 < grid.gridSize.y &&
            grid.grid[n.pos.y + 1, n.pos.x - 1] == 0 &&
            grid.grid[n.pos.y, n.pos.x - 1] == 0 &&
            grid.grid[n.pos.y + 1, n.pos.x] == 0)
        {
            CheckIfInOpen(o, c, n, -1, 1, 14);
        }
        if (n.pos.x - 1 >= 0 && n.pos.y - 1 >= 0 &&
            grid.grid[n.pos.y - 1, n.pos.x - 1] == 0 &&
            grid.grid[n.pos.y, n.pos.x - 1] == 0 &&
            grid.grid[n.pos.y - 1, n.pos.x] == 0)
        {
            CheckIfInOpen(o, c, n, -1, -1, 14);
        }

        return n;
    }

    private Node findLowestFCostNode(List<Node> o)
    {
        Node min = new Node(Vector2Int.zero, Vector2Int.zero, int.MaxValue);
        foreach(Node n in o)
        {
            if (n.fCost() < min.fCost())
            {
                min = n;
            }
        }
        return min;
    }

    private List<Vector2Int> getPathFromNode(Node n)
    {
        List<Vector2Int> nodes = new List<Vector2Int>();

        Node c = n;
        nodes.Insert(0, c.pos);

        while (c.previousNode != null)
        {
            c = c.previousNode;
            nodes.Insert(0, c.pos);            
        }

        return nodes;
    }

    public List<Vector2Int> AStarPathFinding(Vector2Int target)
    {
        Vector2Int start = currentPos;
        List<Node> opened = new List<Node>();
        List<Node> closed = new List<Node>();

        Node currentNode = closeNode(opened, closed, new Node(start, target, 0));

        while (currentNode.hCost != 0)
        {
            currentNode = closeNode(opened, closed, findLowestFCostNode(opened));
        }

        return getPathFromNode(currentNode);
    }

    //Outputs
    public bool IsMoving()
    {
        return move;
    }

    public Vector2Int getCurrentPos()
    {
        return currentPos;
    }

    //Gizmos
    private void OnDrawGizmos()
    {
        if (grid.grid != null && drawPath)
        {
            List<Vector2Int> l = AStarPathFinding(grid.GetGridPosFromWorld(drawPathPos.position));
            for (int i = 0; i < l.Count - 1; i++)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(grid.GetWorldPosFromGrid(l[i]), grid.GetWorldPosFromGrid(l[i + 1]));
            }
        }
    }

}
