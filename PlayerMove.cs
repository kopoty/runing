using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rg;
    private SpriteRenderer sr; // спрайт для игрока
    private SpriteRenderer je; // спрайт для эффекта прыжка
    private SpriteRenderer re; // спрайт для эффекта бега
    private SpriteRenderer re2; // спрайт для эффекта бега2
    private BoxCollider2D legsCollider;
    private Collider2D sidesCollider;
    private PolygonCollider2D slideCollider;
    public Animator animator;
    public GameObject jumpEffect;
    public GameObject runEffect;
    public GameObject runEffect2;
   
    public LayerMask groundLayer;

    public float firstStepTime;     // первый временной шаг после которого игрок ускоряется
    public float secondStepTime;    // второй временной шаг после которого добавляется анимация
    public float speed;
    private float timerOfMoving;
    private float addSpeed;

    public float slideForce;
    public float slideMaxTime;
    private float slideCurrentTime;

    public float jump;
    public float jumpTime;
    public bool isJumping = false;
    public int countJumps; // задаем кол-во прыжков
    private int maxJumps;
    private float jumpForcePerTime;
    
    private float gravity;
    private float timerOfDown;

    ////////////////////////////////////////////////////////////////////////////////////


    private void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        re = runEffect.GetComponent<SpriteRenderer>();
        re2 = runEffect2.GetComponent<SpriteRenderer>();

        sidesCollider = GetComponent<CapsuleCollider2D>();
        legsCollider = GetComponent<BoxCollider2D>();
        slideCollider = GetComponent<PolygonCollider2D>();
        slideCollider.isTrigger = true;
        
        timerOfMoving = 1f;
        maxJumps = countJumps;
        runEffect.SetActive(false);
        jumpForcePerTime = jumpTime;
        gravity = rg.gravityScale;
        timerOfDown = 0.1f;
        slideCurrentTime = slideMaxTime;
    }

    // Update is called once per frame
    void Update()
    {
        
        fastRuning();
        Jump();
        noSlide();
    }


    private void fastRuning()   // бег и ускорение
    {
        
        rg.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rg.velocity.y);

        if(isGrounded() && rg.velocity.x !=0)
            animator.SetBool("isRuning", true);
        else  animator.SetBool("isRuning", false);

        FlipAxisCheck(Input.GetAxisRaw("Horizontal"), sr);
        FlipAxisCheck(Input.GetAxisRaw("Horizontal"), re);
        FlipAxisCheck(Input.GetAxisRaw("Horizontal"), re2);

        

        if (rg.velocity.x != 0f && !isWall() )
        {
            timerOfMoving += Time.deltaTime;

        } else timerOfMoving = 1f;



        if (timerOfMoving > firstStepTime) // первый этап ускорения
        { 
            addSpeed += Time.deltaTime;
            rg.velocity = new Vector2(rg.velocity.x +(Input.GetAxisRaw("Horizontal") *addSpeed), rg.velocity.y);
            
            animator.SetBool("StepRuning1", true);

            // Slide();
            

            if(isGrounded()) //активируем эффект пыли из под ног при беге
            {
                runEffect.SetActive(true);
                runEffect.transform.position = new Vector2(transform.position.x - Input.GetAxisRaw("Horizontal")*legsCollider.bounds.size.x, runEffect.transform.position.y);
                
            }else runEffect.SetActive(false);

            

            if (timerOfMoving > secondStepTime ) // второй этап ускорения
            {
                // animator.SetBool("StepRuning1", false);
                animator.SetBool("StepRuning2", true);

                if(isGrounded()) //активируем эффект пыли из под ног при беге
                {
                    runEffect2.SetActive(true);
                    runEffect2.transform.position = new Vector2(transform.position.x - Input.GetAxisRaw("Horizontal")*legsCollider.bounds.size.x, runEffect2.transform.position.y);
                
                }else runEffect2.SetActive(false);

            } else animator.SetBool("StepRuning2", false);
        } else 
        {
            runEffect.SetActive(false);
            runEffect2.SetActive(false);
            animator.SetBool("StepRuning1", false);
            animator.SetBool("StepRuning2", false);
            addSpeed = 1f;
        }        
    }

    private void Jump()     //прыжок
    {
        if (Input.GetKeyDown(KeyCode.Space) && isJumping == false)
        {
            rg.velocity = new Vector2(rg.velocity.x, jump);
            countJumps--;
            animator.SetBool("jumping", true);
            ShowJumpEffect();
        }

        if(jumpForcePerTime > 0)
            if (Input.GetKey(KeyCode.Space) && !isGrounded())
                {
                    rg.velocity = new Vector2(rg.velocity.x, jump);
                    jumpForcePerTime -= Time.deltaTime;
                } 
        
        if(Input.GetKeyUp(KeyCode.Space) && countJumps > 0) jumpForcePerTime = jumpTime;
        
        if(countJumps == 0) 
        {            
            isJumping = true;
            jumpForcePerTime -= Time.deltaTime;
            if (isGrounded())
            {
                 animator.SetBool("jumping", false);
                 isJumping = false;
                 countJumps = maxJumps;
                 jumpForcePerTime = jumpTime;
            }
        }

        if (isGrounded() && rg.velocity.y <= 0)
        {
            animator.SetBool("jumping", false);
            isJumping = false;
            countJumps = maxJumps;
            jumpForcePerTime = jumpTime;
        }
    }
    
    private void FlipAxisCheck (float axis, SpriteRenderer flipSprite )    // Поворачивает персонажа по направлению движения
    {
        if (axis < 0f)
        {
            flipSprite.flipX = true;
        }
        else if (axis > 0f)
        {
            flipSprite.flipX = false;
        }
    }
   
    private bool isGrounded()   // проверяем стоит ли персонаж на поверхности
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(legsCollider.bounds.center, legsCollider.bounds.size, 0, Vector2.down, 0.2f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool isWall()   // проверяем упирается ли персонаж в стену
    {
        RaycastHit2D raycastHit_L = Physics2D.CapsuleCast(sidesCollider.bounds.center, sidesCollider.bounds.size, CapsuleDirection2D.Vertical, 0, Vector2.left, 0.1f, groundLayer);
        RaycastHit2D raycastHit_R = Physics2D.CapsuleCast(sidesCollider.bounds.center, sidesCollider.bounds.size, CapsuleDirection2D.Vertical, 0, Vector2.right, 0.1f, groundLayer);
        return (raycastHit_L.collider != null || raycastHit_R.collider != null);
    }
   
    private void ShowJumpEffect() // запускаем эффект прыжка
    {
            GameObject e  = Instantiate(jumpEffect) as GameObject;
            je = e.GetComponent<SpriteRenderer>();
            e.transform.position = new Vector2(transform.position.x, transform.position.y - 0.5f);

            if(maxJumps - countJumps == 1) je.color = new Color(255, 255, 255);
            else je.color = new Color(0, 182, 255);

            Destroy(e, 0.4f);
    }
    
    private void noSlide() // убираем гравитацию пока никакие клавиши не нажаты, чтобы персонаж не скользил
    {
        timerOfDown -= Time.deltaTime;
       
        if( timerOfDown <=0)
            if( !Input.anyKeyDown && isGrounded())
            {
                    rg.gravityScale = 0;
                
            } else 
            {
                rg.gravityScale = gravity;
                timerOfDown = 0.05f;
            }
    }

    private void Slide()
    {
        if( Input.GetButtonDown("Slide") && slideCurrentTime >0 )
        {
            slideCurrentTime -= Time.deltaTime;
            sidesCollider.isTrigger = true;
            legsCollider.isTrigger = true;
            slideCollider.isTrigger = false;
            rg.AddForce(new Vector2(rg.velocity.x + slideForce, 0), ForceMode2D.Impulse);
            animator.SetBool("Slide", true);
           
        }
        else
        {
            animator.SetBool("Slide", false);

            slideCurrentTime = slideMaxTime;
            sidesCollider.isTrigger = false;
            legsCollider.isTrigger = false;
            slideCollider.isTrigger = true;

        }
    }

}
