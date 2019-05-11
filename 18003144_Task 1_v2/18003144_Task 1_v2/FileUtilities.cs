using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _18003144_Task_1_v2
{
    class FileUtilities //Class to hold methods used regularly to fetch data from files
    {
        //Method to loop through file and return list of forecast objects
        public static List<UserForecast> GetForecastsFromFile()
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

        //Randomly select background
        public static ImageBrush ChooseBackground()
        {
            string directoryPath = Directory.GetCurrentDirectory() + "/BackgroundImages/";
            int fileCount = Directory.GetFiles(directoryPath, "*", SearchOption.TopDirectoryOnly).Length;
            string imageName = Directory.GetFiles(directoryPath, "*", SearchOption.TopDirectoryOnly)[new Random().Next(fileCount)];
            ImageBrush backgroundBrush = new ImageBrush();
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(@imageName));
            backgroundBrush.ImageSource = image.Source;
            backgroundBrush.Opacity = 0.3;
            return backgroundBrush;

        }

        //Method to loop through file and return list of User objects
        public static List<User> GetUsersFromFile()
        {
            List<User> users = new List<User>();
            using (StreamReader sr = new StreamReader("Users.txt"))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    var lineParts = line.Split(',');
                    users.Add(new User(lineParts[0], lineParts[1], (UserType)Convert.ToInt16(lineParts[2])));
                    line = sr.ReadLine();
                }

            }
            return users;
        }
    }
}
