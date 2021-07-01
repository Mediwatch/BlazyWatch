﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCalendar.Data
{
    public class CalendarResourceFeed
    {
        public string url { get; set; }
        public string method { get; set; }
        public Dictionary<string, string> extraParams { get; set; }
    }
}
