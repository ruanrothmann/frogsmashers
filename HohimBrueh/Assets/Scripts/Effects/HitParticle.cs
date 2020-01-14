using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticle : MonoBehaviour
{

    public float life;
    
    public float colorChangeDelay;

    public float startSize, endSize;

    float counter, colorCounter;

    public Vector2 velocity;

    public bool alphaOut;

    SpriteRenderer spriteRenderer;

    public Color[] colors;

    public void SetColors(Color[] colors)
    {
        this.colors = colors;
    }

    
    int colorIndex;
    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorCounter = Random.Range(0, colorChangeDelay);

    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.deltaTime;// / Time.timeScale;


        counter += t;
        colorCounter += t;

        var lifeM = counter / life;

        transform.localScale = Vector3.one * Mathf.Lerp(startSize, endSize, lifeM);

        if (colorCounter >= colorChangeDelay)
        {
            colorCounter -= colorChangeDelay;
            //spriteRenderer.color = EffectsController.GetRandomColor();
            
                colorIndex++;
                spriteRenderer.color = colors[colorIndex % colors.Length];
            


        }

        if (alphaOut)
        {
            if (counter > life * 0.75f)
            {
                float halfLifeM = (counter - life * 0.75f) / life * 0.25f;
                var c = spriteRenderer.color;
                c.a = 1f - halfLifeM;
                spriteRenderer.color = c;
            }
        }
        transform.position += (Vector3)velocity * t;
        
        if (counter > life)
            Destroy(gameObject);

    }
}
