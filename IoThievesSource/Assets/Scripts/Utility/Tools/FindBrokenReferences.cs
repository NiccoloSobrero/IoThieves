#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class FindBrokenReferences
{
    [MenuItem("Tools/Find Missing References In Scene")]
    static void FindMissing()
    {
        var objects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (var go in objects)
        {
            var components = go.GetComponents<Component>();
            foreach (var c in components)
            {
                if (c == null)
                {
                    Debug.LogError("Missing script in: " + go.name, go);
                }
            }
        }

        Debug.Log("Scan complete.");
    }
}
#endif
