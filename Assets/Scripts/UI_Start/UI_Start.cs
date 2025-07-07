using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UI_Start : MonoBehaviour
{
    public GameObject[] startBack;


    private AudioSource audioSource;
    public AudioClip[] clip;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip[0];
        audioSource.Play();
        MoveLu();
        Invoke("MoveLin", 1);
        Invoke("MoveVS", 2);
        Invoke("MoveZi", 4);
    }


    private void MoveLu()
    {
        startBack[0].transform.DOMove(new Vector2(960,540),2);
    }

    private void MoveLin()
    {
        startBack[1].transform.DOMove(new Vector2(960,540), 2);
    }

    private void MoveVS()
    {
        startBack[2].transform.DOMove(new Vector2(960, 540), 2);
        startBack[3].transform.DOMove(new Vector2(960, 540), 2);
    }

    private void MoveZi()
    {
        startBack[4].transform.DOMove(new Vector2(960, 540), 2);
    }

    public void ButtonBBB()
    {
        startBack[5].transform.DORotate(new Vector3(0, 0,-180), 0.5f);
        startBack[5].transform.DOScale(0, 0.5f);
        Invoke("OpenNext", 0.5f);
    }

    private void OpenNext()
    {
        startBack[6].SetActive(true);
    }
   
}
