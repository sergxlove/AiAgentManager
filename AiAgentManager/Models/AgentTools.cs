using System.Diagnostics;
using System.Text;

namespace AiAgentManager.Models
{
    public class AgentTools
    {
        public static bool IsAgentRunning(Dictionary<string, AgentProcess> processes, object _lock, 
            string name)
        {
            lock (_lock)
            {
                return processes.ContainsKey(name) && !processes[name].Process.HasExited;
            }
        }

        public static bool StartAgentProcess(Dictionary<string, AgentProcess> processes, object _lock, 
            string name, string exePath)
        {
            lock (_lock)
            {
                if (processes.ContainsKey(name) && !processes[name].Process.HasExited)
                    return false;

                if (!File.Exists(exePath))
                    return false;

                try
                {
                    var process = new Process();
                    process.StartInfo.FileName = exePath;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.StandardOutputEncoding = Encoding.Default;
                    process.StartInfo.StandardErrorEncoding = Encoding.Default;

                    process.Start();

                    processes[name] = new AgentProcess
                    {
                        Process = process,
                        StartTime = DateTime.UtcNow
                    };

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool StopAgentInternal(Dictionary<string, AgentProcess> processes, object _lock, 
            string name)
        {
            lock (_lock)
            {
                if (!processes.TryGetValue(name, out var agentProcess))
                    return false;

                try
                {
                    var process = agentProcess.Process;

                    if (!process.HasExited)
                    {
                        try
                        {
                            process.StandardInput.WriteLine("exit");
                            process.StandardInput.Flush();
                            Thread.Sleep(500);
                        }
                        catch { }

                        if (!process.HasExited)
                        {
                            process.Kill();
                        }
                    }

                    process.Dispose();
                    processes.Remove(name);

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static async Task<string> SendCommandAndGetResponse(Dictionary<string, AgentProcess> processes,
            object _lock, string name, string command)
        {
            lock (_lock)
            {
                if (!processes.TryGetValue(name, out var agentProcess))
                    return "Ошибка: агент не запущен";

                var process = agentProcess.Process;

                if (process.HasExited)
                {
                    processes.Remove(name);
                    return "Ошибка: агент завершил работу";
                }

                try
                {
                    process.StandardInput.WriteLine(command);
                    process.StandardInput.Flush();
                    var timeout = TimeSpan.FromSeconds(30);
                    var startTime = DateTime.UtcNow;
                    var output = new StringBuilder();

                    while ((DateTime.UtcNow - startTime) < timeout)
                    {
                        if (process.StandardOutput.Peek() > -1)
                        {
                            var line = process.StandardOutput.ReadLine();
                            if (line != null)
                            {
                                output.AppendLine(line);
                                if (string.IsNullOrWhiteSpace(line) && output.Length > 0)
                                    break;
                            }
                        }
                        else
                        {
                            Thread.Sleep(100);
                        }
                    }

                    var result = output.ToString().Trim();
                    return string.IsNullOrEmpty(result) ? "Команда выполнена (ответ не получен)" : result;
                }
                catch (Exception ex)
                {
                    return $"Ошибка: {ex.Message}";
                }
            }
        }
    }
}
