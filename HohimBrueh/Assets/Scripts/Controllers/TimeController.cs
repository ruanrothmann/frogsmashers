using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{

    static TimeController instance;

    float timeBumpTimeLeft;

    public float timeBumpTime, timeBumpScale;

    bool timeBumpedThisFrame;

    public static bool TimeBumpActive
    {
        get
        { return instance.timeBumpTimeLeft > 0f; }
    }

    // Use this for initialization
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (Time.timeScale > 0.5f)
            {
                Time.timeScale = 0.1f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }

        if (timeBumpTimeLeft > 0f)
        {
            if (timeBumpedThisFrame)
            {
                timeBumpedThisFrame = false;

            }
            else
            {
                timeBumpTimeLeft -= Time.deltaTime / Time.timeScale;
                if (timeBumpTimeLeft <= 0f)
                    Time.timeScale = 1f;
            }
        }
    }

    public static void TimeBumpCharacters(Vector2 center, float durationM, float radius, bool dropOff)
    {
        foreach (var player in GameController.activePlayers)
        {
            if (player.character != null)
            {
                float dist = Vector2.Distance(center, player.character.transform.position);
                if (dist < radius)
                {
                    float m = dist / radius;
                    player.character.TimeBump(durationM,dropOff ? m * m : 0f);
                }
            }
        }
    }


    public static void Timebump(float intensity)
    {

        instance.timeBumpTimeLeft = instance.timeBumpTime * Mathf.Clamp(intensity, 1f, 5f);
        Time.timeScale = instance.timeBumpScale;
        instance.timeBumpedThisFrame = true;
    }
}
