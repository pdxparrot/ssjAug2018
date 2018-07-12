using System;
using System.Diagnostics;

namespace pdxpartyparrot.Editor
{
    public static class Executor
    {
        private static bool ExecuteProcess(Process process, Action<Process> onExit, bool redirectOutput=true, bool redirectError=true)
        {
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;

            if(redirectOutput) {
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += (sender, args) => {
                    UnityEngine.Debug.Log($"[Process Output]: {args.Data}");
                };
            }

            if(redirectError) {
                process.StartInfo.RedirectStandardError = true;
                process.ErrorDataReceived += (sender, args) => {
                    UnityEngine.Debug.LogError($"[Process Error]: {args.Data}");
                };
            }

            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => {
                onExit(process);
            };

            UnityEngine.Debug.Log($"Executing process '{process.StartInfo.FileName} {process.StartInfo.Arguments}'...");
            if(!process.Start()) {
                UnityEngine.Debug.LogError($"Unable to execute process '{process.StartInfo.FileName}'!");
                return false;
            }

            if(redirectOutput) {
                process.BeginOutputReadLine();
            }

            if(redirectError) {
                process.BeginErrorReadLine();
            }

            return true;
        }

        public static bool ExecuteScriptFromCurrentDirectory(string filename, Action<Process> onExit, string arguments="")
        {
            Process process = new Process
            {
                StartInfo =
                {
                    FileName = filename,
                    Arguments = arguments
                },
            };
            return ExecuteProcess(process, onExit);
        }

        public static bool ExecuteScript(string workingDirectory, string filename, Action<Process> onExit, string arguments="")
        {
            Process process = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = workingDirectory,
                    FileName = filename,
                    Arguments = arguments
                },
            };
            return ExecuteProcess(process, onExit);
        }
    }
}
