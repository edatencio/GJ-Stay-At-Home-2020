using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundStats", menuName = "GJ-Stay-At-Home-2020/RoundStats", order = 0)]
public class RoundStats : ScriptableObject
{
    [Range(60, 240)] public float roundTime = 120f;
    public List<ClientGroup> clientsPrefabs;


}