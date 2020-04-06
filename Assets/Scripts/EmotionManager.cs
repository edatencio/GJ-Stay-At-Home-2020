using UnityEngine;
using System.Collections.Generic;

public class EmotionManager : MonoBehaviour
{
    public static EmotionManager instance;
    public List<ClientEmotion> clientEmotions;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public ClientEmotion GetEmotion(EmotionType emotionType)
    {
        foreach (var clientEmotion in clientEmotions)
        {
            if(clientEmotion.emotionType == emotionType)
            return clientEmotion;
        }
        return null;
    }
}