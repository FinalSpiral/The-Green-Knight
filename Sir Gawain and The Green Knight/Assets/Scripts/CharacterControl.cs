using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{   
    public float speed;

    private Rigidbody rb;

    private StateMachine stateMachine;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        stateMachine = GetComponent<StateMachine>();
    }

    private void Update()
    {
        if(Input.GetAxisRaw("Horizontal")!=0 || Input.GetAxisRaw("Vertical") != 0)
        {
            stateMachine.walking = true;
        }
        else
        {
            stateMachine.walking = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (StateMachine.CurrentState == State.Walking)
        {
            rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * speed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}
