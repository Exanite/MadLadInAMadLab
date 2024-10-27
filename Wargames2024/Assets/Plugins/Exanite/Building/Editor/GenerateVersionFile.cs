#if UNITY_EDITOR
using System;
using System.IO;
using Exanite.Building.Versioning;
using Exanite.Building.Versioning.Internal;
using UnityEditor;
using UnityEngine;
#if EXANITE_BUILDING_GENERATE_VERSION_FILE
using UnityEditor.Callbacks;
#endif

namespace Exanite.Building.Editor
{
    public static class GenerateVersionFile
    {
#if EXANITE_BUILDING_GENERATE_VERSION_FILE
        [PostProcessBuild]
#endif
        public static void Generate(BuildTarget target, string pathToBuiltProject)
        {
            var githubWorkspace = Environment.GetEnvironmentVariable("GITHUB_WORKSPACE");
            if (githubWorkspace != null)
            {
                var arguments = $"config --global --add safe.directory {githubWorkspace}";

                Debug.Log($"Running git with arguments: {arguments}");
                Git.Run(arguments);
            }

            var buildDirectoryPath = Path.GetDirectoryName(pathToBuiltProject);
            var versionFilePath = Path.Combine(buildDirectoryPath, "VERSION");

            File.WriteAllText(versionFilePath, GameVersion.Generate().ToString());
        }
    }
}
#endif
