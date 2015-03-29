﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using Grabacr07.KanColleViewer.Composition;

namespace Logger
{
    [Export(typeof(IToolPlugin))]
    [ExportMetadata("Title", "CodeA")]
    [ExportMetadata("Description", "")]
    [ExportMetadata("Version", "1.0")]
    [ExportMetadata("Author", "Mystic Monkey")]
    public class Base : IToolPlugin
    {
        private readonly Counter counter = new Counter(Grabacr07.KanColleWrapper.KanColleClient.Current.Proxy);

        public string ToolName
        {
            get { return "Logger"; }
        }

        public object GetToolView()
        {
            return new Panel() { Counter = counter };
        }

        public object GetSettingsView()
        {
            return null;
        }
    }
}
