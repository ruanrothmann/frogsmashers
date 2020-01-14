using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroVictoryAnim : MonoBehaviour
{
    public SpriteRenderer rend;
    public Sprite[] frames;

    public bool stopAnimating;
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }
    float frameCounter;
    // Update is called once per frame
    void Update()
    {
        frameCounter += Time.deltaTime;

        if (frameCounter % 2f < 0.9f)
        {
            rend.sprite = frames[3];
            if (stopAnimating)
                frameCounter -= Time.deltaTime;
        }
        else if (frameCounter % 2f < 1f)
        {
            rend.sprite = frames[2];
        }
        else if (frameCounter % 2f < 1.9f)
        {
            rend.sprite = frames[1];
        }
        else
        {
            rend.sprite = frames[2];
        }
    }
}
