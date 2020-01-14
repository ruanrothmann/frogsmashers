using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BackgroundParralaxPieceInfo
{
    public SpriteRenderer[] prefabs;
    public Vector2 speedMin;
    public Vector2 speedMax;
    public float depth;
    public float probability;

    public Vector2 spawnRangeY;
    public Vector2 spawnRangeX;
    public bool dontScale;
    public bool parentToCamera;

    public bool dropFrog;
}

public class BackgroundParallaxSpawner : MonoBehaviour
{
    public List<BackgroundParralaxPieceInfo> pieces;

    public Confetti[] frogParticles;

    float spawnDelay;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        spawnDelay -= Time.deltaTime;

        if (spawnDelay < 0f)
        {


            var info = GetParallaxInfo();
            if (info != null)
            {

                spawnDelay = Random.Range(0f, 0.25f);
                StartCoroutine(RunParallaxPiece(info));

            }
        }

    }


    IEnumerator RunParallaxPiece(BackgroundParralaxPieceInfo inf)
    {
        float lerpVal = Random.value;
        Vector3 spawnPos = new Vector3(Random.Range(inf.spawnRangeX.x, inf.spawnRangeX.y), Random.Range(inf.spawnRangeY.x, inf.spawnRangeY.y), inf.depth - lerpVal);
        var p = Instantiate(inf.prefabs[Random.Range(0, inf.prefabs.Length)], spawnPos, Quaternion.identity);
        if (inf.parentToCamera)
            p.transform.parent = Camera.main.transform;
        float life = 10f;

        if (!inf.dontScale)
            p.transform.localScale = Vector3.one * Mathf.Lerp(0.5f, 1.1f, lerpVal);
        bool haveDroppedFrog = false;
        Vector2 speed = Vector2.Lerp(inf.speedMin, inf.speedMax, lerpVal);
        while (life > 0f || p.isVisible)
        {
            life -= Time.deltaTime;

            yield return null;
            p.transform.Translate(speed * Time.deltaTime);

            


        }

        if (!haveDroppedFrog)
        {
            haveDroppedFrog = true;
            if (inf.dropFrog && Random.value < 0.5f)
            {
                var s = Instantiate(frogParticles[Random.Range(0, frogParticles.Length)], p.transform.position, Quaternion.identity);
                s.GetComponent<SpriteRenderer>().color = new Color(170f / 255f, 176f / 255f, 88f / 255f);
                s.GetComponent<SpriteRenderer>().material.renderQueue = 3000;
                s.GetComponent<SimpleAnim>().animSpeed = 0.1f;
                s.velocity = new Vector3(Random.Range(-5f, 5f), 0f, 0f);
            }
        }

        Destroy(p.gameObject);

    }


    BackgroundParralaxPieceInfo GetParallaxInfo()
    {
        foreach (var piece in pieces)
        {
            if (Random.value < piece.probability)
            {
                return piece;
            }
        }

        return null;
    }
}
