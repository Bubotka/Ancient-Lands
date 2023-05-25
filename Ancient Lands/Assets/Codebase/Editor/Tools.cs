using System.IO;
using UnityEditor;
using UnityEngine;

namespace Codebase.Editor
{
    public class Tools 
    {
        [MenuItem("Tools/ClearSave")]
        public static void ClearSave()
        {
            string path = Application.persistentDataPath + "/SaveData.json";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                Debug.Log("File was cleared");
            }
        }
    }
}
  