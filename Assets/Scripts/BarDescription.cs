using UnityEngine;

[CreateAssetMenu(fileName = "BarDescription", menuName = "GJ-Stay-At-Home-2020/BarDescription", order = 0)]
public class BarDescription : ScriptableObject 
{
    public string title;
    [TextArea]public string description;
}

