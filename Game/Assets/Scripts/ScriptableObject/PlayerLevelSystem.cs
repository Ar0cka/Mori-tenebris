using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "PlayerLevelData", menuName = "Create/LevelData", order = 0)]
public class PlayerLevelSystem : ScriptableObject
{
    [SerializeField] private LevelData levelData;
    public LevelData LevelData => levelData;
}

[Serializable]
public class LevelData
{
    public int level;
    public int currentExperience;
    public int experienceForNextLevel;
    public int maxLevel;
}