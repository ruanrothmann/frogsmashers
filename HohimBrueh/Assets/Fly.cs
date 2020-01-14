using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    Vector2 velocity, targetVelocity;
    int terrainLayer;
    public SpriteRenderer sprite;

    public float maxSpeed;
    internal bool BeingIngested;

    float updateDirectionDelay;

    // Use this for initialization
    void Start()
    {
        terrainLayer = 1 << LayerMask.NameToLayer("Ground");
        velocity = Random.insideUnitCircle.normalized * 10f;
    }

    // Update is called once per frame
    void Update()
    {

        updateDirectionDelay -= Time.deltaTime;
        if (updateDirectionDelay < 0f)
        {
            updateDirectionDelay = Random.Range(3f, 10f);
            UpdateDirection();
        }

        if (!BeingIngested)
            RunMotion();

        if (transform.position.x < Terrain.LeftKillPoint || transform.position.x > Terrain.RightKillPoint || transform.position.y > Terrain.TopKillPoint || transform.position.y < Terrain.BotKillPoint)
            Destroy(gameObject);

    }

    void RunMotion()
    {
        velocity = Vector2.MoveTowards(velocity, targetVelocity, 5f * Time.deltaTime);

        Vector2 velocityT = velocity * Time.deltaTime + Vector2.up * Mathf.Sin(Time.time * 8f) * 3f * Time.deltaTime;
        if (velocityT.x < 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.left, Mathf.Abs(velocityT.x) + 1f, terrainLayer))
            {
                velocityT.x = 0;
                velocity.x *= -0.5f;
                UpdateDirection();
            }
        }
        if (velocityT.x > 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.right, Mathf.Abs(velocityT.x) + 1f, terrainLayer))
            {
                velocityT.x = 0;
                velocity.x *= -0.5f;
                UpdateDirection();
            }
        }
        if (velocityT.y < 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.down, Mathf.Abs(velocityT.y) + 1f, terrainLayer))
            {
                velocityT.y = 0;
                velocity.y *= -0.5f;
                UpdateDirection();
            }
        }
        if (velocityT.y > 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.up, Mathf.Abs(velocityT.y) + 1f, terrainLayer))
            {
                velocityT.y = 0;
                velocity.y *= -0.5f;
                UpdateDirection();
            }
        }

        if (velocityT.x < 0)
            sprite.transform.localScale = new Vector3(1f, 1f, 1f);
        else
            sprite.transform.localScale = new Vector3(-1f, 1f, 1f);

        transform.position += (Vector3)velocityT;
    }

    private void UpdateDirection()
    {
        if (Random.value < 0.1f)
        {
            targetVelocity = Vector2.zero;
            updateDirectionDelay = Random.Range(1f, 3f);
        }
        else
            targetVelocity = Random.insideUnitCircle.normalized * maxSpeed;
    }
}
