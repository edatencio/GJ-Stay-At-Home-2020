using UnityEngine;

public static class ExtensionMethods
{
    /*************************************************************************************************
    *** Vector3
    *************************************************************************************************/
    public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
    }
}
