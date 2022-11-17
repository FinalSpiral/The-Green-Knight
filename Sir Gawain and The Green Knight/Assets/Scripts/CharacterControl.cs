using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{      
    private Rigidbody rb;

    public State CurrentState;
    public Mode CurrentMode;

    public float speed;

    [SerializeField]
    private AnimationScript aniS;

    [SerializeField]
    private Transform attackPoint;

    [SerializeField]
    private Vector3 attackBoxSize;

    [SerializeField]
    private LayerMask attackLayers;

    [SerializeField]
    private GridMovement grid;

    private List<KeyCode> orderOfInputs;

    private KeyCode blockingInput, receveingInput, parryingInput, attackingInput, chargingInput;

    private bool allowHit = true;

    private Vector2Int inpt;

    private List<int> orderOfMovement;

    void Awake()
    {
        //Definitions
        orderOfInputs = new List<KeyCode>();
        orderOfMovement = new List<int>();
        inpt = new Vector2Int(0, 0);
        //Input definitions
        blockingInput = KeyCode.Q;
        receveingInput = KeyCode.E;
        parryingInput = KeyCode.R;
        attackingInput = KeyCode.X;
        chargingInput = KeyCode.C;

        //State declarations
        State standing = new State(StateIdentifier.Standing);
        State walking = new State(StateIdentifier.Walking);
        State stunned = new State(StateIdentifier.Stunned);
        State hit = new State(StateIdentifier.Hit);
        State blocking = new State(StateIdentifier.Blocking);
        State reciving = new State(StateIdentifier.Receiving);
        State parrying = new State(StateIdentifier.Parrying);
        State cooldown = new State(StateIdentifier.Cooldown);
        State attacking1 = new State(StateIdentifier.Attacking1);
        State attacking2 = new State(StateIdentifier.Attacking2);
        State attacking3 = new State(StateIdentifier.Attacking3);
        State attackCooldown1 = new State(StateIdentifier.AttackCooldown1);
        State attackCooldown2 = new State(StateIdentifier.AttackCooldown2);
        State attackCooldown3 = new State(StateIdentifier.AttackCooldown3);
        State charging = new State(StateIdentifier.Charging);
        State attackC = new State(StateIdentifier.AttackingC);
        State sideWalking = new State(StateIdentifier.SideWalking);

        //State transitions
        standing.AddTransitions(walking, sideWalking, stunned, hit, blocking, reciving, parrying, attacking1, charging);
        walking.AddTransitions(standing, sideWalking, stunned, hit, blocking, reciving, parrying, attacking1, charging);
        sideWalking.AddTransitions(standing, walking, stunned, hit, blocking, reciving, parrying, attacking1, charging);
        stunned.AddTransitions(standing);
        hit.AddTransitions(standing);
        blocking.AddTransitions(standing, reciving, parrying, attacking1, charging);
        reciving.AddTransitions(standing, hit, blocking, parrying, attacking1, charging);
        parrying.AddTransitions(standing, stunned, hit, cooldown);
        cooldown.AddTransitions(standing);
        attacking1.AddTransitions(attackCooldown1);
        attacking2.AddTransitions(attackCooldown2);
        attacking3.AddTransitions(attackCooldown3);
        attackCooldown1.AddTransitions(attacking2, standing);
        attackCooldown2.AddTransitions(attacking3, standing);
        attackCooldown3.AddTransitions(standing);
        charging.AddTransitions(attackC);
        attackC.AddTransitions(attackCooldown3);

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
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            orderOfMovement.Remove(1);
        }
        else
        {
            if (!orderOfMovement.Contains(1))
                orderOfMovement.Add(1);
        }
        if (Input.GetAxisRaw("Vertical") == 0)
        {
            orderOfMovement.Remove(2);
        }
        else
        {
            if (!orderOfMovement.Contains(2))
                orderOfMovement.Add(2);
        }

        if (Input.GetAxisRaw("Horizontal") != 0 && (orderOfMovement.Count == 0 || orderOfMovement[orderOfMovement.Count - 1] == 1))
        {           
            CharacterTransition(StateIdentifier.Walking, 1);
            inpt = new Vector2Int((int)Input.GetAxisRaw("Horizontal"), 0);
        }
        else if (Input.GetAxisRaw("Vertical") != 0 && (orderOfMovement.Count == 0 || orderOfMovement[orderOfMovement.Count - 1] == 2))
        {
            CharacterTransition(StateIdentifier.SideWalking, 16);
            inpt = new Vector2Int(0, (int)Input.GetAxisRaw("Vertical"));
        }
        else if (grid.IsMoving())
        {
            inpt = new Vector2Int(0, 0);
        }
        else if (CurrentState.state == StateIdentifier.Walking || CurrentState.state == StateIdentifier.SideWalking)
        {
            CharacterTransition(StateIdentifier.Standing, 0);
        }
      
        //this will get transfered into colision detection
        if (Input.GetKeyDown(KeyCode.F))
        {
            CharacterTransition(StateIdentifier.Stunned, 2);
        }
        if(aniS.animationFinished() && CurrentState.state == StateIdentifier.Stunned)
        {
            CharacterTransition(StateIdentifier.Standing, 0);
        }
        //this will get transfered into colision detection
        if (Input.GetKeyDown(KeyCode.G))
        {
            CharacterTransition(StateIdentifier.Hit, 3);
        }
        if (aniS.animationFinished() && CurrentState.state == StateIdentifier.Hit)
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
            else if (aniS.animationFinished() && CurrentState.state == StateIdentifier.Parrying)
            {
                CharacterTransition(StateIdentifier.Cooldown, 7);
            }
            else if (aniS.animationFinished() && CurrentState.state == StateIdentifier.Cooldown)
            {
                CharacterTransition(StateIdentifier.Standing, 0);
            }
        }

        //Attack Combo input
        if (CurrentMode != Mode.EmptyHanded)
        {
            if (Input.GetKeyDown(attackingInput))
            {
                CharacterTransition(StateIdentifier.Attacking1, 8);
            }
            else if (aniS.animationFinished() && CurrentState.state == StateIdentifier.Attacking1)
            {
                CharacterTransition(StateIdentifier.AttackCooldown1, 9);
            }
            else if (Input.GetKey(attackingInput) && CurrentState.state == StateIdentifier.AttackCooldown1)
            {
                CharacterTransition(StateIdentifier.Attacking2, 10);
            }
            else if (aniS.animationFinished() && CurrentState.state == StateIdentifier.Attacking2)
            {
                CharacterTransition(StateIdentifier.AttackCooldown2, 11);
            }
            else if (Input.GetKey(attackingInput) && CurrentState.state == StateIdentifier.AttackCooldown2)
            {
                CharacterTransition(StateIdentifier.Attacking3, 12);
            }
            else if (aniS.animationFinished() && CurrentState.state == StateIdentifier.Attacking3)
            {
                CharacterTransition(StateIdentifier.AttackCooldown3, 13);
            }
            else if (aniS.animationFinished() && 
                (CurrentState.state == StateIdentifier.AttackCooldown1 ||
                CurrentState.state == StateIdentifier.AttackCooldown2 ||
                CurrentState.state == StateIdentifier.AttackCooldown3))
            {
                Debug.Log("ello");
                Debug.Log(aniS.animationFinished());
                CharacterTransition(StateIdentifier.Standing, 0);
            }
        }

        //Charging Attack Input
        if (CurrentMode != Mode.EmptyHanded)
        {
            if (Input.GetKeyDown(chargingInput))
            {
                CharacterTransition(StateIdentifier.Charging, 14);
            }
            else if (aniS.animationFinished() && CurrentState.state == StateIdentifier.Charging)
            {
                CharacterTransition(StateIdentifier.AttackingC, 15);
            }
            else if (aniS.animationFinished() && CurrentState.state == StateIdentifier.AttackingC)
            {
                CharacterTransition(StateIdentifier.AttackCooldown3, 13);
            }
            else if (aniS.animationFinished() && CurrentState.state == StateIdentifier.AttackCooldown3)
            {
                CharacterTransition(StateIdentifier.Standing, 0);
            }
        }

        //Standing
        /*if (!Input.anyKey && (aniS.Finished || aniS.loop ))
        {
            CharacterTransition(StateIdentifier.Standing, 0);
        }*/
        //Debug.Log(CurrentState.state);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CurrentState.state == StateIdentifier.Walking || CurrentState.state == StateIdentifier.SideWalking)
        {
            grid.moveInDir(inpt);

            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if(Input.GetAxisRaw("Horizontal") < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
            }
        }
        
        if (CurrentState.state == StateIdentifier.Parrying)
        {
            if (aniS.getCurrentFrameImage() == aniS.getCurrentAnimationData().attackFrame)
            {
                if (allowHit)
                {
                    Collider[] hitColliders = Physics.OverlapBox(attackPoint.position, attackBoxSize, Quaternion.identity, attackLayers);
                    foreach (Collider c in hitColliders)
                    {
                        Debug.Log("Enemy Stunned!");
                    }
                    allowHit = false;
                }
            }
            else
            {
                allowHit = true;
            }
        }
        else if (CurrentState.state == StateIdentifier.Attacking1 ||
            CurrentState.state == StateIdentifier.Attacking2 ||
            CurrentState.state == StateIdentifier.Attacking3 ||
            CurrentState.state == StateIdentifier.AttackingC)
        {
            if (aniS.getCurrentFrameImage() == aniS.getCurrentAnimationData().attackFrame)
            {
                if (allowHit)
                {
                    Collider[] hitColliders = Physics.OverlapBox(attackPoint.position, attackBoxSize, Quaternion.identity, attackLayers);
                    foreach (Collider c in hitColliders)
                    {
                        Debug.Log("Enemy Hit!");
                        c.GetComponent<BanditControl>().gotHit();
                    }
                    allowHit = false;
                }
            }
            else
            {
                allowHit = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(attackPoint.position, attackBoxSize);
    }

    private void CharacterTransition(StateIdentifier s, int a)
    {
        if (CurrentState.CheckTransitionPosibility(s))
        {
            Debug.Log("pre transition: " + CurrentState.state);
            CurrentState = CurrentState.MakeTransition(s);
            aniS.ChangeAnimation(a + (int)CurrentMode);
            Debug.Log("post transition: " + CurrentState.state);
        }
    }

    public void gotStunned()
    {
        CharacterTransition(StateIdentifier.Stunned, 2);        
    }

    public void gotHit()
    {
        CharacterTransition(StateIdentifier.Hit, 3);
    }
}
