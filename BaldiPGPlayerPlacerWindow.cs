using UnityEditor;
using UnityEngine;

namespace Yahtz.BaldiPG
{
    public class BaldiPGPlayerPlacerWindow : EditorWindow
    {
        private bool snapToGrid = true;
        private bool alignToFloor = true;
        private float gridSize = 0.5f;
        private Vector2 scroll;

        [MenuItem("Yahtz/Baldi PG/Player Placer")]
        public static void Open()
        {
            GetWindow<BaldiPGPlayerPlacerWindow>("Player Placer");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Baldi PG - Player Placer", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Create and manage player spawn points.", MessageType.Info);

            snapToGrid = EditorGUILayout.Toggle("Snap to Grid", snapToGrid);
            using (new EditorGUI.DisabledScope(!snapToGrid))
                gridSize = Mathf.Max(0.01f, EditorGUILayout.FloatField("Grid Size", gridSize));

            alignToFloor = EditorGUILayout.Toggle("Align to Floor", alignToFloor);

            EditorGUILayout.Space(8);

            if (GUILayout.Button("Place Player Spawn", GUILayout.Height(32)))
                PlaceSpawn();

            if (GUILayout.Button("Set Selected as Default"))
                SetSelectedAsDefault();

            if (GUILayout.Button("Move Selected to Scene View"))
                MoveSelectedToSceneView();

            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField("Spawn Points", EditorStyles.boldLabel);

            scroll = EditorGUILayout.BeginScrollView(scroll);
            var spawns = FindObjectsByType<BaldiPGPlayerSpawn>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

            foreach (var spawn in spawns)
            {
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button(spawn.name, EditorStyles.miniButton))
                {
                    Selection.activeGameObject = spawn.gameObject;
                    EditorGUIUtility.PingObject(spawn.gameObject);
                }

                if (spawn.isDefaultSpawn)
                    EditorGUILayout.LabelField("DEFAULT", GUILayout.Width(65));

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        private void PlaceSpawn()
        {
            Vector3 position = Vector3.zero;

            if (SceneView.lastActiveSceneView != null)
            {
                var cam = SceneView.lastActiveSceneView.camera;
                position = cam.transform.position + cam.transform.forward * 5f;
            }

            if (snapToGrid)
                position = Snap(position);

            if (alignToFloor)
                position = FindFloor(position);

            var obj = new GameObject("Player Spawn");
            Undo.RegisterCreatedObjectUndo(obj, "Create Player Spawn");
            obj.transform.position = position;

            if (SceneView.lastActiveSceneView != null)
                obj.transform.rotation = Quaternion.Euler(
                    0f,
                    SceneView.lastActiveSceneView.camera.transform.eulerAngles.y,
                    0f
                );

            obj.AddComponent<BaldiPGPlayerSpawn>();
            Selection.activeGameObject = obj;
        }

        private Vector3 Snap(Vector3 p)
        {
            return new Vector3(
                Mathf.Round(p.x / gridSize) * gridSize,
                Mathf.Round(p.y / gridSize) * gridSize,
                Mathf.Round(p.z / gridSize) * gridSize
            );
        }

        private Vector3 FindFloor(Vector3 p)
        {
            if (Physics.Raycast(p + Vector3.up * 50f, Vector3.down, out RaycastHit hit, 100f))
                return hit.point;

            return p;
        }

        private void SetSelectedAsDefault()
        {
            var selected = Selection.activeGameObject;
            if (selected == null)
                return;

            var target = selected.GetComponent<BaldiPGPlayerSpawn>();
            if (target == null)
                return;

            var spawns = FindObjectsByType<BaldiPGPlayerSpawn>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

            foreach (var spawn in spawns)
            {
                Undo.RecordObject(spawn, "Set Default Player Spawn");
                spawn.isDefaultSpawn = spawn == target;
                EditorUtility.SetDirty(spawn);
            }
        }

        private void MoveSelectedToSceneView()
        {
            var selected = Selection.activeGameObject;
            if (selected == null || selected.GetComponent<BaldiPGPlayerSpawn>() == null)
                return;

            if (SceneView.lastActiveSceneView == null)
                return;

            Undo.RecordObject(selected.transform, "Move Player Spawn");

            var cam = SceneView.lastActiveSceneView.camera;
            selected.transform.position = cam.transform.position + cam.transform.forward * 5f;

            if (snapToGrid)
                selected.transform.position = Snap(selected.transform.position);

            if (alignToFloor)
                selected.transform.position = FindFloor(selected.transform.position);
        }
    }
}
