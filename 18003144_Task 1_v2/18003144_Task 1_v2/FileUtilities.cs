using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18003144_Task_1_v2
{
    class FileUtilities
    {
        public static List<UserForecast> getForecastsFromFile()
        {
            List<UserForecast> forecasts = new List<UserForecast>();
            using (StreamReader file = new StreamReader("Forecasts.txt"))
            {
                string line = file.ReadLine();
                while (line != null)
                {
                    var lineParts = line.Split(',');
                    forecasts.Add(new UserForecast(Convert.ToInt32(lineParts[0]), Convert.ToDateTime(lineParts[1]), Convert.ToInt32(lineParts[2]), Convert.ToInt32(lineParts[3]), Convert.ToInt32(lineParts[4]), Convert.ToInt32(lineParts[5]), Convert.ToInt32(lineParts[6])));
                    line = file.ReadLine();
                }
            }
            return forecasts;
        }
    }
}
