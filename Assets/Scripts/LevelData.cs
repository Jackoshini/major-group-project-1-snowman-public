using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public Vector3 PlayerLocation;
    public int PlayerHealth;
    public float PlayerDamage;
    public int PlayerLevel;
    public float PlayerXpToNextLevel;
    public float PlayerCurrentXp;
    public float CurrentTime;

    public float SkeletonMaxHp;
    public float NecromancerMaxHp;
    public int SkeletonGroupSize;
    public int NecromancerGroupSize;

    public List<Vector3> SkeletonLocations = new();
    public List<Vector3> NecromancerLocations = new();

    public static LevelData Load(string json)
    {
        return JsonUtility.FromJson<LevelData>(json);
    }
}
