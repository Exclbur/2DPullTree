using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Tip : MonoBehaviour
{

    public Image[] text;

    void Start()
    {
        StartCoroutine(FillImages());
        
    }

    void Update()
    {
        
    }

    private IEnumerator FillImages()
    {
        foreach (var image in text)
        {
            // ÿ�ο�ʼ���֮ǰ����fillAmount����Ϊ0
            image.fillAmount = 0f;

            // Э�̿���ÿ��ͼƬ��������
            float elapsedTime = 0f;
            while (elapsedTime < 5)
            {
                // ����ʱ����� fillAmount����0��1��
                image.fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / 5);
                elapsedTime += Time.deltaTime;
                yield return null; // �ȴ�һ֡
            }

            // ȷ��������
            image.fillAmount = 1f;
            image.gameObject.SetActive(false);
            // ��������Լ���һЩ��ʱ��ȷ����һ��ͼƬ��ȫ��ʾ����һ�Ųſ�ʼ
            yield return new WaitForSeconds(0.5f); // ����ʱ������������ͼƬ
        }
    }
}
