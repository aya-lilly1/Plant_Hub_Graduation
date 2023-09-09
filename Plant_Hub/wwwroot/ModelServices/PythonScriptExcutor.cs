using System.Diagnostics;

namespace Plant_Hub.ModelServices
{
    public class PythonScriptExcutor : IPythonScriptExcutor
    {
        public async Task<string> ExecuteAsync(string rootPath, string scriptName, string imgeFile, string model)
        {
             rootPath += @"\ModelServices\";
             string arguments = $"{rootPath + @"Scripts\" + scriptName}" + $"{imgeFile}" + $"{model}";

            ProcessStartInfo start = new()
            {
                FileName = rootPath + @"Interpreter\python-3.11.5-amd64.exe",
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true

            };
            using (Process process = await Task.Run(() => Process.Start(start)))
            {
                StreamReader reader = process.StandardOutput;
                var result = reader.ReadToEnd().Trim();
                Console.WriteLine(result);
                return result;
            }

            
        }
    }
}
