using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorXakis.DotNet
{
    /// <summary>
    /// Actions are categorized into inputs and outputs.
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// An input action, from the perspective of the SUT.
        /// Going from the tester to the SUT.
        /// </summary>
        Input,

        /// <summary>
        /// An output action, from the perspective of the SUT.
        /// Going from the SUT to the tester.
        /// </summary>
        Output,
    }
}
