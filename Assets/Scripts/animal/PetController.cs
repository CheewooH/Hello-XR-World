using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : MonoBehaviour
{
    public float moveSpeed = 1f;      // 걷는 속도
    public float walkRadius = 3f;     // 이 반경 내에서만 돌아다님
    public float minIdleTime = 2f;    // 최소 대기 시간
    public float maxIdleTime = 5f;    // 최대 대기 시간

    private Animator animator;
    private Vector3 startPosition;      // 처음 소환된 위치
    private Vector3 targetPosition;     // 다음 이동할 목표 위치
    private bool isWalking = false;     // 현재 걷고 있는지 상태 저장
    private Coroutine walkRoutine;  // 코루틴 제어를 위한 변수

    void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position; // 처음 위치 저장

        // 행동 시작
        StartCoroutine(WalkRoutine());
    }

    void Update()
    {
        if (isWalking)
        {
            // 목표 지점으로 천천히 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 목표 지점을 향해 몸을 회전
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            // 목표 지점에 거의 도착했다면 멈춤
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isWalking = false;
                animator.SetBool("IsWalking", false);
            }
        }
    }

    public void OnTouched()
    {
        if (walkRoutine != null)
        {
            StopCoroutine(walkRoutine);
        }

        animator.SetBool("IsWalking", false);
        animator.SetTrigger("Bounce");

        walkRoutine = StartCoroutine(WalkRoutine());
    }

    IEnumerator WalkRoutine()
    {
        while (true)
        {
            // 대기
            float idleTime = Random.Range(minIdleTime, maxIdleTime);
            yield return new WaitForSeconds(idleTime);

            // 새로운 목표 지점 설정
            Vector2 randomPoint = Random.insideUnitCircle * walkRadius;
            targetPosition = startPosition + new Vector3(randomPoint.x, 0, randomPoint.y);
            // ChooseNewDestination();

            // 걷기 시작
            isWalking = true;
            animator.SetBool("IsWalking", true); // "Walk" 애니메이션 시작

            yield return new WaitUntil(() => !isWalking);
        }
    }
    void ChooseNewDestination() // ARPlane 위에 있는 목표 지점 선택
    {
        
    }
}
