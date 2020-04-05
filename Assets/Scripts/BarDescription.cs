using UnityEngine;

[CreateAssetMenu(fileName = "BarDescription", menuName = "GJ-Stay-At-Home-2020/BarDescription", order = 0)]
public class BarDescription : ScriptableObject
{
    public barInfo[] info = new barInfo[3];
}

[System.Serializable]
public class barInfo
{
    public string title;
    [TextArea] public string content;
}