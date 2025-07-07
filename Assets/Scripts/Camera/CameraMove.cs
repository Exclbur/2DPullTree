using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform root;
    private void Start()
    {
        Invoke("Find", .5f);
    }

     private void Find()
    {
        if (transform.name == "÷ìÓñÊ÷¸ùÊÓ½Ç")
            root = GameObject.Find("Player2(Clone)/root").transform;
        else
        {
            root = GameObject.Find("Player1(Clone)/root").transform;
        }
    }
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if(transform.position.y <= -7.5)
        {
            transform.position = new Vector3(transform.position.x, root.position.y - .1f,transform.position.z);
        }
    }
}
