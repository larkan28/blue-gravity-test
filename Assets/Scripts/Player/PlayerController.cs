using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private bool m_isFacingLeft;
    private Vector2 m_moveDir;
    private Animator m_animator;
    private Rigidbody2D m_rigidbody2D;

    private readonly static int k_animSpeed = Animator.StringToHash("Speed");

    internal void Init()
    {
        m_animator = GetComponent<Animator>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    internal void Think()
    {
        float horAxis = Input.GetAxisRaw("Horizontal");
        float verAxis = Input.GetAxisRaw("Vertical");

        m_moveDir = new(horAxis, verAxis);
        m_animator.SetFloat(k_animSpeed, m_moveDir.sqrMagnitude * moveSpeed);

        Rotate();
    }

    internal void ThinkFixed()
    {
        Move();
    }

    private void Move()
    {
        m_rigidbody2D.velocity = m_moveDir.normalized * moveSpeed;
    }

    private void Rotate()
    {
        float speedX = m_moveDir.x;

        if (speedX == 0f)
            return;

        if (speedX < 0f && !m_isFacingLeft)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            m_isFacingLeft = true;
        }
        else if (speedX > 0f && m_isFacingLeft)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
            m_isFacingLeft = false;
        }
    }
}
