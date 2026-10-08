using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Yahtz.BaldiPG
{
    public class BaldiPGPartGeneratorWindow : EditorWindow
    {
        private enum Category
        {
            All,
            Rooms,
            Hallways,
            Stairs,
            Walls,
            Props
        }

        private readonly List<string> prefabs = new List<string>();
        private Vector2 scroll;
        private string search = "";
        private Category category = Category.All;

        [MenuItem("Yahtz/Baldi PG/Part Generator", priority = 10)]
        public static void Open()
        {
            GetWindow<BaldiPGPartGeneratorWindow>("Part Generator");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Part Generator", EditorStyles.boldLabel);

            search = EditorGUILayout.TextField("Search", search);
            category = (Category)EditorGUILayout.EnumPopup("Category", category);

            if (GUILayout.Button("Scan Game Parts"))
                Scan();

            GUILayout.Space(5);

            if (prefabs.Count == 0)
            {
                EditorGUILayout.HelpBox(
                    BaldiPGSettings.IsValidFolder()
                        ? "No prefabs indexed. Click Scan Game Parts."
                        : "Select your game/decompile folder in Baldi PG Setup first.",
                    MessageType.Warning);
                return;
            }

            scroll = EditorGUILayout.BeginScrollView(scroll);

            foreach (string path in prefabs)
            {
                string name = Path.GetFileNameWithoutExtension(path);

                if (!MatchesSearch(name))
                    continue;

                if (!MatchesCategory(path, name))
                    continue;

                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField(name);

                if (GUILayout.Button("Place", GUILayout.Width(55)))
                    PlacePrefab(path);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        private bool MatchesSearch(string name)
        {
            return string.IsNullOrEmpty(search) ||
                   name.IndexOf(search, System.StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool MatchesCategory(string path, string name)
        {
            if (category == Category.All)
                return true;

            string text = (path + " " + name).ToLowerInvariant();

            switch (category)
            {
                case Category.Rooms:
                    return text.Contains("room") || text.Contains("classroom") ||
                           text.Contains("office") || text.Contains("cafeteria");

                case Category.Hallways:
                    return text.Contains("hall") || text.Contains("corridor");

                case Category.Stairs:
                    return text.Contains("stair") || text.Contains("step") ||
                           text.Contains("landing");

                case Category.Walls:
                    return text.Contains("wall") || text.Contains("door") ||
                           text.Contains("window");

                case Category.Props:
                    return text.Contains("prop") || text.Contains("desk") ||
                           text.Contains("chair") || text.Contains("locker");

                default:
                    return true;
            }
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
            Undo.RegisterCreatedObjectUndo(instance, "Place Baldi PG Part");
            Selection.activeGameObject = instance;

            SceneView scene = SceneView.lastActiveSceneView;
            if (scene != null)
                instance.transform.position = scene.camera.transform.position +
                                              scene.camera.transform.forward * 3f;
        }
    }
}