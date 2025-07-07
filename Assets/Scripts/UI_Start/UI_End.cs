using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UI_End : MonoBehaviour
{

    public GameObject[] endBack;
    // Start is called before the first frame update
    public Image[] text;
    [SerializeField] private int runtime;
    [SerializeField] private int runtime2;
    void Start()
    {
        StartMove();
        Invoke("Fly", 3f);
    }
    private void StartMove()
    {
        endBack[0].transform.DOMove(new Vector2(840, transform.position.y), 3f);
    }


    public void Fly()
    {
        endBack[0].transform.DORotate(new Vector3(0, 0, -180), 3f);
        endBack[0].transform.DOMove(new Vector2(1790, transform.position.y), 3f);
        endBack[0].transform.DOScale(new Vector3(2.7f, 2.7f, 2.7f), 3);
        Invoke("SetTrue", 3f);
    }

    public void SetTrue()
    {
        endBack[0].SetActive(false);
        endBack[1].SetActive(true);
        Invoke("StickMove", 0.5f);
    }

    private void StickMove()
    {
        endBack[2].transform.DOMove(new Vector2(-900, transform.position.y), runtime);
        StartCoroutine(FillImages());
        Invoke("LoadOpen", runtime);
    }

    private IEnumerator FillImages()
    {
        foreach (var image in text)
        {
            // ÿ�ο�ʼ���֮ǰ����fillAmount����Ϊ0
            image.fillAmount = 0f;

            // Э�̿���ÿ��ͼƬ��������
            float elapsedTime = 0f;
            while (elapsedTime < runtime2)
            {
                // ����ʱ����� fillAmount����0��1��
                image.fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / runtime2);
                elapsedTime += Time.deltaTime;
                yield return null; // �ȴ�һ֡
            }

            // ȷ��������
            image.fillAmount = 1f;

            // ��������Լ���һЩ��ʱ��ȷ����һ��ͼƬ��ȫ��ʾ����һ�Ųſ�ʼ
            yield return new WaitForSeconds(0f); // ����ʱ������������ͼƬ
        }
    }

    private void LoadOpen()
    {
        float elapsedTime = 0f;
        while (elapsedTime < runtime2)
        {
            // ����ʱ����� fillAmount����0��1��
            text[17].fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / runtime2);
            elapsedTime += Time.deltaTime;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}

