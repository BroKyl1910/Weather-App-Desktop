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

    class DataUtilities //Class to hold methods used regularly to fetch data from files
    {

        static WeatherForecastAppEntities db = new WeatherForecastAppEntities();

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

        public static List<UserForecast> GetForecastsFromDB()
        {
            return db.DB_Forecast.ToList().Select(f => new UserForecast(f.ForecastID, f.CityID, f.ForecastDate, f.MinTemp, f.MaxTemp, f.WindSpeed, f.Humidity, f.Precipitation)).ToList();
        }
        public static void InsertForecast(UserForecast newForecast)
        {
            DB_Forecast dbForecast = new DB_Forecast();

            dbForecast.CityID = newForecast.CityID;
            dbForecast.ForecastDate = newForecast.ForecastDate;
            dbForecast.MinTemp = newForecast.MinimumTemp;
            dbForecast.MaxTemp = newForecast.MaximumTemp;
            dbForecast.WindSpeed = newForecast.WindSpeed;
            dbForecast.Humidity = newForecast.Humidity;
            dbForecast.Precipitation = newForecast.Precipitation;


            db.DB_Forecast.Add(dbForecast);
            db.SaveChanges();
        }
        public static void EditForecast(int forecastID, UserForecast newForecast)
        {
            DB_Forecast forecast = db.DB_Forecast.Find(forecastID);
            forecast.CityID = newForecast.CityID;
            forecast.ForecastDate = newForecast.ForecastDate;
            forecast.MinTemp = newForecast.MinimumTemp;
            forecast.MaxTemp= newForecast.MaximumTemp;
            forecast.WindSpeed = newForecast.WindSpeed;
            forecast.Humidity = newForecast.Humidity;
            forecast.Precipitation = newForecast.Precipitation;

            db.SaveChanges();
        }
        public static void DeleteForecast(UserForecast forecast)
        {
            db.DB_Forecast.Remove(db.DB_Forecast.Find(forecast.ForecastID));
            db.SaveChanges();
        }

        public static List<User> GetUsersFromDB()
        {
            return db.DB_User.ToList().Select(u => new User(u.Username, u.Password, (UserType)u.UserType)).ToList();
        }
        public static void InsertUser(User user)
        {
            DB_User dbUser = new DB_User();
            dbUser.Username = user.Username;
            dbUser.Password = user.Password;
            dbUser.UserType = (int) user.UserType;

            db.DB_User.Add(dbUser);
            db.SaveChanges();
        }

        public static List<int> GetFavouriteCityIDsFromDB(string username)
        {
            return db.DB_Favourite.Where(f => f.Username.Equals(username)).Select(c => c.CityID).ToList();
        }

        public static void AddFavouriteCity(string username, int cityID)
        {
            db.DB_Favourite.Add(new DB_Favourite { Username = username, CityID = cityID });

            db.SaveChanges();
        }

        public static void DeleteFavouriteCity(string username, int cityID)
        {
            db.DB_Favourite.Remove(db.DB_Favourite.Where(fav => fav.Username.Equals(username) && fav.CityID == cityID).First());
            db.SaveChanges();
        }
    }
}
