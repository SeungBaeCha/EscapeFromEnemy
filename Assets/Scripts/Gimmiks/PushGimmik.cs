using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushGimmik : MonoBehaviour
{
    // 밀쳐낼 부모 오브젝트 지정
    public GameObject parentObject;

    // 밀치는 힘 지정
    public float pushForce;

    // 비 활성화 유지시간 
    public float disableDuration;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // 부모 오브젝트의 리지드바디 기능을 가져옴
            Rigidbody playerRb = other.GetComponent<Rigidbody>();

            if(playerRb != null)
            {
                // pushDirection은 자식 오브젝트 transform.position 과 부모 오브젝트 transform.position 를
                // 정규화해서 뺀 값
                Vector3 pushDirection = (other.transform.position - parentObject.transform.position).normalized;
                // playerRb를 pushDirection 와 pushForce를 곱한값으로 순간적으로 힘을준다
                playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);

                //Debug.Log("밀쳐짐");

                StartCoroutine(ReturnEnable());
            }
            
        }
    }


    IEnumerator ReturnEnable()
    {

        // GameObject.setActive(false)로 지정하면 코루틴 자체가 꺼지기때문에,
        // collider를 껏다가 키는식으로 진행했음.
         
        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(disableDuration);

        GetComponent<Collider>().enabled = true;

        //Debug.Log("밀치기 활성화");

    }

}
