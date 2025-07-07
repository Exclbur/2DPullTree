using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_Choose : MonoBehaviour
{

    public Image[] image;
    public int chooseNum;
    // Start is called before the first frame update
    void Start()
    {
        chooseNum = 0;
        foreach (var item in image)
        {
            item.color = Color.gray;
        }
        image[0].color = Color.red;
    }

    public void Change()
    {
        foreach (var item in image)
        {
            item.color = Color.gray;
        }

        image[chooseNum].color = Color.red;
    }
}
