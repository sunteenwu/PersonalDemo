using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace DistanceTracker.Model
{
    public class SeriesPoint
    {   
        public int Id { get; set; }
        public double Longtitude { get; set; }
        public double Latitude { get; set; }
        public string Ngay { get; set; }
        public string Group { get; set; }
    }
}
