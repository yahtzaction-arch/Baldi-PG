using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Yahtz.BaldiPG
{
    public class BaldiPGNPCDraggerWindow : EditorWindow
    {
        private readonly List<string> prefabs = new List<string>();
        private Vector2 scroll;
        private string search = "";

        [MenuItem("Yahtz/Baldi PG/NPC Dragger", priority = 20)]
        public static void Open()
        {
            GetWindow<BaldiPGNPCDraggerWindow>("NPC Dragger");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("NPC Dragger", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "Drag a discovered prefab into the Scene. " +
                "For best results, use prefabs from your Unity/decompile project.",
                MessageType.Info);

            search = EditorGUILayout.TextField("Search", search);

            if (GUILayout.Button("Scan Prefabs"))
                Scan();

            GUILayout.Space(5);

            if (prefabs.Count == 0)
            {
                EditorGUILayout.HelpBox(
                    BaldiPGSettings.IsValidFolder()
                        ? "No prefabs indexed. Click Scan Prefabs."
                        : "Select your game/decompile folder in Baldi PG Setup first.",
                    MessageType.Warning);
                return;
            }

            scroll = EditorGUILayout.BeginScrollView(scroll);

            foreach (string path in prefabs)
            {
                string name = Path.GetFileNameWithoutExtension(path);

                if (!string.IsNullOrEmpty(search) &&
                    name.IndexOf(search, System.StringComparison.OrdinalIgnoreCase) < 0)
                    continue;

                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField(name);

                if (GUILayout.Button("Select", GUILayout.Width(55)))
                {
                    GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (asset != null)
                        Selection.activeObject = asset;
                }

                if (GUILayout.Button("Place", GUILayout.Width(55)))
                    PlacePrefab(path);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        private void Scan()
        {
            prefabs.Clear();

            if (!BaldiPGSettings.IsValidFolder())
                return;

            foreach (string absolute in BaldiPGAssetScanner.FindPrefabs(BaldiPGSettings.GameFolder))
            {
                string assetPath = BaldiPGAssetScanner.ToAssetPath(absolute);
                if (!string.IsNullOrEmpty(assetPath))
                    prefabs.Add(assetPath);
            }

            Repaint();
        }

        private static void PlacePrefab(string path)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null)
                return;

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            Undo.RegisterCreatedObjectUndo(instance, "Place Baldi PG NPC");
            Selection.activeGameObject = instance;

            SceneView scene = SceneView.lastActiveSceneView;
            if (scene != null)
                instance.transform.position = scene.camera.transform.position +
                                              scene.camera.transform.forward * 3f;
        }
    }
}