using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace xtcp.xjson
{
    internal static class Xjson
    {
        public static string XjsonMain(string json)
        {
            List<string> inputs = new List<string>();
            int openBrackets = 0;
            //int startChar = 0;
            int lastSpace = 0;
            for (int i = 0; i < json.Length; i++)
            {
                if (i == json.Length - 1)
                {
                    inputs.Add(json.Substring(lastSpace, (i - lastSpace) + 1));
                    break;
                }
                if (json[i] == '[' || json[i] == '{')
                {
                    //if (openBrackets == 0) {
                    //    startChar = i;
                    //}
                    openBrackets++;
                }
                else if (json[i] == ']' || json[i] == '}')
                {
                    openBrackets--;
                    //if (openBrackets == 0) {
                    //    inputs.Add(json.Substring(startChar, (i - startChar) + 1));
                    //}
                }
                else if (json[i] == ' ' && openBrackets == 0 && lastSpace != i)
                {
                    inputs.Add(json.Substring(lastSpace, (i - lastSpace) + 1));
                    lastSpace = i;
                }
            }
            if (inputs.Count == 0)
            {
                inputs.Add(json);
            }
            string output = "";
            foreach (string input in inputs)
            {
                using var stream = new MemoryStream();
                using var writer = new Utf8JsonWriter(stream);
                JsonDocument document = JsonDocument.Parse(input);
                JsonElement root = document.RootElement;
                JsonReverser.ReverseElement(writer, root);
                writer.Flush();
                output += Encoding.UTF8.GetString(stream.ToArray()) + " ";

            }
            output = output.Remove(output.Length - 1, 1);
            return output;
        }
    }
}