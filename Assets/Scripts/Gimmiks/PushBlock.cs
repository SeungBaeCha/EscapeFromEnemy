using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlock : MonoBehaviour
{
    // �ڽ� ������Ʈ ������ �Ͽ� �θ� �̵��� ��ǥ ��ġ ����
    public Transform targetPoint;

    // �ڽ� ������Ʈ �������� ������ ����
    public float moveSpeed;

    // ���ƿ��� �� ���ð�
    public float returnDelay;
    
    // ���� ��ġ ����
    Vector3 originalPosition;

    
    bool isMove = false;


    private void Start()
    {
        // ���� ��ġ ���� 
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
            // ��ũ��Ʈ�� ���� ������Ʈ�� �������� targetChild.position ������ moveSpeed��ŭ �����δ�.
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null; 

        }

        // ���� ��ġ�� ���ư��� ���� ���ð� (returnDelay�� ���� �����)
        yield return new WaitForSeconds(returnDelay);

        while(Vector3.Distance(transform.position, originalPosition) > 0.1f)
        {
            // ��ũ��Ʈ�� ���� ������Ʈ�� �������� originalPosition ������ moveSpeed��ŭ �����δ�.
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);

            yield return null;
        }

        // ���� �����ϵ��� false�� ����
        isMove = false;
    }
}
