using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnim : MonoBehaviour
{

    public float animSpeed;
    public Sprite[] frames;

    public bool playOnce;

    int frame = -1;
    float counter;


    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > animSpeed)
        {
            counter -= animSpeed;
            frame++;

            GetComponent<SpriteRenderer>().sprite = frames[frame % frames.Length];
            if (playOnce && frame >= frames.Length)
                Destroy(gameObject);
        }
    }
}
