using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScorePlum : MonoBehaviour
{

    public float animTime;

    public float textFadeTime;

    public Sprite[] loop;
    public Sprite[] deathAnim;

    float lifeCounter;
    float frameCounter;

    public float frameDelay;

    int frame = -1;

    bool looping = true;

    public Transform canvasTransform;

    public UnityEngine.UI.Text text;

    Color color;

    int points;

    public void SetText(string text, Color color)
    {
        this.text.text = text;
        this.text.color = color;
        this.color = color;
    }

    public void SetPoints(int points)
    {
        this.points = points;
        //animTime = animTime + 0.05f * points;

    }

    void Start()
    {
        canvasTransform.rotation = Quaternion.identity;
    }

    void RunText()
    {
        if (lifeCounter <= animTime)
        {
            canvasTransform.localScale = Vector3.Lerp(canvasTransform.localScale, Vector3.one, Time.deltaTime * 10f);
            Vector3 pos = canvasTransform.localPosition;
            pos = Vector3.Lerp(pos, new Vector3(4f, 0f, 0f), Time.deltaTime * 5f);
            canvasTransform.localPosition = pos;
        }
        else
        {
            float timePastAnim = lifeCounter - animTime;
            float m = timePastAnim / textFadeTime;

            if (m < 0.5f)
            {
                canvasTransform.localScale = Vector3.Lerp(canvasTransform.localScale, Vector3.one, Time.deltaTime * 2f);
            }
            else
            {
                canvasTransform.localScale = Vector3.Lerp(canvasTransform.localScale, Vector3.one * 1.5f, Time.deltaTime);
            }
            Vector3 pos = canvasTransform.localPosition;
            pos = Vector3.Lerp(pos, new Vector3(4f, 0f, 0f), Time.deltaTime * 3f);
            canvasTransform.localPosition = pos;
        }

        var col = text.color;
        float a = col.a;

        if (Time.time % 0.3f < 0.1f)
            col = Color.Lerp(col, color, Time.deltaTime * 25f);
        else if (Time.time % 0.3f < 0.2f)
            col = Color.Lerp(col, Color.white, Time.deltaTime * 25f);
        else col = Color.Lerp(col, Color.black, Time.deltaTime * 25f);

        if (lifeCounter > animTime)
        {
            float timePastAnim = lifeCounter - animTime;
            float m = timePastAnim / textFadeTime;
            if (m > 0.5f)
            {
                a = Mathf.Lerp(1f, 0f, (m- 0.5f) * 2f);
            }
            col.a = a;
        }

        text.color = col;

        if (lifeCounter > animTime + textFadeTime)
            Destroy(gameObject);


    }


    // Update is called once per frame
    void Update()
    {

        lifeCounter += Time.deltaTime;

        frameCounter += Time.deltaTime;
        RunText();
        if (lifeCounter > animTime && looping)
        {
            looping = false;
            frame = -1;
            frameCounter = frameDelay;
        }



        if (frameCounter >= frameDelay)
        {
            frameCounter -= frameDelay;
            frame++;
            


            if (looping)
            {
                SprayParticles();
                if (frame >= loop.Length)
                {
                    frame = 0;
                }
                GetComponent<SpriteRenderer>().sprite = loop[frame];
            }
            else
            {
                
                if (frame >= deathAnim.Length)
                    GetComponent<SpriteRenderer>().enabled = false;
                else
                {
                    SprayParticles();
                    GetComponent<SpriteRenderer>().sprite = deathAnim[frame];
                }

            }

        }

    }

    void SprayParticles()
    {
        if (points > 0)
            EffectsController.SprayParticles(transform.position, transform.right, 15f + 5f * points, points, 0.22f, 15f);
    }
}
