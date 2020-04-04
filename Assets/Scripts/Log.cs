using UnityEngine;

public static class Log
{
    private enum Type { Message, Error, Warning }

    /*************************************************************************************************
    *** Write
    *************************************************************************************************/
    private static void Write(Type type, string message)
    {
        if (!Debug.isDebugBuild)
            return;

        switch (type)
        {
            case Type.Message:
                Debug.Log(message);
                break;

            case Type.Error:
                Debug.LogError(message);
                break;

            case Type.Warning:
                Debug.LogWarning(message);
                break;
        }
    }

    /*************************************************************************************************
    *** Methods
    *************************************************************************************************/

    // Message
    public static void Message(string message)
    {
        Write(Type.Message, message);
    }

    public static void Message(string name, string message)
    {
        Write(Type.Message, string.Concat(name, ": ", message));
    }

    public static void Message(string name, params string[] message)
    {
        string str = null;

        for (int i = 0; i < message.Length; i++)
            str += message[i];

        Write(Type.Message, string.Concat(name, ": ", str));
    }

    public static void Message(string name, params object[] message)
    {
        string str = null;

        for (int i = 0; i < message.Length; i++)
            str += message[i].ToString();

        Write(Type.Message, string.Concat(name, ": ", str));
    }

    // Error
    public static void Error(string message)
    {
        Write(Type.Error, message);
    }

    public static void Error(string name, string message)
    {
        Write(Type.Error, string.Concat(name, ": ", message));
    }

    public static void Error(string name, params string[] message)
    {
        string str = null;

        for (int i = 0; i < message.Length; i++)
            str += message[i];

        Write(Type.Error, string.Concat(name, ": ", str));
    }

    public static void Error(string name, params object[] message)
    {
        string str = null;

        for (int i = 0; i < message.Length; i++)
            str += message[i].ToString();

        Write(Type.Error, string.Concat(name, ": ", str));
    }

    // Warning
    public static void Warning(string message)
    {
        Write(Type.Warning, message);
    }

    public static void Warning(string name, string message)
    {
        Write(Type.Warning, string.Concat(name, ": ", message));
    }

    public static void Warning(string name, params string[] message)
    {
        string str = null;

        for (int i = 0; i < message.Length; i++)
            str += message[i];

        Write(Type.Warning, string.Concat(name, ": ", str));
    }

    public static void Warning(string name, params object[] message)
    {
        string str = null;

        for (int i = 0; i < message.Length; i++)
            str += message[i].ToString();

        Write(Type.Warning, string.Concat(name, ": ", str));
    }

    // Flag
    public static void Flag()
    {
        Write(Type.Message, "FLAG");
    }

    public static void Flag(string name)
    {
        Write(Type.Message, string.Concat(name, ": FLAG"));
    }

    public static void Flag(int number)
    {
        Write(Type.Message, string.Concat("FLAG ", number));
    }

    public static void Flag(string name, int number)
    {
        Write(Type.Message, string.Concat(name, ": FLAG ", number));
    }
}
