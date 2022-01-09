using GotYourBackLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotYourBackLibrary.DataAccess
{
    public class CSVConnector
    {
        public List<MonitoredFileModel> LoadFileToList(string filePath)
        {
            List<MonitoredFileModel> output = new();
            if (!File.Exists(filePath))
            {
                return output;
            }
            List<string> lines = File.ReadAllLines(filePath).ToList();
            foreach (string line in lines)
            {
                string[] cols = line.Split('\u002C'); //Comma
                MonitoredFileModel model = new MonitoredFileModel();
                model.FilePath = cols[0];
                model.FileDate = Convert.ToDateTime(cols[1],System.Globalization.CultureInfo.CurrentCulture);
                output.Add(model);
            }
            return output;
        }

        public void SaveListToFile(List<MonitoredFileModel> models, string filePath)
        {
            List<string> lines = new List<string>();
            // Convert each model into a comma seperated string
            foreach (MonitoredFileModel model in models)
            {
                lines.Add($"{model.FilePath},{model.FileDate.ToString("o", System.Globalization.CultureInfo.CurrentCulture)}");
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}
