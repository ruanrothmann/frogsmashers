using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailFader : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public float life;

    public float counter;

    public float lifeM = 1f;

    public SpriteRenderer copySpriteRenderer;
    public bool grow;

    float scaleM;
    float scaleXSign;
    void Start()
    {
        scaleM = lifeM;
    }

    void LateUpdate()
    {
        if (copySpriteRenderer != null)
            spriteRenderer.sprite = copySpriteRenderer.sprite;
    }

    void Update()
    {

        
        //if (TimeController.TimeBumpActive)
        //{
        //    counter += Time.deltaTime / lifeM;
        //    transform.localScale = Vector3.one *  (1f +  0.25f * counter / (life / lifeM)); 
        //}
        //else
        {
            counter += Time.deltaTime;
            var c = spriteRenderer.color;
            c.a = 1f - (counter / life);
            spriteRenderer.color = c;
            float scale = scaleM * (1f - counter / (life * lifeM));
            if (grow)
            {
                scale = scaleM * (1f + counter / (life * lifeM));
            }
            transform.localScale = Vector3.one * scale;
            if (scale < 1f && !grow)
            {
                Destroy(gameObject);
                //transform.parent = null;
                //copySpriteRenderer = null;
            }
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (grow ? Time.deltaTime * 0.1f : -Time.deltaTime * 0.1f));

        //if (counter > (life * lifeM) * 0.5f)
        //    transform.parent = null;
        if (counter > (life * lifeM))
            Destroy(gameObject);
    }
    
}
