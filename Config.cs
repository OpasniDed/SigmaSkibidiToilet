using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkibidiToilet
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("SKIBIDI TOILET CONFIG")]
        public int Health { get; set; } = 5000;
        public string schematicName { get; set; } = "Toilet";
        public string soundName { get; set; } = "SkibidiToilet.ogg";

    }
}
