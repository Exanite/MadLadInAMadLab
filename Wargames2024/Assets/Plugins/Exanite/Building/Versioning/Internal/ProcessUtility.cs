// Notice:
// This code has been modified for use in Exanite.Building
//
// The original code can be found at the link below under the MIT License:
// Code: https://github.com/game-ci/unity-builder/blob/3032a4ab97a9bb3fe204eeecd61b5abcc14885d3/dist/default-build-script/Assets/Editor/UnityBuilderAction/
// License: https://github.com/game-ci/unity-builder/blob/3032a4ab97a9bb3fe204eeecd61b5abcc14885d3/LICENSE

using System.Diagnostics;
using System.Text;

namespace Exanite.Building.Versioning.Internal
{
    public static class ProcessUtility
    {
        /// <summary>
        /// Executes an application with given arguments
        /// </summary>
        /// <param name="process">
        /// The <see cref="Process"/> to use
        /// </param>
        /// <param name="application">
        /// Application for the <see cref="Process"/> to run
        /// </param>
        /// <param name="arguments">
        /// Arguments to supply to the <see cref="Process"/>
        /// </param>
        /// <param name="workingDirectory">
        /// Directory the <see cref="Process"/> will run in
        /// </param>
        /// <param name="stdout">
        /// <see cref="string"/> containing output from the
        /// <see cref="Process"/>
        /// </param>
        /// <param name="stderr">
        /// <see cref="string"/> containing errors from the
        /// <see cref="Process"/>
        /// </param>
        /// <returns>
        /// The process exit code
        /// </returns>
        public static int Run(
            this Process process,
            string application,
            string arguments,
            string workingDirectory,
            out string stdout,
            out string stderr)
        {
            // Configure how to run the application
            process.StartInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                FileName = application,
                Arguments = arguments,
                WorkingDirectory = workingDirectory,
            };

            // Read the output
            var outputBuilder = new StringBuilder();
            var errorsBuilder = new StringBuilder();
            process.OutputDataReceived += (_, args) => outputBuilder.AppendLine(args.Data);
            process.ErrorDataReceived += (_, args) => errorsBuilder.AppendLine(args.Data);

            // Run the application and wait for it to complete
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            // Format the output
            stdout = outputBuilder.ToString().TrimEnd();
            stderr = errorsBuilder.ToString().TrimEnd();

            return process.ExitCode;
        }
    }
}
