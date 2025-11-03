using UnityEngine;

public static class Utility
{
    public static Vector3 ZeroOutY(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }
}
