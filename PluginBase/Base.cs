using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using Grabacr07.KanColleViewer.Composition;

namespace Logger
{
    [Export(typeof(IToolPlugin))]
    [ExportMetadata("Title", "Logger")]
    [ExportMetadata("Description", "记录器")]
    [ExportMetadata("Version", "1.0")]
    [ExportMetadata("Author", "Mystic Monkey")]
    public class Base : IToolPlugin
    {
        private ViewModel counter = new ViewModel(Grabacr07.KanColleWrapper.KanColleClient.Current.Proxy);

        public string ToolName
        {
            get { return "Logger"; }
        }

        public object GetToolView()
        {
            return new Panel() { DataContext = counter };
        }

        public object GetSettingsView()
        {
            return null;
        }
    }
}
