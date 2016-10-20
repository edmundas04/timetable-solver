using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TimetableSolver.Samples.Models;

namespace TimetableSolver.Samples
{
    public static class HtmlExportHelper
    {
        private static readonly Dictionary<string, string>  _resourcePathMap = new Dictionary<string, string>
        {
            { "TimetableSolver.Samples.resources.angular.angular.js", "angular\\angular.js" },
            { "TimetableSolver.Samples.resources.angular.angular.min.js", "angular\\angular.min.js" },
            { "TimetableSolver.Samples.resources.app.config.js", "app\\config.js" },
            { "TimetableSolver.Samples.resources.app.timetable_body.timetable-body.component.js", "app\\timetable-body\\timetable-body.component.js" },
            { "TimetableSolver.Samples.resources.app.timetable_body.timetable_row.timetable-row.component.js", "app\\timetable-body\\timetable-row\\timetable-row.component.js" },
            { "TimetableSolver.Samples.resources.app.timetable_header.timetable-header.component.js", "app\\timetable-header\\timetable-header.component.js" },
            { "TimetableSolver.Samples.resources.app.timetable.component.js", "app\\timetable.component.js" },
            { "TimetableSolver.Samples.resources.styles.css", "styles.css" },
            { "TimetableSolver.Samples.resources.angular.angular.d.ts", "angular\\angular.d.ts" }
        };

        private static readonly List<string> direcotries = new List<string>
        {
            "angular", "app\\timetable-body\\timetable-row", "app\\timetable-header"
        };

        public static string PrepareEnvironment()
        {
            var folderName = DateTime.Now.ToString("yyyy-MM-dd hhmmss");
            string currentPath = Directory.GetCurrentDirectory();
            var path = Path.Combine(currentPath, folderName);
            Directory.CreateDirectory(path);

            var resouceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(x => x.StartsWith("TimetableSolver.Samples.resources") 
                && x != "TimetableSolver.Samples.resources.index.html");


            foreach (var directory in direcotries)
            {
                Directory.CreateDirectory(Path.Combine(path, directory));
            }
            foreach (var resouceName in resouceNames)
            {
                var extractionPath = Path.Combine(path, _resourcePathMap[resouceName]);
                using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(resouceName))
                using (FileStream resourceFile = new FileStream(extractionPath, FileMode.Create))
                {
                    byte[] b = new byte[s.Length + 1];
                    s.Read(b, 0, Convert.ToInt32(s.Length));
                    resourceFile.Write(b, 0, Convert.ToInt32(b.Length - 1));
                    resourceFile.Flush();
                    resourceFile.Close();
                }
            }

            return folderName;
        }

        public static void ExportHtml(TimetableInfo timetableInfo, string environment, string fileName)
        {
            var viewModel = new TimetableInfoViewModel(timetableInfo);
            var jsonSerializationSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var viewModelJson = JsonConvert.SerializeObject(viewModel, jsonSerializationSettings);


            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "TimetableSolver.Samples.resources.index.html";
            string fileContent;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                fileContent = reader.ReadToEnd().Replace("$jsonData", viewModelJson);
            }

            string currentPath = Directory.GetCurrentDirectory();
            var path = Path.Combine(currentPath, environment);
            path = Path.Combine(path, fileName + ".html");

            using (FileStream fs = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(fileContent);
                fs.Write(info, 0, info.Length);
            }
        }
    }
}
