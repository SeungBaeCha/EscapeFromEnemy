using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushGimmik : MonoBehaviour
{
    // ���ĳ� �θ� ������Ʈ ����
    public GameObject parentObject;

    // ��ġ�� �� ����
    public float pushForce;

    // �� Ȱ��ȭ �����ð� 
    public float disableDuration;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // �θ� ������Ʈ�� ������ٵ� ����� ������
            Rigidbody playerRb = other.GetComponent<Rigidbody>();

            if(playerRb != null)
            {
                // pushDirection�� �ڽ� ������Ʈ transform.position �� �θ� ������Ʈ transform.position ��
                // ����ȭ�ؼ� �� ��
                Vector3 pushDirection = (other.transform.position - parentObject.transform.position).normalized;
                // playerRb�� pushDirection �� pushForce�� ���Ѱ����� ���������� �����ش�
                playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);

                //Debug.Log("������");

                StartCoroutine(ReturnEnable());
            }
            
        }
    }


    IEnumerator ReturnEnable()
    {

        // GameObject.setActive(false)�� �����ϸ� �ڷ�ƾ ��ü�� �����⶧����,
        // collider�� ���ٰ� Ű�½����� ��������.
         
        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(disableDuration);

        GetComponent<Collider>().enabled = true;

        //Debug.Log("��ġ�� Ȱ��ȭ");

    }

}
