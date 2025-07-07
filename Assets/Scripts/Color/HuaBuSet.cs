using System;
using UnityEngine;
using UnityEngine.UI;

public class HuaBuSet : MonoBehaviour
{

    public RawImage huabu;
    public Image penSizeImage;

    private Slider slider;
    private int penSize; 

    private Texture2D huabu2D;
    private int width, height;

    private  ColorSet colorSet;

    private void Awake()
    {
        colorSet = GetComponent<ColorSet>();
        slider = GetComponentInChildren<Slider>();

        Texture2D tex = (Texture2D)huabu.mainTexture;
        huabu2D = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
        height = huabu2D.height;
        width = huabu2D.width;


        huabu2D.Apply();
        huabu.texture = huabu2D;


        penSizeImage.transform.localScale = new Vector2(penSize / 20, penSize / 20);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
            SetImagePixel(Input.mousePosition,colorSet.setColor);

        penSize = (int)(slider.value * 20);
    }

    public void SetImagePixel(Vector3 pos, Color color)
    {

        Vector3 point = huabu.transform.InverseTransformPoint(pos);


        for (int i = (int)pos.x - penSize; i <pos.x + penSize ; i++)
        {
            for (int j = (int)pos.y - penSize; j < pos.y + penSize; j++)
            {
                huabu2D.SetPixel(i + huabu2D.width/4 + 50, j + huabu2D.height/2,color);
            }
        }
       huabu2D.Apply();
    }
}
