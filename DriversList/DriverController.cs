using CSVParser;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace DriversList
{
    public class DriverController
    {
        /// <summary>
        /// Gets All Driver Models from driverquery /v command
        /// </summary>
        public ObservableCollection<DriverModel> DriverModel
        {
            get
            {
                var driverModelList = new ObservableCollection<DriverModel>();
                var allInfo = (GetDriverQueryOutput()).Split('\n');
                var modelDescription = allInfo.FirstOrDefault();
                var parser = new CSVParser<DriverModel>(modelDescription);
                var parsedModels = parser.ParseCSV(string.Join("\n", allInfo.Skip(1)));
                foreach (var item in parsedModels)
                {
                    driverModelList.Add(item);
                }
                return driverModelList;
            }
        }

        private static string GetDriverQueryOutput()
        {
            using (var p = new Process())
            {
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = @"/c driverquery /v /fo csv";
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardInput = false;
                p.StartInfo.UseShellExecute = false;

                p.Start();
                var output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                return output;
            }
        }
    }
}