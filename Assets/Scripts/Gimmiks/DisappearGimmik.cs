using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearGimmik : MonoBehaviour
{
    
    public float disappearDelay;    // �� �� �Ŀ� ������� ����

    public int blinkCount;          // �� �� �������� ����

    public float blinkInterval;     // �����̴� ���� ����

    public float reAppearDelay;     // �ٽ� ��Ȱ��ȭ �Ǵ� �ð� ����



    private Renderer platformRenderer;      // ������ ���� ����
    Collider platformCollider;              // �÷��� ���� ����
    //Transform platformTransform;            // �θ� ������Ʈ���� ������ ���� ���� ����



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
            Debug.Log("�÷��� ����! " + disappearDelay + "�� �� �����.");

            StartCoroutine(BlinkAndDisappear());
        }
    }

    IEnumerator BlinkAndDisappear()
    {
        // �����̱� �� ���ð� �����ϴ� �ڵ� 
        float blinkStartTime = disappearDelay - (blinkCount * blinkInterval);

        // blinkStartTime �ð����� ��ٷȴ� ����
        yield return new WaitForSeconds(blinkStartTime);

        
        for (int i = 0; i < blinkCount; i++)
        {
            platformRenderer.enabled = !platformRenderer.enabled; // �����̱�

            yield return new WaitForSeconds(blinkInterval);
        }

        // ������Ʈ�� ��Ȱ��ȭ
        platformRenderer.enabled = false;
        platformCollider.enabled = false;
        gameObject.SetActive(false);

        Debug.Log("�÷��� ��Ȱ��ȭ " + reAppearDelay + "�� �� ��Ȱ��ȭ ");

        GameManager.Instance.StartCoroutine(Reappear());
        
    }
    
    IEnumerator Reappear()
    {
        yield return new WaitForSeconds(reAppearDelay); // ���� �ð� ���

        // ������Ʈ�� Ȱ��ȭ
        platformRenderer.enabled = true;
        platformCollider.enabled = true;
        gameObject.SetActive(true);

        Debug.Log("�÷��� Ȱ��ȭ!");
    }
}

