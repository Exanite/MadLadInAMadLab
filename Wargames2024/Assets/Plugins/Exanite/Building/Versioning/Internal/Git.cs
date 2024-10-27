// Notice:
// This code has been modified for use in Exanite.Building
//
// The original code can be found at the link below under the MIT License:
// Code: https://github.com/game-ci/unity-builder/blob/3032a4ab97a9bb3fe204eeecd61b5abcc14885d3/dist/default-build-script/Assets/Editor/UnityBuilderAction/
// License: https://github.com/game-ci/unity-builder/blob/3032a4ab97a9bb3fe204eeecd61b5abcc14885d3/LICENSE

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Exanite.Building.Versioning.Internal
{
    /// <summary>
    /// Used to run Git commands in C#.
    /// </summary>
    public static class Git
    {
        public static Regex IsDetachedHeadRegex { get; } = new(@"^\* \(HEAD detached", RegexOptions.Multiline);
        public static Regex GetBranchNameRegex { get; } = new(@"^\* (?<Branch>.+)$", RegexOptions.Multiline);

        public const string GitExecutableName = @"git";

        /// <summary>
        /// Generates a version based on the latest tag and the amount of
        /// commits.
        /// <para/>
        /// Format: 0.1.2.3 (where 3 is the amount of commits)
        /// </summary>
        public static string GenerateCommitVersion()
        {
            // If no tag is present in the repository then v0.0.0 is assumed
            // This would result in 0.0.0.# where # is the amount of commits

            string version;

            if (HasAnyVersionTags())
            {
                version = GetCommitInfo();
            }
            else
            {
                version = $"0.0.0.{GetTotalNumberOfCommits()}";
            }

            return version;
        }

        /// <summary>
        /// Gets the current checked out branch's name. May return null if the current commit is not on a branch.
        /// </summary>
        public static string GetBranchName()
        {
            // Properly handle Github Actions
            var headRef = Environment.GetEnvironmentVariable("GITHUB_HEAD_REF");
            if (!string.IsNullOrWhiteSpace(headRef))
            {
                return headRef.Trim();
            }

            if (IsDetachedHead())
            {
                return null;
            }

            var match = GetBranchNameRegex.Match(Run(@"branch"));
            if (!match.Success)
            {
                return null;
            }

            return match.Groups["Branch"].Value.Trim();
        }

        /// <summary>
        /// Gets the total number of commits.
        /// </summary>
        public static int GetTotalNumberOfCommits()
        {
            var numberOfCommitsAsString = Run(@"rev-list --count HEAD");

            return int.Parse(numberOfCommitsAsString);
        }

        /// <summary>
        /// Retrieves the build version from git based on the most recent
        /// matching tag and commit history.
        /// <para/>
        /// Format: 0.1.2.3 (where 3 is the amount of commits)
        /// </summary>
        public static string GetCommitInfo()
        {
            // v0.1-2-g12345678 (where 2 is the amount of commits, g stands for git)
            var version = GetVersionString();
            // 0.1-2
            version = version.Substring(1, version.LastIndexOf('-') - 1);
            // 0.1.2
            version = version.Replace('-', '.');

            return version;
        }

        public static string GetCommitHashShort()
        {
            return Run(@"rev-parse --short HEAD");
        }

        /// <summary>
        /// Gets version string.
        /// <para/>
        /// Format: v0.1-2-g12345678 (where 2 is the amount of commits since
        /// the last tag)
        /// </summary>
        public static string GetVersionString()
        {
            // Reference: https://softwareengineering.stackexchange.com/questions/141973/how-do-you-achieve-a-numeric-versioning-scheme-with-git

            return Run(@"describe --tags --long --match ""v[0-9]*""");
        }

        /// <summary>
        /// Whether or not the repository has any version tags yet.
        /// </summary>
        public static bool HasAnyVersionTags()
        {
            var output = Run(@"tag --list --merged HEAD");
            var regex = new Regex("v[0-9]*");

            var matches = regex.Matches(output);

            return matches.Count > 0;
        }

        /// <summary>
        /// Returns true if the current commit is not on a branch.
        /// </summary>
        public static bool IsDetachedHead()
        {
            return IsDetachedHeadRegex.IsMatch(Run(@"branch"));
        }

        /// <summary>
        /// Runs git binary with any given arguments and returns the output..
        /// </summary>
        public static string Run(string arguments)
        {
            using (var process = new Process())
            {
                var workingDirectory = Application.dataPath;

                var exitCode = process.Run(GitExecutableName, arguments, workingDirectory, out var output, out var errors);

                if (exitCode != 0)
                {
                    throw new GitException(exitCode, errors);
                }

                return output;
            }
        }
    }
}
