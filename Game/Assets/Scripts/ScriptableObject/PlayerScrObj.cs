using System;
using DefaultNamespace;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerStats")]
public class PlayerScrObj : ScriptableObject
{
    [SerializeField] private PlayerData _playerData;
    public PlayerData PlayerData => _playerData;
}