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
    /// RaisingBlock hold
    /// </summary>

    public static State CurrentState = State.Standing;

    [HideInInspector]
    public bool walking = false, blocking = false, reciving = false, attacking = false, stunned = false,
        parry = false, charging = false;

    private AnimationScript animationScript;

    // Start is called before the first frame update
    void Start()
    {
        animationScript = GetComponent<AnimationScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentState == State.Walking)
        {
            if(blocking || reciving || charging || attacking || stunned || parry || !walking)
            {
                CurrentState = State.Standing;
                animationScript.ChangeAnimation(2);
            }
        }
        if (CurrentState == State.Standing)
        {
            if (walking)
            {
                CurrentState = State.Walking;
                animationScript.ChangeAnimation(0);
            }
            if (blocking)
            {

            }
            if (reciving)
            {

            }
            if (charging)
            {

            }
            if (attacking)
            {

            }
            if (stunned)
            {

            }
            if (parry)
            {

            }
        }        
    }

    public void StateTransition(State from, State to)
    {
        if (from == StateMachine.CurrentState)
        {
            StateMachine.CurrentState = to;
        }
    }
}
