using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriversList
{
    /// <summary>
    /// POCO object for driver info
    /// </summary>
    public class DriverModel
    {
        public string AcceptPause { get; set; }

        public string AcceptStop { get; set; }

        public string BSS { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public string DisplayName { get; set; }

        public string DriverType { get; set; }

        public string Init { get; set; }

        public string LinkDate { get; set; }
        public string ModuleName { get; set; }

        public string PagedPool { get; set; }

        public string Path { get; set; }

        public string StartMode { get; set; }

        public string State { get; set; }

        public string Status { get; set; }
    }
}