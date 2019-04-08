using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18003144_Task_1_v2
{
    class CityUtilities //Class to hold methods and properties relating to Cities
    {
        //Dictionaries for rest of program
        public static Dictionary<int, City> CityCodeDict;

        public static Dictionary<string, City> CityNameDict;



        /*When called, if dictionary is already made, it returns it, 
        otherwise it makes and returns the required dictionary*/
        public static Dictionary<int, City> getCityCodeDict()
        {

            if (CityCodeDict == null)
            {
                CityCodeDict = new Dictionary<int, City>();

                CityFile cityFile;
                using (StreamReader file = File.OpenText("cities.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    cityFile = (CityFile)serializer.Deserialize(file, typeof(CityFile));
                }

                for (int i = 0; i < cityFile.cities.Length; i++)
                {
                    City city = cityFile.cities[i];
                    CityCodeDict.Add(city.id, city);
                }
            }


            return CityCodeDict;
        }

        public static Dictionary<string, City> getNameCityDict()
        {

            if (CityNameDict == null)
            {
               CityNameDict = new Dictionary<string, City>();
                CityFile cityFile;
                using (StreamReader file = File.OpenText("cities.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    cityFile = (CityFile)serializer.Deserialize(file, typeof(CityFile));
                }

                foreach (City city in cityFile.cities)
                {
                    CityNameDict.Add(city.name + ", " + city.country + ", " + city.id, city);
                }

            }

            return CityNameDict;

        }

        public static List<City> SearchCities(string cityName)
        {
            return CityNameDict.Where(q => q.Key.ToLower().Contains(cityName.ToLower())).Select(q => q.Value).OrderBy(o => o.name).ToList<City>();
        }
    }
}
