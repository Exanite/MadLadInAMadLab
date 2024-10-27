using Exanite.Building.Versioning.Internal;

namespace Exanite.Building.Versioning
{
    /// <summary>
    /// Contains information about the game and its current version
    /// </summary>
    public struct GameVersion
    {
        /// <summary>
        /// The default version.
        /// <para/>
        /// Note: This is also used when the Git commit version generation
        /// fails (Git was not found in Path).
        /// </summary>
        public static GameVersion DefaultVersion => new("branch", "0.0.0.0", "0");

        public GameVersion(string branch, string commitVersion, string commitHash)
        {
            Branch = branch;
            CommitVersion = commitVersion;
            CommitHash = commitHash;
        }

        public string Branch { get; }
        public string CommitVersion { get; }
        public string CommitHash { get; }

        public override string ToString()
        {
            if (Branch != null)
            {
                return $"{Branch}/{CommitVersion} {CommitHash}";
            }
            else
            {
                return $"{CommitHash}";
            }
        }

        public static GameVersion Generate()
        {
            return new GameVersion(Git.GetBranchName(), Git.GenerateCommitVersion(), Git.GetCommitHashShort());
        }
    }
}
