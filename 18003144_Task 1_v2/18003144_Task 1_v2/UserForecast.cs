﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18003144_Task_1_v2
{
    public class UserForecast //Class to represent forecasts created by users
    {
        private int forecastID;
        private int cityID;
        private DateTime forecastDate;
        private int minimumTemp;
        private int maximumTemp;
        private int windSpeed;
        private int humidity;
        private int precipitation;


        public int ForecastID { get => forecastID; set => forecastID = value; }
        public int CityID { get => cityID; set => cityID = value; }
        public DateTime ForecastDate { get => forecastDate; set => forecastDate = value; }
        public int MinimumTemp { get => minimumTemp; set => minimumTemp = value; }
        public int MaximumTemp { get => maximumTemp; set => maximumTemp = value; }
        public int WindSpeed { get => windSpeed; set => windSpeed = value; }
        public int Humidity { get => humidity; set => humidity = value; }
        public int Precipitation { get => precipitation; set => precipitation = value; }

        public UserForecast(int ForecastID, int cityID, DateTime forecastDate, int minimumTemp, int maximumTemp, int windSpeed, int humidity, int precipitation)
        {
            this.ForecastID = ForecastID;
            this.cityID = cityID;
            this.forecastDate = forecastDate;
            this.minimumTemp = minimumTemp;
            this.maximumTemp = maximumTemp;
            this.windSpeed = windSpeed;
            this.humidity = humidity;
            this.precipitation = precipitation;
        }

        public string GetTextFileFormat() // returns string to write to text file to properly represent forecast
        {
            return cityID + "," + forecastDate.Date.ToShortDateString() + "," + minimumTemp + "," + maximumTemp + "," + windSpeed + "," + humidity + "," + precipitation;
        }

    }
}
