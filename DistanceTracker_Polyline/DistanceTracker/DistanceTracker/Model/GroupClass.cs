using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistanceTracker.Model
{
    public class GroupClass
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Group { get; set; }
    }
}
