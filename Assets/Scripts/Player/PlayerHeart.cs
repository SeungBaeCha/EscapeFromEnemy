using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class PlayerHeart : MonoBehaviour
{
    [Header("HP Setting")]
    public int maxHP = 5;
    int currentHP;

    [Header("UI Setting")]
    public GameObject heartPrefab;
    public Transform hpContainer;

    [Header("Screen Effect")]
    public Image redOverlayImage;



    [Header("Death Setting")]
    public GameObject deathPanel;

    [Header("Sound Setting")] 
    public AudioClip deathSound;           // 죽음 효과음
    public float deathVolume;       // 효과음 볼륨
    public AudioClip hitSound;
    public float hitVolume;


    AudioSource audioSource;       // 사운드 재생기

    List<GameObject> heartList = new List<GameObject>();

    void Start()
    {
        currentHP = maxHP;

        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    void InitHearts()
    {
        foreach (Transform child in hpContainer)
        {
            Destroy(child.gameObject);
        }
        heartList.Clear();

        for (int i = 0; i < maxHP; i++)
        {
            GameObject heart = Instantiate(heartPrefab, hpContainer);
            heartList.Add(heart);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        Debug.Log("현재 체력: " + currentHP);
        UpdateHearts();


        UpdateRedOverlay();



        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound, hitVolume);
        }


        if (currentHP <= 0)
        {
            HandleDeath();
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < heartList.Count; i++)
        {
            heartList[i].SetActive(i < currentHP);
        }
    }

    public void InitHpUI()
    {
        InitHearts();
    }

    void HandleDeath()
    {
        Debug.Log("플레이어 사망!");

        // 죽음 처리 코루틴 실행
        StartCoroutine(PauseGame());
    }

    IEnumerator PauseGame()
    {
        if (deathSound != null && audioSource != null)
        {
            // 그 다음 게임 정지 및 사망 UI 처리
            Time.timeScale = 0f;

            // 커서 재활성화
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // 효과음 재생
            audioSource.PlayOneShot(deathSound, deathVolume);

            // 패널 활성화
            deathPanel.SetActive(true); 

            // 사운드 길이만큼 기다림
            yield return new WaitForSecondsRealtime(deathSound.length);
        }

        // 모든 음향효과 중단
        AudioListener.pause = true;
    }


    void UpdateRedOverlay()
    {
        if (redOverlayImage != null)
        {
            float intensity = 1f - (float)currentHP / maxHP; // 체력이 낮을수록 값이 커짐
            Color color = redOverlayImage.color;
            color.a = Mathf.Lerp(0f, 0.6f, intensity); // 최대 알파는 0.6
            redOverlayImage.color = color;
        }
    }






    public int CurrentHP => currentHP;
}
