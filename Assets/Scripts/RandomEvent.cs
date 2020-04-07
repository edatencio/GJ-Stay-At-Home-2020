using UnityEngine;

[CreateAssetMenu(fileName = "RandomEvent", menuName = "GJ-Stay-At-Home-2020/RandomEvent", order = 0)]
public class RandomEvent : ScriptableObject 
{
 public string title;
 [TextArea]public string content;   
}