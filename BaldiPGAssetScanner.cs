using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Yahtz.BaldiPG
{
    internal static class BaldiPGAssetScanner
    {
        public static List<string> FindPrefabs(string folder)
        {
            var results = new List<string>();

            if (string.IsNullOrEmpty(folder) || !Directory.Exists(folder))
                return results;

            foreach (var file in Directory.GetFiles(folder, "*.prefab", SearchOption.AllDirectories))
                results.Add(file);

            return results;
        }

        public static List<string> FindModels(string folder)
        {
            var results = new List<string>();

            if (string.IsNullOrEmpty(folder) || !Directory.Exists(folder))
                return results;

            foreach (var ext in new[] { "*.fbx", "*.obj", "*.dae" })
            {
                foreach (var file in Directory.GetFiles(folder, ext, SearchOption.AllDirectories))
                    results.Add(file);
            }

            return results;
        }

        public static string ToAssetPath(string absolutePath)
        {
            if (string.IsNullOrEmpty(absolutePath))
                return "";

            string projectAssets = Path.GetFullPath(Application.dataPath)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            string full = Path.GetFullPath(absolutePath);

            if (full.StartsWith(projectAssets, StringComparison.OrdinalIgnoreCase))
            {
                string relative = full.Substring(projectAssets.Length)
                    .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                return "Assets/" + relative.Replace('\\', '/');
            }

            return "";
        }
    }
}