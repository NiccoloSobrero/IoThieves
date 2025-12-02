using UnityEngine;
using System.Runtime.InteropServices;

namespace SaveLoadSystem
{
    public static class FirebaseWeb
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void FB_SetTarget(string goName);
        [DllImport("__Internal")] private static extern void FB_Save(string json);
        [DllImport("__Internal")] private static extern void FB_Load();
#else
        // Editor/other platforms: stubs to avoid the code to blow up
         private static void FB_SetTarget(string goName)
         {
             // Debug.Log("(Stub) FB_SetTarget "  + goName);
         }
        
         private static void FB_Save(string json)
         {
             Debug.Log("(Stub) FB_Save: " + json);
         }
        
         private static void FB_Load()
         {
             Debug.Log("(Stub) FB_Load");
         }
#endif

        public static void InitTarget(string gameObjectName) => FB_SetTarget(gameObjectName);
        public static void SaveJson(string json) => FB_Save(json);
        public static void Load() => FB_Load();
    }
}
