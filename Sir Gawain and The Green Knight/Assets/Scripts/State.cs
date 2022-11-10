using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
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

    public StateIdentifier state;

    public List<State> transitions;

    public State(StateIdentifier state)
    {
        this.state = state;
        transitions = new List<State>();
    }

    public void AddTransitions(params State[] states)
    {
        foreach(State s in states)
        {
            transitions.Add(s);
        }
    }

    public bool CheckTransitionPosibility(StateIdentifier s)
    {
        foreach(State sm in transitions)
        {
            if (sm.state == s)
            {
                return true;
            }
        }
        return false;
    }

    public State MakeTransition(StateIdentifier s)
    {
        foreach (State sm in transitions)
        {
            if (sm.state == s)
            {
                return sm;
            }
        }
        return new State(StateIdentifier.Standing);
    }

}
