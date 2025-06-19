using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearGimmik : MonoBehaviour
{
    
    public float disappearDelay;    // 몇 초 후에 사라질지 설정

    public int blinkCount;          // 몇 번 깜빡일지 설정

    public float blinkInterval;     // 깜빡이는 간격 설정

    public float reAppearDelay;     // 다시 재활성화 되는 시간 설정



    private Renderer platformRenderer;      // 렌더러 설정 변수
    Collider platformCollider;              // 플랫폼 설정 변수
    //Transform platformTransform;            // 부모 오브젝트에서 실행을 위한 변수 선언



    void Awake()
    {
        platformRenderer = GetComponent<Renderer>();
        platformCollider = GetComponent<Collider>();
        //platformTransform = transform;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플랫폼 밟힘! " + disappearDelay + "초 후 사라짐.");

            StartCoroutine(BlinkAndDisappear());
        }
    }

    IEnumerator BlinkAndDisappear()
    {
        // 깜빡이기 전 대기시간 조정하는 코드 
        float blinkStartTime = disappearDelay - (blinkCount * blinkInterval);

        // blinkStartTime 시간동안 기다렸다 생성
        yield return new WaitForSeconds(blinkStartTime);

        
        for (int i = 0; i < blinkCount; i++)
        {
            platformRenderer.enabled = !platformRenderer.enabled; // 깜빡이기

            yield return new WaitForSeconds(blinkInterval);
        }

        // 오브젝트의 비활성화
        platformRenderer.enabled = false;
        platformCollider.enabled = false;
        gameObject.SetActive(false);

        Debug.Log("플랫폼 비활성화 " + reAppearDelay + "초 후 재활성화 ");

        GameManager.Instance.StartCoroutine(Reappear());
        
    }
    
    IEnumerator Reappear()
    {
        yield return new WaitForSeconds(reAppearDelay); // 일정 시간 대기

        // 오브젝트의 활성화
        platformRenderer.enabled = true;
        platformCollider.enabled = true;
        gameObject.SetActive(true);

        Debug.Log("플랫폼 활성화!");
    }
}

