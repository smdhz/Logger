using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using Grabacr07.KanColleViewer.Composition;

namespace Logger
{
    [Export(typeof(IPlugin))]
    [Export(typeof(ITool))]
    [ExportMetadata("Guid", "7f41e3a9-e203-4fef-9eaf-8e1f30873b97")]
    [ExportMetadata("Title", "Logger")]
    [ExportMetadata("Description", "记录器")]
    [ExportMetadata("Version", "1.0")]
    [ExportMetadata("Author", "Mystic Monkey")]
    public class Base : IPlugin, ITool
    {
        private ViewModel counter;

        public string Name => "Logger";

        public object View => new Panel() { DataContext = counter };

        public void Initialize() { counter = new ViewModel(Grabacr07.KanColleWrapper.KanColleClient.Current.Proxy); }
    }
}
