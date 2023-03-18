using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using DoraTheExplorer.Structure;

namespace DoraTheExplorer.Util;

public class ReadJSON
{
    public class ResultJSONItem
    {
        public List<Coordinate> visitedLocations;
        public List<Coordinate> backtrackLocations;
        public Coordinate CurrentLocation;
    }
    public static object Deserialize(string filepath)
    {
        var serializer = new JsonSerializer();

        using (var sw = new StreamReader(filepath))
        using (var reader = new JsonTextReader(sw))
        {
            return serializer.Deserialize(reader);
        }
    }
}