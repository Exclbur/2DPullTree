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
            // 每次开始填充之前，将fillAmount重置为0
            image.fillAmount = 0f;

            // 协程控制每张图片的填充过程
            float elapsedTime = 0f;
            while (elapsedTime < 5)
            {
                // 根据时间更新 fillAmount（从0到1）
                image.fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / 5);
                elapsedTime += Time.deltaTime;
                yield return null; // 等待一帧
            }

            // 确保填充完毕
            image.fillAmount = 1f;
            image.gameObject.SetActive(false);
            // 这里你可以加入一些延时，确保上一张图片完全显示后，下一张才开始
            yield return new WaitForSeconds(0.5f); // 无延时，紧接着下张图片
        }
    }
}
