using UnityEditor;

namespace Yahtz.BaldiPG
{
    internal static class BaldiPGMenu
    {
        [MenuItem("Yahtz/Baldi PG/About", priority = 100)]
        private static void About()
        {
            EditorUtility.DisplayDialog(
                "Yahtz - Baldi PG",
                "Baldi PG\n\nPart Generator + NPC Dragger\n\nMade by Yahtz.",
                "OK");
        }
    }
}