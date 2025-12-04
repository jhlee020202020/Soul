using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 3.5f;
    public float runSpeed = 6f;

    private Animator anim;
    private bool isAttacking = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 공격 중일 경우 이동, 회전 금지 (애니메이션만 재생)
        if (isAttacking)
        {
            anim.SetFloat("Horizontal", 0);
            anim.SetFloat("Vertical", 0);
            anim.SetFloat("Blend", 0);
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        anim.SetFloat("Horizontal", h);
        anim.SetFloat("Vertical", v);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        anim.SetBool("IsRunning", isRunning);

        float speed = isRunning ? runSpeed : walkSpeed;

        Vector3 dir = new Vector3(h, 0, v).normalized;
        if (dir.magnitude > 0)
        {
            transform.forward = dir;
            transform.position += dir * speed * Time.deltaTime;
        }

        anim.SetFloat("Blend", new Vector2(h, v).magnitude);

        // 공격 입력
        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
            anim.SetBool("Attack", true);
        }
    }

    // 공격 애니메이션 끝에서 호출되는 이벤트
    public void OnAttackEnd()
    {
        isAttacking = false;
        anim.SetBool("Attack", false);
    }
}













