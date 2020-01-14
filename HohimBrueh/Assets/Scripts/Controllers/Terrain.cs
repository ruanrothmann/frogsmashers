using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{

    static Terrain instance;

    public Transform leftKillPoint;
    public Transform rightKillPoint;
    public Transform botKillPoint;
    public Transform topKillPoint;
    public Transform screenTop;

    public Transform flySpawn;

    public Transform[] spawnPoints;

    void Awake()
    {
        instance = this;
    }

    public static Vector3 GetFlySpawnPoint()
    {
        if (instance.flySpawn != null)
            return instance.flySpawn.position;
        return Vector3.zero;
    }

    public static Vector3 GetSpawnPoint(int index)
    {
        return instance.spawnPoints[index].position;
    }

    public static Vector3 GetSpawnPoint()
    {
        float[] closestCharacterDistance = new float[instance.spawnPoints.Length];
        int i = 0;
        foreach (var spawn in instance.spawnPoints)
        {
            closestCharacterDistance[i] = float.MaxValue;
            foreach (var player in GameController.activePlayers)
            {
                if (player.character != null)
                {
                    var dist = Vector2.Distance(player.character.transform.position, spawn.position);
                    if (dist < closestCharacterDistance[i])
                    {
                        closestCharacterDistance[i] = dist;
                    }
                }
            }
            i++;
        }

        int furthestIndex = 0;
        float furthestDistance = float.MinValue;
        i = 0;
        foreach (var spawn in instance.spawnPoints)
        {
            if (closestCharacterDistance[i] == furthestDistance && Random.value < 0.5f)
            {
                furthestIndex = i;
                furthestDistance = closestCharacterDistance[i];
            }
            else
            if (closestCharacterDistance[i] > furthestDistance)
            {
                furthestIndex = i;
                furthestDistance = closestCharacterDistance[i];
            }
            i++;
        }

        return instance.spawnPoints[furthestIndex].position;
    }

    public static float LeftKillPoint
    {
        get
        {
            return instance.leftKillPoint.position.x;

        }
    }

    public static float RightKillPoint
    {
        get
        {
            return instance.rightKillPoint.position.x;

        }
    }

    public static float TopKillPoint
    {
        get
        {
            return instance.topKillPoint.position.y;

        }
    }

    public static float BotKillPoint
    {
        get
        {
            return instance.botKillPoint.position.y;

        }
    }

    public static float ScreenTop
    {
        get
        {
            return instance.screenTop.position.y;
        }
    }

}
