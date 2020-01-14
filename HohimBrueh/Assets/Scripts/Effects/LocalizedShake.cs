using UnityEngine;
using System.Collections;

public class LocalizedShake : MonoBehaviour
{
    public Material material;

    public Vector2 Magnitude;

    public float life;

    float lifeLeft;

    public float velocity;

    Vector2 offset;

    float counter;

    //public float springStrength;
	// Use this for initialization
	void Start ()
    {
        lifeLeft = life;
        material = GetComponent<SpriteRenderer>().material;
	}
	
	// Update is called once per frame
	void Update ()
    {
        counter += Time.deltaTime;
        lifeLeft -= Time.deltaTime;

        float shakeM = Mathf.PingPong((counter / life) * 2f, 1f);

        if (lifeLeft <= 0f)
            Destroy(gameObject);
        float lifeM = lifeLeft / life;

        //offset = new Vector2(-Mathf.Sin(counter * velocity) * Magnitude.x , Mathf.Sin(counter * velocity) * Magnitude.y) * lifeM ;

        offset = new Vector2(- shakeM * Magnitude.x, shakeM * Magnitude.y);

        material.SetFloat("_xOffset", offset.x);
        material.SetFloat("_yOffset", offset.y);

	}
}
