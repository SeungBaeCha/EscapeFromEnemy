using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMAudio : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;           // �ݺ� ���
        audioSource.playOnAwake = false;   // �ڵ� ��� ���� (���� ���)

        audioSource.Play();                // �� ���� �� ���
    }
}
