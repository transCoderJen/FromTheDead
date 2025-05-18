using System.Collections;
using TMPro;
using UnityEngine;

public class Entity : MonoBehaviour
{
     #region Debugging
    [Header("Debugging")]
    public TextMeshProUGUI debugStateText;
    #endregion

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb {get; private set; }
    public SpriteRenderer sr { get; private set; } 
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion


    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;

    #region Collision Info
    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    public Transform attackCheck2;
    public float attackCheckRadius2;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    #endregion

    public int facingDir {get; private set; } = 1;
    protected bool facingRight = true;
    public bool isDead = false;

    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        if (GetComponent<CapsuleCollider2D>() == null)
        {
            PolygonCollider2D cd;
            cd = GetComponent<PolygonCollider2D>();
        }
        else
        {
            
            cd = GetComponent<CapsuleCollider2D>();
        }
    }

    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        StartCoroutine(SlowEntity(_slowPercentage, _slowDuration));
    }

    public IEnumerator SlowEntity(float _slowPercentage, float _slowDuration)
    {
        anim.speed = 1 - _slowPercentage;
        yield return new WaitForSeconds(_slowDuration);
        ReturnDefaultSpeed();
    }
    
    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    public virtual void DamageEffect(bool _knockback)
    {
        if(_knockback)
            StartCoroutine("HitKnockback");  
    }

    protected virtual IEnumerator HitKnockback()
    {
        // if (GetComponent<Player>() != null)
        //     yield break;

        isKnocked = true;
        int modifier = 1;
        if (facingDir == 1 && transform.position.x > PlayerManager.Instance.player.transform.position.x ||
            facingDir == -1 && transform.position.x < PlayerManager.Instance.player.transform.position.x)
            modifier = -1;
        rb.linearVelocity = new Vector2(knockbackDirection.x * -facingDir * modifier, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;

        ZeroVelocity();
    }

    #region Velocity
    public void ZeroVelocity()
    {
         if(isKnocked)
             return;
            
        rb.linearVelocity = new Vector2(0, 0);
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if(isKnocked)
             return;
        
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    #region Colllision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance , whatIsGround);
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
        if (attackCheck2 != null)
            Gizmos.DrawWireSphere(attackCheck2.position, attackCheckRadius2);
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        // onFlipped();
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip(); 
    }

    public virtual void SetupDefaultFacingDir(int _direction)
    {
        facingDir = _direction;

        if (facingDir == -1)
            facingRight = false;
    }
    #endregion

    protected virtual void Update()
    {

    }

    public virtual void Die()
    {

    }


}