using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLightFlicker : MonoBehaviour
{

    public Vector2 delay;

    float explosionDelayLeft;

    float flickerDelayLeft;
    bool flickerOn;


    public ParticleSystem dustParticlePrefab;

    SpriteRenderer sr;

    float intensity;

    // Use this for initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        explosionDelayLeft -= Time.deltaTime;

        if (explosionDelayLeft < 0f)
        {
            explosionDelayLeft = Random.Range(delay.x, delay.y);
            intensity = Random.Range(0.3f,1.3f);
            var c = sr.color;
            c.a = 1f + intensity;
            sr.color = c;
            
            {
                EffectsController.ShakeCamera(Random.insideUnitCircle, Random.value * 0.2f + intensity);
            }
            FreeLives.SoundController.PlaySoundEffect("BackgroundExplosion", 0.5f);

            float dustx = Random.Range(-21f, -9f);
            if (Random.value < 0.5f)
            {
                dustx = -dustx;
            }
            if (intensity > 1f)
            {
                var dp = Instantiate(dustParticlePrefab, new Vector3(dustx, 10.1f, 0f), dustParticlePrefab.transform.rotation);

                Destroy(dp.gameObject, 15f);
            }
        }
        else
        {
            flickerDelayLeft -= Time.deltaTime;
            if (flickerDelayLeft < 0f)
            {
                flickerOn = !flickerOn;
                flickerDelayLeft = Random.Range(0.01f, 0.03f);
            }


            intensity = Mathf.MoveTowards(intensity, 0.2f, Time.deltaTime);


            var c = sr.color;

            if (flickerOn && intensity > 0.5f)
                c.a = intensity + 0.2f;
            else
            {
                if (intensity > 0.2f)
                    c.a = Random.Range(0.2f, 0.5f + intensity);
                else
                    c.a = 0.2f;
            }

            sr.color = c;
        }
    }
}
