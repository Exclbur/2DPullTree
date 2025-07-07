using UnityEngine;
using UnityEngine.UI;

public class ColorSet : MonoBehaviour
{

    public RawImage colorBlock;
    private Texture2D colorBlock2D;

    public Image[] image;

    public Color setColor= Color.black;
    // Start is called before the first frame update
    void Start()
    {
        colorBlock2D = (Texture2D)colorBlock.mainTexture;
    }

    // Update is called once per frame
    void Update()
    {
        image[0].color = GetImagePixel(Input.mousePosition);
        if (Input.GetMouseButtonDown(1))
        {
            image[1].color = GetImagePixel(Input.mousePosition);
            setColor = image[1].color;
        }
    }

    public Color GetImagePixel(Vector3 mousePos)
    {
        
        Vector3 point = colorBlock.transform.InverseTransformPoint(mousePos);

        int x = (int)point.x + colorBlock2D.width / 2;
        int y = (int)point.y + colorBlock2D.height / 2;

        
        return colorBlock2D.GetPixel(x, y);
    }
}
