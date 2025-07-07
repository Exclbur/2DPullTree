using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score instance;
    public int[] score;
    public int[] scoreTotal = new int[2];

    public bool win;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (score[0] >= scoreTotal[0])
        {
            win = true;
        }
    }
}
