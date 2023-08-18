using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    public Rigidbody2D rigid;
    public Animator anime;
    public Transform detectGround;
    public LayerMask groundDetector;

    public float speed;
    public float jump;
    public float horizontalInput;

    bool isFacingRight = true;
    public bool isOnGround;

    public int extraJumps = 1;

    public enum MovementState { idle, run, jump, fall }

    [SerializeField] public AudioSource jumpSoundEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        anime = GetComponent<Animator>();

        isOnGround = Physics2D.OverlapCircle(detectGround.position, 0.2f, groundDetector);

        if (isOnGround == true)
        {
            extraJumps=1;
        }

        //this one is for jumping
        if (Input.GetButtonDown("Jump") && isOnGround == true)
        {
            jumpSoundEffect.Play();
            rigid.velocity = Vector2.up * 12;
        }

        //decrement jumping
        if (Input.GetButtonDown("Jump") && isOnGround == false && extraJumps > 0)
        {
            rigid.velocity = Vector2.up * 12;
            extraJumps--;
        }

        // Flip the character's direction if necessary
        if ((horizontalInput > 0 && !isFacingRight) || (horizontalInput < 0 && isFacingRight))
        {
            Flip();
        }

        UpdateAnimation();

    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rigid.velocity = new Vector2(horizontalInput * speed, rigid.velocity.y);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        // Multiply the character's local scale by -1 to flip it
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void UpdateAnimation()
    {
        MovementState state;

        if (horizontalInput > 0f || horizontalInput < 0f)
        {
            state = MovementState.run;
        }
        else
        {
            state = MovementState.idle;
        }

        if (!isOnGround)
        {
            if (rigid.velocity.y > 0.1f)
            {
                state = MovementState.jump;
            }
            else if (rigid.velocity.y < -0.1f)
            {
                state = MovementState.fall;
            }
        }

        anime.SetInteger("state", (int)state);
    }

}
