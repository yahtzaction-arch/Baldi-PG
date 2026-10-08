#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Yahtz.BaldiPG
{
    [CustomEditor(typeof(BaldiPGPlayerSpawn))]
    public class BaldiPGPlayerSpawnGizmos : Editor
    {
        private void OnSceneGUI()
        {
            var spawn = (BaldiPGPlayerSpawn)target;
            Handles.color = spawn.isDefaultSpawn ? Color.green : Color.cyan;
            Handles.DrawWireDisc(spawn.transform.position, Vector3.up, 0.35f);
            Handles.DrawLine(
                spawn.transform.position,
                spawn.transform.position + spawn.transform.forward * 1.25f
            );
            Handles.Label(
                spawn.transform.position + Vector3.up * 0.5f,
                spawn.isDefaultSpawn ? "PLAYER SPAWN (DEFAULT)" : "PLAYER SPAWN"
            );
        }
    }
}
#endif
