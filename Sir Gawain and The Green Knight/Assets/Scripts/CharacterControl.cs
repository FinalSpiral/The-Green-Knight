using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{   
    public float speed;

    private Rigidbody rb;

    private StateMachine stateMachine;

    [SerializeField]
    private AnimationScript aniS;

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
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                if (!aniS.forward)
                {
                    aniS.change = true;
                    aniS.forward = true;
                }
            }
            else if(Input.GetAxisRaw("Horizontal") < 0)
            {
                if (aniS.forward)
                {
                    aniS.change = true;
                    aniS.forward = false;
                }
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}
