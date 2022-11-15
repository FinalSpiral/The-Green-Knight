using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SideMovement : MonoBehaviour
{
    private Rigidbody rb;

    //x & y are the side distance and z & w are where that distance lasts
    public List<Vector4> map;

    private Vector2 currentMapPos;

    [SerializeField]
    private float middle, step, speed;

    private float goal;

    [HideInInspector]
    public bool move = false;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentMapPos = new Vector2(middle, middle);
        goal = middle;
    }

    private void FixedUpdate()
    {
        if (move)
        {
            foreach(Vector4 v in map)
            {
                if(transform.position.x >= v.z && transform.position.x < v.w)
                {                    
                    currentMapPos = new Vector2(v.x, v.y);
                    goal = transform.position.z + step;
                    Debug.Log(goal);
                    break;
                }
            }
            if(goal <= currentMapPos.x && goal > currentMapPos.y)
            {
                Debug.Log("veloc");
                rb.velocity = new Vector3(rb.velocity.x, 0, step * speed);
            }
            else
            {
                Debug.Log("veloc2");
                rb.velocity = new Vector3(rb.velocity.x, 0, 0);
                move = false;
            }
        }
    }

    public void Move(int dir)
    {
        if (dir != 0)
        {
            step = Mathf.Abs(step) * dir;
            move = true;
        }
    }
}
