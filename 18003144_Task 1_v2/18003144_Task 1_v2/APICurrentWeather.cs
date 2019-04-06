public class APICurrentWeather
{
    public Coord coord { get; set; }
    public Weather[] weather { get; set; }
    public string _base { get; set; }
    public Main main { get; set; }
    public int visibility { get; set; }
    public Wind wind { get; set; }
    public Rain rain { get; set; }
    public Clouds clouds { get; set; }
    public int dt { get; set; }
    public Sys sys { get; set; }
    public int id { get; set; }
    public string name { get; set; }
    public int cod { get; set; }

    public double GetRain()
    {
        double sum = 0;
        if (rain != null)
        {
            sum += rain._3h + rain._1h;
        }

        return sum;
    }
}

public class Main
{
    public float temp { get; set; }
    public float pressure { get; set; }
    public float humidity { get; set; }
    public float temp_min { get; set; }
    public float temp_max { get; set; }
}

public class Wind
{
    public float speed { get; set; }
    public float deg { get; set; }
    public float gust { get; set; }
}

public class Rain
{
    public float _1h { get; set; }
    public float _3h { get; set; }
}

public class Clouds
{
    public float all { get; set; }
}

public class Sys
{
    public int type { get; set; }
    public int id { get; set; }
    public float message { get; set; }
    public string country { get; set; }
    public int sunrise { get; set; }
    public int sunset { get; set; }
}

public class Weather
{
    public int id { get; set; }
    public string main { get; set; }
    public string description { get; set; }
    public string icon { get; set; }
}
