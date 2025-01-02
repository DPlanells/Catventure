using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int lives;
    public int coins;
    public bool canRun;
    public bool canJump;
    public bool canAttack;
    public Vector3 checkpointPosition;
}
