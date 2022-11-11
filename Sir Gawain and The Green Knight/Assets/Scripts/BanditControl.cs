using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditControl : MonoBehaviour
{
    public State CurrentState;

    public float speed;

    [SerializeField]
    private AnimationScript aniS;

    [SerializeField]
    private Transform attackPoint;

    [SerializeField]
    private Vector3 attackBoxSize;

    [SerializeField]
    private LayerMask attackLayers;

    void Awake()
    {
        //Definitions

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

        //State transitions
        standing.AddTransitions(walking, stunned, hit, blocking, reciving, parrying, attacking1, charging);
        walking.AddTransitions(standing, stunned, hit, blocking, reciving, parrying, attacking1, charging);
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

    // Update is called once per frame
    void Update()
    {
        //Standing
        if (aniS.Finished)
        {
            CharacterTransition(StateIdentifier.Standing, 0);
        }
    }

    private void CharacterTransition(StateIdentifier s, int a)
    {
        if (CurrentState.CheckTransitionPosibility(s))
        {
            CurrentState = CurrentState.MakeTransition(s);
            aniS.ChangeAnimation(a);
            aniS.change = true;
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
