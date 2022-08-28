using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    //states:
    /// <summary>
    /// Walking hold
    /// Blocking hold
    /// Reciving hold
    /// Attacking time
    /// Stunned time
    /// Standing neutral
    /// Parrying time
    /// Charging hold
    /// </summary>

    public static State CurrentState = State.Standing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StateTransition(State from, State to)
    {
        if (from == StateMachine.CurrentState)
        {
            StateMachine.CurrentState = to;
        }
    }
}
