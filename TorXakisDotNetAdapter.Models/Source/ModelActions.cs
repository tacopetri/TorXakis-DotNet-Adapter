using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TorXakisDotNetAdapter.Refinement;

namespace TorXakisDotNetAdapter.Models
{
    /// <summary>TorXakis: MoveCreate</summary>
	public sealed class MoveCreate : ModelAction
	{
        /// <summary>TorXakis: moveCreateId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: MoveStart</summary>
	public sealed class MoveStart : ModelAction
	{
        /// <summary>TorXakis: moveStartId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: MovePause</summary>
	public sealed class MovePause : ModelAction
	{
        /// <summary>TorXakis: movePauseId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: MoveResume</summary>
	public sealed class MoveResume : ModelAction
	{
        /// <summary>TorXakis: moveResumeId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: MoveAbort</summary>
	public sealed class MoveAbort : ModelAction
	{
        /// <summary>TorXakis: moveAbortId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: MoveComplete</summary>
	public sealed class MoveComplete : ModelAction
	{
        /// <summary>TorXakis: moveCompleteId</summary>
        public int Id { get; set; }
	}

}
