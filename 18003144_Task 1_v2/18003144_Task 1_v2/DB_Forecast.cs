//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _18003144_Task_1_v2
{
    using System;
    using System.Collections.Generic;
    
    public partial class DB_Forecast
    {
        public int ForecastID { get; set; }
        public int CityID { get; set; }
        public System.DateTime ForecastDate { get; set; }
        public int MinTemp { get; set; }
        public int MaxTemp { get; set; }
        public int WindSpeed { get; set; }
        public int Humidity { get; set; }
        public int Precipitation { get; set; }
    }
}