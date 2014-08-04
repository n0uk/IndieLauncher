﻿using System;
using System.Collections.Generic;

namespace Dan200.Launcher.RSS
{
    public class RSSChannel
    {
        public string Title;
        public string Description;
        public string Link;
        public readonly IList<RSSEntry> Entries;

        public RSSChannel()
        {
            Entries = new List<RSSEntry>();
        }
    }
}

