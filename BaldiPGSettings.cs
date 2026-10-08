using UnityEditor;
using UnityEngine;

namespace Yahtz.BaldiPG
{
    internal static class BaldiPGSettings
    {
        private const string FolderKey = "Yahtz.BaldiPG.GameFolder";

        public static string GameFolder
        {
            get => EditorPrefs.GetString(FolderKey, "");
            set => EditorPrefs.SetString(FolderKey, value ?? "");
        }

        public static bool IsValidFolder()
        {
            return !string.IsNullOrEmpty(GameFolder) && System.IO.Directory.Exists(GameFolder);
        }

        public static void Clear()
        {
            EditorPrefs.DeleteKey(FolderKey);
        }
    }
}