using System.Collections;
using UnityEngine;

public class PetController : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public float walkRadius = 2.0f;
    public float minIdleTime = 2.0f;
    public float maxIdleTime = 5.0f;

    private Animator animator;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Coroutine walkCoroutine;

    void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        targetPosition = transform.position; // 시작할 땐 움직이지 않음

        walkCoroutine = StartCoroutine(WalkRoutine());
    }
    
    void Update()
    {
        if (animator.GetBool("IsWalking"))
        {
            // 목표 지점으로 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 목표 지점을 향해 회전
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
            // 목표 지점에 도착하면 멈춤
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                animator.SetBool("IsWalking", false);
            }
        }
    }

    // 터치시 상호작용
    public void OnTouched()
    {
        if (walkCoroutine != null)
        {
            StopCoroutine(walkCoroutine);
        }
        targetPosition = transform.position;
        animator.SetBool("IsWalking", false);
        animator.SetTrigger("Bounce");

        walkCoroutine = StartCoroutine(WalkRoutine());
    }

    // 이동 명령을 받아서 지정된 위치로 이동
    public void MoveTo(Vector3 newDestination)
    {
        if (walkCoroutine != null)
        {
            StopCoroutine(walkCoroutine);
        }

        targetPosition = newDestination;

        animator.SetBool("IsWalking", true);

        walkCoroutine = StartCoroutine(ReturnWalkRoutine());
    }

    // 펫이 돌아다니는 코루틴
    private IEnumerator WalkRoutine()
    {
        while (true)
        {
            float idleTime = Random.Range(minIdleTime, maxIdleTime); 
            yield return new WaitForSeconds(idleTime);

            ChooseNewDestination();
            animator.SetBool("IsWalking", true);

            yield return new WaitUntil(() => !animator.GetBool("IsWalking")); // 이동이 끝날 때까지 대기
        }
    }

    // 이동명령 후 다시 돌아다니는 코루틴
    private IEnumerator ReturnWalkRoutine() 
    {
        yield return new WaitUntil(() => !animator.GetBool("IsWalking"));

        walkCoroutine = StartCoroutine(WalkRoutine());
    }

    // 새로운 목적지를 선택하는 메소드
    private void ChooseNewDestination()
    {
        Vector2 randomPoint = Random.insideUnitCircle * walkRadius;
        targetPosition = startPosition + new Vector3(randomPoint.x, 0, randomPoint.y);
    }
}
