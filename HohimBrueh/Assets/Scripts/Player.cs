using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Blue,
    Red
}

public class Player
{

    public FreeLives.InputReader.Device inputDevice;

    public Team team;

    public Color color;

    public int score;

    public int roundWins;

    public SpriteRenderer offscreenDot;

    public int sortPriority;

    public Player(FreeLives.InputReader.Device inputDevice, Color color, int sortPriority)
    {
        this.inputDevice = inputDevice;
        this.color = color;
        this.sortPriority = sortPriority;
    }

    public Character character;

    public float spawnDelay = 0f;

}
