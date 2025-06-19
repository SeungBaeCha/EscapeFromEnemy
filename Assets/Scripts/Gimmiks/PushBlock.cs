using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlock : MonoBehaviour
{
    // 자식 오브젝트 설정로 하여 부모가 이동할 목표 위치 지정
    public Transform targetPoint;

    // 자식 오브젝트 방향으로 움직임 설정
    public float moveSpeed;

    // 돌아오기 전 대기시간
    public float returnDelay;
    
    // 기존 위치 저장
    Vector3 originalPosition;

    
    bool isMove = false;


    private void Start()
    {
        // 시작 위치 저장 
        originalPosition = transform.position;
    }





    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isMove)
        {
            isMove = true;
            StartCoroutine(MoveTarget(targetPoint.position, returnDelay));
        }
    }


    IEnumerator MoveTarget(Vector3 targetPosition, float delay)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // 스크립트를 가진 오브젝트의 움직임이 targetChild.position 쪽으로 moveSpeed만큼 움직인다.
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null; 

        }

        // 원래 위치로 돌아가기 위한 대기시간 (returnDelay의 값을 사용함)
        yield return new WaitForSeconds(returnDelay);

        while(Vector3.Distance(transform.position, originalPosition) > 0.1f)
        {
            // 스크립트를 가진 오브젝트의 움직임이 originalPosition 쪽으로 moveSpeed만큼 움직인다.
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);

            yield return null;
        }

        // 재사용 가능하도록 false로 지정
        isMove = false;
    }
}
