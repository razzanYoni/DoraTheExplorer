using System;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace DoraTheExplorer.Util;

public class WriteToJSON
{
    public static async Task WriteToJSONFileAsync<T>(string filePath, T data)
    {
        string jsonString = JsonSerializer.Serialize(data);
        
        filePath = filePath.Substring(0, filePath.Length - 4);
        
        System.IO.FileInfo file = new System.IO.FileInfo("./Result/" + filePath + ".json");
        file.Directory.Create(); // If the directory already exists, this method does nothing.
        
        await File.WriteAllTextAsync(file.FullName, jsonString);
    }
}