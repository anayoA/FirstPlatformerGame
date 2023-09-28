using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _anim;
    private SpriteRenderer _sprite;
    private BoxCollider2D _coll;
    
    [SerializeField] private LayerMask jumpableGround; 
    private float _dirX = 0f;
   [SerializeField] private float moveSpeed = 7f;
   [SerializeField] private float jumpForce = 7f;
   private static readonly int State = Animator.StringToHash("state");


   private enum MovementState {Idle, Running, Jumping, Falling}

   [SerializeField] private AudioSource jumpSoundEffect;
    
    // Start is called before the first frame update
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        _dirX = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector2(_dirX * moveSpeed, _rb.velocity.y);
      
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            _rb.velocity = new Vector3(_rb.velocity.x, jumpForce);
            jumpSoundEffect.Play();
        }
        
        UpdateAnimationState();

    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if (_dirX > 0f)
        {
            state = MovementState.Running;
            _sprite.flipX = false;
        }
        else if (_dirX < 0f)
        {
            state = MovementState.Running;
            _sprite.flipX = true;

        }
        else
        {
            state = MovementState.Idle;
        }

        if (_rb.velocity.y > .1f)
        {
            state = MovementState.Jumping;
        }
        if (_rb.velocity.y < -.1f)
        {
            state = MovementState.Falling;
        }
        _anim.SetInteger(State, (int)state);
    }

    private bool IsGrounded()
    {
        var bounds = _coll.bounds;
        return Physics2D.BoxCast(bounds.center, bounds.size, 0f, Vector2.down, .01f, jumpableGround);
    }
}
