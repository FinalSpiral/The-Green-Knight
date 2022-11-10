using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterControl : MonoBehaviour
{      
    private Rigidbody rb;

    public State CurrentState;
    public Mode CurrentMode;

    public float speed;

    [SerializeField]
    private AnimationScript aniS;

    private List<KeyCode> orderOfInputs;

    private KeyCode blockingInput, receveingInput, parryingInput;

    void Awake()
    {
        //Definitions
        orderOfInputs = new List<KeyCode>();
        rb = GetComponent<Rigidbody>();
        //Input definitions
        blockingInput = KeyCode.Q;
        receveingInput = KeyCode.E;
        parryingInput = KeyCode.R;

        //State declarations
        State standing = new State(StateIdentifier.Standing);
        State walking = new State(StateIdentifier.Walking);
        State stunned = new State(StateIdentifier.Stunned);
        State hit = new State(StateIdentifier.Hit);
        State blocking = new State(StateIdentifier.Blocking);
        State reciving = new State(StateIdentifier.Receiving);
        State parrying = new State(StateIdentifier.Parrying);
        State cooldown = new State(StateIdentifier.Cooldown);

        //State transitions
        standing.AddTransitions(walking, stunned, hit, blocking, reciving, parrying);
        walking.AddTransitions(standing, stunned, hit, blocking, reciving, parrying);
        stunned.AddTransitions(standing);
        hit.AddTransitions(standing);
        blocking.AddTransitions(standing, reciving, parrying);
        reciving.AddTransitions(standing, hit, blocking, parrying);
        parrying.AddTransitions(standing, stunned, hit, cooldown);
        cooldown.AddTransitions(standing);

        //Current State
        CurrentState = standing;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        //Movement input
        if (Input.GetAxisRaw("Horizontal")!=0 || Input.GetAxisRaw("Vertical") != 0 )
        {
            CharacterTransition(StateIdentifier.Walking, 1);          
        }        
        else if (CurrentState.state == StateIdentifier.Walking)
        {
            CharacterTransition(StateIdentifier.Standing, 0);
        }
      
        //this will get transfered into colision detection
        if (Input.GetKeyDown(KeyCode.F))
        {
            CharacterTransition(StateIdentifier.Stunned, 2);
        }
        else if(aniS.Finished && CurrentState.state == StateIdentifier.Stunned)
        {
            CharacterTransition(StateIdentifier.Standing, 0);
        }
        //this will get transfered into colision detection
        if (Input.GetKeyDown(KeyCode.G))
        {
            CharacterTransition(StateIdentifier.Hit, 3);
        }
        else if (aniS.Finished && CurrentState.state == StateIdentifier.Hit)
        {
            CharacterTransition(StateIdentifier.Standing, 0);
        }

        //Blocking Input
        if (CurrentMode != Mode.EmptyHanded)
        {
            if (Input.GetKey(blockingInput) &&
                ((orderOfInputs.Count > 0) ? 
                (orderOfInputs[orderOfInputs.Count-1] == blockingInput || !orderOfInputs.Contains(blockingInput)) : true))
            {
                if (!orderOfInputs.Contains(blockingInput))
                    orderOfInputs.Add(blockingInput);

                CharacterTransition(StateIdentifier.Blocking, 4);
            }
            if (Input.GetKeyUp(blockingInput) && CurrentState.state == StateIdentifier.Blocking)
            {
                CharacterTransition(StateIdentifier.Standing, 0);
            }
            if (Input.GetKeyUp(blockingInput))
            {
                orderOfInputs.Remove(blockingInput);
            }
        }

        //Reciving input
        if (CurrentMode != Mode.EmptyHanded)
        {
            if (Input.GetKey(receveingInput) && 
                ((orderOfInputs.Count > 0) ? 
                (orderOfInputs[orderOfInputs.Count-1] == receveingInput || !orderOfInputs.Contains(receveingInput)) : true) )
            {
                if(!orderOfInputs.Contains(receveingInput))
                    orderOfInputs.Add(receveingInput);

                CharacterTransition(StateIdentifier.Receiving, 5);
            }
            if (Input.GetKeyUp(receveingInput) && CurrentState.state == StateIdentifier.Receiving)
            {
                CharacterTransition(StateIdentifier.Standing, 0);
            }
            if (Input.GetKeyUp(receveingInput))
            {
                orderOfInputs.Remove(receveingInput);
            }
        }

        //Parrying Input
        if (CurrentMode != Mode.EmptyHanded)
        {
            if (Input.GetKeyDown(parryingInput))
            {
                CharacterTransition(StateIdentifier.Parrying, 6);
            }
            else if (aniS.Finished && CurrentState.state == StateIdentifier.Parrying)
            {
                CharacterTransition(StateIdentifier.Cooldown, 7);
            }
            else if (aniS.Finished && CurrentState.state == StateIdentifier.Cooldown)
            {
                CharacterTransition(StateIdentifier.Standing, 0);
            }
        }

        //Standing
        if (!Input.anyKey && (aniS.Finished || aniS.loop ))
        {
            CharacterTransition(StateIdentifier.Standing, 0);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CurrentState.state == StateIdentifier.Walking)
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

    private void CharacterTransition(StateIdentifier s, int a)
    {
        if (CurrentState.CheckTransitionPosibility(s))
        {
            CurrentState = CurrentState.MakeTransition(s);
            aniS.ChangeAnimation(a + (int)CurrentMode);
            aniS.change = true;
            Debug.Log(CurrentState.state);
        }
    }
}
