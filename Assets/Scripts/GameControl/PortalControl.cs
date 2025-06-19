using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalControl : MonoBehaviour
{
    public GameObject portal;

    bool portalActivate = false;

    void Start()
    {
        portal.SetActive(false);        
    }

    public void ActivatePortal()
    {
        portal.SetActive(true);
        portalActivate = true;
    }


    void OnTriggerEnter(Collider other)
    {
        if(portalActivate && other.CompareTag("Player"))
        {
            // ���� ���� �ε����� ������ ���� ������ �̵�
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;       // bulidIndex�� �ش� �� �����ȣ ��������
            SceneManager.LoadScene(currentSceneIndex + 1);


            Debug.Log("��Ż Ȱ��ȭ");
        }

    }


}
