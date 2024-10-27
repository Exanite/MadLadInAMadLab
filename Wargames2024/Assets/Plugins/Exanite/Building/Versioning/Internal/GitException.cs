// Notice:
// This code has been modified for use in Exanite.Building
//
// The original code can be found at the link below under the MIT License:
// Code: https://github.com/game-ci/unity-builder/blob/3032a4ab97a9bb3fe204eeecd61b5abcc14885d3/dist/default-build-script/Assets/Editor/UnityBuilderAction/
// License: https://github.com/game-ci/unity-builder/blob/3032a4ab97a9bb3fe204eeecd61b5abcc14885d3/LICENSE

using System;

namespace Exanite.Building.Versioning.Internal
{
    /// <summary>
    /// The exception that is thrown when Git fails to exit successfully
    /// </summary>
    public class GitException : InvalidOperationException
    {
        /// <summary>
        /// Creates a new <see cref="GitException"/>
        /// </summary>
        public GitException(int exitCode, string errors) : base($"\n{errors}")
        {
            ExitCode = exitCode;
        }

        /// <summary>
        /// Exit code specified by Git
        /// </summary>
        public int ExitCode { get; }
    }
}
