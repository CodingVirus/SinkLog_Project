using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Unit2D
{
    #region Parameter
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public LayerMask jumpCheckLayer;
    [SerializeField] public Transform myCursor;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private TrailRenderer myTrail;
    [SerializeField] private PhysicsMaterial2D noFriction;
    [SerializeField] private PhysicsMaterial2D fullFriction;
    [SerializeField] private DashCount dashControl;

    // Component reference
    private CapsuleCollider2D myCapsule;

    // move
    private float moveX;
    private float moveY;
    /*[SerializeField] public float moveSpeed = 3.0f;*/
    // jump
    [SerializeField] private float jumpPower;
    private float maxJumpPower;
    private float minJumpPower;
    private float jumpHight;
    // collider check
    [SerializeField] private float groundCheckRadius;
    // dash
    private float dashingPower = 30.0f;
    private float dashingTime = 0.1f;
    // Slope
    [SerializeField] private float slopeCheckDistance;
    private float slopeDownAngle;
    private float slopeDownAngleOld;
    private float slopeSideAngle;

    // Vector
    private Vector2 dashingDir;
    private Vector2 newVelocity;
    private Vector2 newForce;
    private Vector2 colliderSize;
    private Vector2 slopeNormalPerp;
    private Vector2 spawnPos = new Vector2(0, 0.1f);

    // bool
    [SerializeField] bool isFlipX;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isPlatFormed;
    [SerializeField] bool isOverLapPlatForm = false;
    [SerializeField] bool isAir;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isOnSlope;
    [SerializeField] private bool isDialogueStart = false;
    #endregion

    #region Player Stat
    #endregion
    void Awake()
    {
        myTrail = GetComponent<TrailRenderer>();
    }
    void Start()
    {
        Initalize();
        myCursor = CrossHairScript.Instance.gameObject.transform;
        myCapsule = GetComponent<CapsuleCollider2D>();
        colliderSize = myCapsule.size;
        
    }

    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        #region Jump
        if (Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.S) && !isAir && !isDialogueStart)
        {
            if (!isGrounded && !isPlatFormed)
            {
                myAnim.SetBool("isAir", false);
            }
            else
            {
                StartCoroutine(Jumping());
            }

        }
        #endregion
        #region Under Jump
        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space) && isPlatFormed)
        {
            if (!isGrounded)
            {
                myCollider.isTrigger = true;
            }
        }
        #endregion
        #region Dash
        if (Input.GetMouseButtonDown(1) && canDash && dashControl.dashCount > 0)
        {
            dashControl.UseDash();
            isDashing = true;
            myAnim.SetTrigger(hashJump);
            canDash = false;
            myTrail.emitting = true;
            dashingDir = new Vector2(myCursor.position.x - transform.position.x, myCursor.position.y - transform.position.y);
            if (dashingDir == Vector2.zero)
            {
                dashingDir = new Vector2(transform.localScale.x, 0.0f);
            }
            StartCoroutine(stopDashing());
        }
        if (isDashing)
        {
            myRigid.velocity = dashingDir.normalized * dashingPower;
            return;
        }
        canDash = true;
        #endregion
        // Reset Location (Debugging)
        if (Input.GetKeyDown(KeyCode.F5))
        {
            transform.position = spawnPos;
        }

    }
    private void FixedUpdate()
    {
        if (!isDialogueStart)
        {
            Move();
        }
        GroundCheck();
        SlopeCheck();

        // 땅에 닿았는지 공중에 있는지 확인
        #region JumpRayCheck
        if (myRigid.velocity.y < 0)
        {
            Debug.DrawRay(transform.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector3.down, 0.5f, jumpCheckLayer);
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1.0f)
                {
                    myAnim.SetBool("isAir", false);
                    isAir = false;
                }
            }
        }
        #endregion
    }

    void Move()
    {
        if (!Mathf.Approximately(moveX, 0.0f))
        {
            myAnim.SetBool("isMoving", true);
        }
        else
        {
            myAnim.SetBool("isMoving", false);
        }

        transform.Translate(Vector2.right * moveX * basicMoveSpeed * Time.deltaTime);
        /*newVelocity.Set(moveSpeed * moveX, myRigid.velocity.y);
        myRigid.velocity = newVelocity;*/

        if (isPlatFormed && !isOnSlope)
        {
            /*newVelocity.Set(moveSpeed * moveX, 0.0f);
            myRigid.velocity = newVelocity;*/
            transform.Translate(Vector2.right * moveX * basicMoveSpeed * Time.deltaTime);
            transform.Translate(Vector2.up * 0.0f * Time.deltaTime);
        }
        else if(isPlatFormed && isOnSlope)
        {
            /*newVelocity.Set(moveSpeed * slopeNormalPerp.x * -moveX, moveSpeed * slopeNormalPerp.y * -moveX);
            myRigid.velocity = newVelocity;*/
            transform.Translate(Vector2.right * slopeNormalPerp.x * -moveX * basicMoveSpeed * Time.deltaTime);
            transform.Translate(Vector2.up * slopeNormalPerp.y * -moveX * basicMoveSpeed * Time.deltaTime);
        }
        else if(!isPlatFormed)
        {
            transform.Translate(Vector2.right * moveX * basicMoveSpeed * Time.deltaTime);
            transform.Translate(Vector2.up * moveY * Time.deltaTime);
        }

        if (isFlipX && myCursor.position.x - transform.position.x < 0.0f)
        {
            /*myRender.flipX = true;*/
            transform.localScale = new Vector3(-1, 1, 1);

            isFlipX = false;
        }
        else if (!isFlipX && myCursor.position.x - transform.position.x > 0.0f)
        {
            /*myRender.flipX = false;*/
            transform.localScale = new Vector3(1, 1, 1);

            isFlipX = true;
        }
    }
    void GroundCheck()
    {
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
            myCollider.isTrigger = false;
            myAnim.SetBool("isAir", false);
        }
    }
    /*void SideGroundCheck()
    {
        isGrounded = false;

    }*/

    void PlatFormCheck(Collision2D collision)
    {
        isPlatFormed = false;
        if ((1 << collision.gameObject.layer & platformLayer) != 0)
        /*Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, platformLayer);*/
        {
            CapsuleCollider2D capsule = myCollider as CapsuleCollider2D;
            if (capsule != null)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, capsule.size.y, platformLayer);
                if (hit.transform != null)
                {
                    isPlatFormed = true;
                    myAnim.SetBool("isAir", false);
                }
            }
        }
    }

    void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, colliderSize.y / 2);

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }
    void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, platformLayer);
        RaycastHit2D slopeHitback = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, platformLayer);

        if(slopeHitFront)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if(slopeHitback)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitback.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }
    }
    void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, platformLayer);
        if(hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(slopeDownAngle != slopeDownAngleOld)
            {
                isOnSlope = true;
            }

            slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }

        if(isOnSlope && moveX == 0.0f)
        {
            myRigid.sharedMaterial = fullFriction;
        }
        else
        {
            myRigid.sharedMaterial = noFriction;
        }
    }
    IEnumerator Jumping()
    {
        isAir = true;
        myAnim.SetTrigger("Jump");
        myRigid.AddForce(Vector3.up * 100.0f * jumpPower);

        while (myRigid.velocity.y > 0.0f || Mathf.Approximately(myRigid.velocity.y, 0.0f) | isOverLapPlatForm)
        {
            myCollider.isTrigger = true;
            yield return null;
        }
        myCollider.isTrigger = false;
    }
    IEnumerator stopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        myTrail.emitting = false;
        isDashing = false;
        myRigid.velocity = myCursor.position - transform.position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlatFormCheck(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        PlatFormCheck(collision);
        if ((1 << collision.gameObject.layer & platformLayer) != 0)
        {
            myCollider.isTrigger = false;
            if(isDashing)
            {
                myCollider.isTrigger = true;
            }
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer & platformLayer) != 0)
        {
            myAnim.SetTrigger("Jump");
            if(isDashing)
            {
                myCollider.isTrigger = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer & platformLayer) != 0)
        {
            myCollider.isTrigger = true;
        }

        if ((1 << collision.gameObject.layer & groundLayer) != 0)
        {
            transform.Translate(Vector2.up * 1.0f * Time.deltaTime);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer & platformLayer) != 0)
        {
            isOverLapPlatForm = true;
            if (isGrounded)
            {
                isOverLapPlatForm = false;
                myCollider.isTrigger = false;
            }
        } 

        /*if ((1 << collision.gameObject.layer & groundLayer) != 0)
        {
            CapsuleCollider2D capsule = myCollider as CapsuleCollider2D;
            if (capsule != null)
            {
                RaycastHit2D lefthit = Physics2D.Raycast(transform.position, Vector2.left, capsule.size.y, platformLayer);
                if (lefthit.transform != null)
                {
                    
                }
            }
                
            transform.Translate(Vector2.down * 0.5f);
        }*/
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isOverLapPlatForm = false;
        /*Debug.Log(myRigid.velocity.y);*/
        if (myRigid.velocity.y < 0.0f) myCollider.isTrigger = false;
    }


    public void IsDialogueEnd()
    {
        isDialogueStart = false;
    }

    public void IsDialogueStrat()
    {
        isDialogueStart = true;
    }
}
