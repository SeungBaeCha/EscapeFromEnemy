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
            // 현재 씬의 인덱스를 가져와 다음 씬으로 이동
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;       // bulidIndex는 해당 씬 빌드번호 가져오기
            SceneManager.LoadScene(currentSceneIndex + 1);


            Debug.Log("포탈 활성화");
        }

    }


}
