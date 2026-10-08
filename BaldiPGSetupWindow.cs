using System.IO;
using UnityEditor;
using UnityEngine;

namespace Yahtz.BaldiPG
{
    public class BaldiPGSetupWindow : EditorWindow
    {
        [MenuItem("Yahtz/Baldi PG/Setup", priority = 0)]
        public static void Open()
        {
            GetWindow<BaldiPGSetupWindow>("Baldi PG Setup");
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Baldi PG Setup", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "Select your Baldi's Basics Steam/game folder or a decompile folder. " +
                "Baldi PG indexes the files you provide and does not include the game's assets.",
                MessageType.Info);

            GUILayout.Space(8);

            EditorGUILayout.LabelField("Game Folder");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(BaldiPGSettings.GameFolder);

            if (GUILayout.Button("Select...", GUILayout.Width(80)))
            {
                string folder = EditorUtility.OpenFolderPanel(
                    "Select Baldi's Basics Game Folder",
                    BaldiPGSettings.GameFolder,
                    "");

                if (!string.IsNullOrEmpty(folder))
                    BaldiPGSettings.GameFolder = folder;
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(8);

            bool valid = BaldiPGSettings.IsValidFolder();

            EditorGUILayout.HelpBox(
                valid
                    ? "Game folder detected. Baldi PG is ready to scan."
                    : "No valid game folder selected.",
                valid ? MessageType.Info : MessageType.Warning);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Scan Game"))
            {
                if (!valid)
                {
                    EditorUtility.DisplayDialog(
                        "Baldi PG",
                        "Please select a valid game folder first.",
                        "OK");
                }
                else
                {
                    int prefabs = BaldiPGAssetScanner.FindPrefabs(BaldiPGSettings.GameFolder).Count;
                    int models = BaldiPGAssetScanner.FindModels(BaldiPGSettings.GameFolder).Count;

                    EditorUtility.DisplayDialog(
                        "Baldi PG Scan Complete",
                        $"Folder:\n{BaldiPGSettings.GameFolder}\n\n" +
                        $"Prefab files found: {prefabs}\n" +
                        $"Model files found: {models}",
                        "OK");
                }
            }

            if (GUILayout.Button("Clear"))
            {
                BaldiPGSettings.Clear();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}