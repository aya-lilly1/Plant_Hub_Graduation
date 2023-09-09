namespace Plant_Hub.ModelServices
{
    public interface IPythonScriptExcutor
    {
        Task<string> ExecuteAsync(string rootPath, string scriptName , string imgeFile, string model);
    }
}
