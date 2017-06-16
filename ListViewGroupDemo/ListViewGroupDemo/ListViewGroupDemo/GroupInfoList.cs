using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListViewGroupDemo
{
    public class GroupInfoList : List<object>
    {
        public object Key { get; set; }
    }
}