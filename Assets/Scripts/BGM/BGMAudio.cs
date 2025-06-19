using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMAudio : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;           // 반복 재생
        audioSource.playOnAwake = false;   // 자동 재생 방지 (수동 재생)

        audioSource.Play();                // 씬 시작 시 재생
    }
}
