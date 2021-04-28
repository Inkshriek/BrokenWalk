using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors;

[RequireComponent(typeof(ActorController))]
public class Player : Actor {

    [SerializeField] private float walkSpeed = 3;
    [SerializeField] private float runSpeed = 6;
    [SerializeField] private float crouchSpeed = 2;
    [SerializeField] private float jumpHeight = 10;
    [SerializeField] private float fear = 0;
    [SerializeField] private float fearThreshold = 240;
    [SerializeField] private float reliefDelay = 2;
    [SerializeField] private bool fearless = false;
    [SerializeField] private GameObject sprite;
    private Rigidbody2D rb;
    private bool jumping = false;
    private float fearRate = -1;
    private Coroutine reliefRoutine;
    private Pickup heldObject;
    private int facingDirection = 1;
    private Vector3 safety;

    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public AudioSource audioSource;

    public enum PlayerState {
        //This is just a set of states intended exclusively for the player.
        Crouching,
        Walking,
        Running,
        Carrying
    }
    public PlayerState State { get; private set; }
    public float Speed {
        get {
            switch (State) {

                case PlayerState.Crouching:
                return crouchSpeed;

                case PlayerState.Running:
                return runSpeed;

                default:
                return walkSpeed;
            }
        }
        set {
            float multRun = runSpeed / walkSpeed;
            float multCrouch = crouchSpeed / walkSpeed;
            walkSpeed = value;
            runSpeed = value * multRun;
            crouchSpeed = value * multCrouch;
        }
    }
    public float JumpHeight { get { return jumpHeight; } set { jumpHeight = value; } }
    public float Fear { get { return fear; } set { fear = Mathf.Clamp(value, 0, fearThreshold); } }
    public float FearThreshold { get { return fearThreshold; } set { fearThreshold = Mathf.Max(0, value); } }
    public float FearRate { get { return fearRate; } private set { fearRate = value; } }
    public float ReliefDelay { get { return reliefDelay; } set { reliefDelay = Mathf.Max(0, value); } }
    public bool Fearless { 
        get { return fearless; } 
        set {
            fearless = value; 
            fearRate = 0;
            fear = 0;
        } 
    }

    private void Awake() {
        DisplayName = "Wanderer";

        rb = GetComponent<Rigidbody2D>();
        Controller = GetComponent<ActorController>();

        Active = true;
        State = PlayerState.Walking;
        safety = transform.position;
    }

    private void Update() {
        if (Active) {
            if (!Busy) {
                if (State != PlayerState.Carrying) {
                    if (Controller.IsTouchingGround && Input.GetKeyDown(KeyCode.Space)) {
                        Controller.ApplyJump(JumpHeight);
                        jumping = true;
                        rb.gravityScale /= 1.5f;
                    }

                    if (Input.GetAxisRaw("Vertical") < 0) State = PlayerState.Crouching;
                    else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) State = PlayerState.Running;
                    else State = PlayerState.Walking;

                    if (Input.GetKeyDown(KeyCode.E)) {
                        RaycastHit2D check = Physics2D.Raycast((Vector2)transform.position + new Vector2(0,0.5f), Vector2.right * facingDirection, 1f, LayerMask.GetMask("Pickup"));
                        if (check) {
                            Pickup pickup = check.collider.GetComponent<Pickup>();
                            if (pickup != null) {
                                heldObject = pickup;
                                State = PlayerState.Carrying;
                                animator.SetTrigger("PickUp");
                                pickup.PickUp();
                            }
                        }
                    }
                }
                else if (Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.E)) {
                    State = PlayerState.Walking;
                    heldObject.Drop();
                    heldObject = null;
                    if (Input.GetKey(KeyCode.Space)) {
                        Controller.ApplyJump(JumpHeight);
                        jumping = true;
                        rb.gravityScale /= 1.5f;
                    }
                }

                if (Controller.IsTouchingGround) Controller.InputMotion = new Vector2(Input.GetAxisRaw("Horizontal") * Speed, 0);
                else Controller.InputMotion = new Vector2(Input.GetAxisRaw("Horizontal") * walkSpeed, 0);

                if (jumping && (!Input.GetKey(KeyCode.Space) || rb.velocity.y < 0)) {
                    rb.gravityScale *= 1.5f;
                    jumping = false;
                }
                
                if (Input.GetAxisRaw("Horizontal") != 0) facingDirection = (int)Mathf.Sign(Input.GetAxisRaw("Horizontal"));

                if (heldObject != null) {
                    heldObject.HoldDirection = facingDirection;
                    heldObject.transform.position = (Vector2)transform.position + new Vector2(facingDirection, 0.5f);
                    heldObject.transform.localScale = new Vector3(facingDirection, heldObject.transform.localScale.y, heldObject.transform.localScale.z);
                }
            }

            Fear += Time.deltaTime * FearRate * 60;
            if (Fear >= FearThreshold && !Fearless) {
                GameController.GameOver();
                Active = false;
            }

            if (sprite != null)
            sprite.transform.localScale = new Vector3(Mathf.Abs(sprite.transform.localScale.x) * facingDirection, sprite.transform.localScale.y, sprite.transform.localScale.z);
        }
        AnimationTrigger();
    }

    public void Frighten() {
        if (Fearless) return;

        animator.SetBool("AnimFear", true);
        audioSource.Play();
        FearRate = 1;
        Speed = 1; //This is just an ez fix for now. Needs to be replaced with something relative.
        if (reliefRoutine != null) StopCoroutine(reliefRoutine);
    }

    public void Relieve() {
        

        animator.SetBool("AnimFear", false);
        audioSource.Stop();
        FearRate = 0;
        Speed = 3; //This too.
        if (reliefRoutine != null) StopCoroutine(reliefRoutine);
        reliefRoutine = StartCoroutine(ReliefTime());
    }

    private IEnumerator ReliefTime() {
        yield return new WaitForSeconds(ReliefDelay);
        FearRate = -1;
    }

    //sending movment and input information from this script to the animator controller
    private void AnimationTrigger()
    {
        //using speed to trigger animation states, might be better using player state, or converting fpeed float to int, but I am a fool
        animator.SetFloat("Speed", Speed);

        //juuump
        if (jumping == true) animator.SetTrigger("Jump");

        //fliping sprit horizontally with movment direction
        if (facingDirection == 1) spriteRenderer.flipX = false;
        if (facingDirection == -1) spriteRenderer.flipX = true;
    }
}
