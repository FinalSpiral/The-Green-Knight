using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewMove : MonoBehaviour
{
    public Transform left, right;

    private Rigidbody rb;

    private float xMove;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            xMove = other.GetComponent<Rigidbody>().velocity.x;
        }
    }

    private void OnTriggerStay(Collider other)
    {       
        if (other.tag == "Player")
        {
            if (other.GetComponent<Rigidbody>().velocity.x != 0 && xMove != other.GetComponent<Rigidbody>().velocity.x)
            {
                xMove = other.GetComponent<Rigidbody>().velocity.x;
            }

            if (Vector3.Distance(other.transform.position, left.position) > Vector3.Distance(other.transform.position, right.position))
            {
                rb.velocity = new Vector3(Mathf.Abs(xMove), 0, 0);
            }
            else
            {
                rb.velocity = new Vector3(-Mathf.Abs(xMove), 0, 0);
            }     
        }
    }

    private void OnTriggerExit(Collider other)
    {
        rb.velocity = Vector3.zero;
    }
}
