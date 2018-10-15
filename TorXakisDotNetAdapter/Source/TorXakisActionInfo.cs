using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorXakisDotNetAdapter
{
    /// <summary>
    /// Contains parsed information about <see cref="TorXakisAction"/> definitions in the model.
    /// </summary>
    public class TorXakisActionInfo
    {
        /// <summary>
        /// The name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The set of parameters: string name, plus string type.
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
    }
}
