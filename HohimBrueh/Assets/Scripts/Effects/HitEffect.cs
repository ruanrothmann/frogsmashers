using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{

    public float life;

    public float growTime;

    public bool flipColors;

    public float rotationDelay;
    float rotationCounter;

    float counter;

    SpriteRenderer spriteRenderer;

    public Sprite[] deathFrames;
    public float deathFrameRate;
    int deathFrame = -1;
    float deathFrameCounter;

    public Color[] colors;

    public float scale;

    public void SetColors(Color[] colors)
    {
        this.colors = colors;
    }

    float colorCounter;
    int colorIndex;
    public float scaleCounter;
    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rotationCounter = Random.Range(0f, rotationDelay);
        
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.deltaTime;// / Time.timeScale;


        counter += t;
        colorCounter += t;
        rotationCounter += t;
        if (counter < life)
        {
            if (flipColors)
            {
                if (colorCounter >= 0.04f)
                {
                    colorIndex++;
                    colorCounter -= 0.04f;
                    spriteRenderer.color = colors[colorIndex % colors.Length];
                }
            }

            if (counter < growTime)
            {
                transform.localScale = (counter / growTime) * Vector3.one * scale *(1f + Mathf.Sin(Time.time * 40f + scaleCounter) * 0.15f);
            }
            else
            {
                transform.localScale = Vector3.one * scale* (1f + Mathf.Sin(Time.time * 40f + scaleCounter) * 0.15f);

                
            }

            if (rotationCounter >= rotationDelay)
            {
                rotationCounter -= rotationDelay;
                transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            }
        }
        
        if (counter >= life)
        {
            //transform.rotation = Quaternion.identity;
            deathFrameCounter -= t;
            if (deathFrameCounter < 0f)
            {
                deathFrameCounter = deathFrameRate;
                deathFrame++;
                if (deathFrame >= deathFrames.Length)
                    Destroy(gameObject);
                else
                    spriteRenderer.sprite = deathFrames[deathFrame];
            }


        }

    }
}
