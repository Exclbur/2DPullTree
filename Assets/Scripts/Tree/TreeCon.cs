using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCon : MonoBehaviour
{
    private Animator anim;
    private Score score;
    private float growState;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        score = GameObject.Find("Score"). GetComponent<Score>();
        
    }

    // Update is called once per frame
    void Update()
    {
        growState = (float)score.score[0] / (float)score.scoreTotal[0];
        
        if (growState > .25f)
        {
            anim.SetBool("grow1", true);
        }
        if(growState > .5f)
        {
            anim.SetBool("grow2", true);
        }
        if(growState > .75f)
        {
            anim.SetBool("grow3", true);
        }
        if(growState >= 1)
        {
            Debug.Log("True");

        }
    }
}
