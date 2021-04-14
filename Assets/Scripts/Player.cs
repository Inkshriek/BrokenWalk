using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors;

[RequireComponent(typeof(ActorController))]
public class Player : ActorMethods, IActor, IHealth {

    public string DisplayName { get; set; }

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float health;
    [SerializeField] private float healthDeficit;
    [SerializeField] private float maxHealth;
    [SerializeField] private float bleedRate;

    private Rigidbody2D rb;
    private bool jumping;

    public ActorController Controller { get; private set; }
    public bool Active { get; set; }
    public PlayerState State { get; set; }

    public enum PlayerState {
        //This is just a set of states intended exclusively for the player.
        Crouching,
        Walking,
        Running,
        Pushing,
        LedgeGrabbed
    }
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
    public float Health { get { return health; } set { health = Mathf.Clamp(value, 0, MaxHealth); } }
    public float HealthDeficit { get { return healthDeficit; } set { healthDeficit = value; } }
    public float MaxHealth { get { return maxHealth; } set { maxHealth = Mathf.Max(0, value); } }
    public float BleedRate {get { return bleedRate; } set { bleedRate = Mathf.Abs(value); } }

    private void Awake() {
        DisplayName = "Player";

        rb = GetComponent<Rigidbody2D>();
        Controller = GetComponent<ActorController>();

        Active = true;
        State = PlayerState.Walking;
        jumping = false;
    }

    private void Update() {
        if (Active) {
            if (!Busy) {
                if (Controller.IsTouchingGround && Input.GetKeyDown(GameController.jumpKey)) {
                    Controller.ApplyJump(JumpHeight);
                    jumping = true;
                    rb.gravityScale /= 1.5f;
                }

                if (Input.GetAxisRaw("Vertical") < 0) {
                    State = PlayerState.Crouching;
                }
                else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                    State = PlayerState.Running;
                }
                else {
                    State = PlayerState.Walking;
                }

                if (Controller.IsTouchingGround) {
                    Controller.InputMotion = new Vector2(Input.GetAxisRaw("Horizontal") * Speed, 0);
                }
                else {
                    Controller.InputMotion = new Vector2(Input.GetAxisRaw("Horizontal") * walkSpeed, 0);
                }

                if (jumping && (!Input.GetKey(GameController.jumpKey) || rb.velocity.y < 0)) {
                    rb.gravityScale *= 1.5f;
                    jumping = false;
                }
            }

            BleedHealth();
        }

        AnimationSelector();
    }

    public void OnDamage(float value, DamageType type) {
        HealthDeficit += value;
        Controller.InputMotion = new Vector2(2, 0);
        Controller.ApplyJump(new Vector2(5,5));
    }

    private void BleedHealth() {
        //This whole method here intends to adjust the current health according to the deficit, making it "bleed" over time until the deficit is zero.

        float adjustment;
        if (HealthDeficit > 0) {
            adjustment = Mathf.Min(HealthDeficit, BleedRate * Time.deltaTime);
        }
        else if (HealthDeficit < 0) {
            adjustment = HealthDeficit;
        }
        else {
            adjustment = 0;
            //*lip smack* nice
        }
        
        HealthDeficit -= adjustment;
        Health -= adjustment;
    }

    private void AnimationSelector() {
        
    }
}
