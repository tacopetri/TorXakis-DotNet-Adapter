using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TorXakisDotNetAdapter.Refinement;

namespace TorXakisDotNetAdapter.Models
{
    /// <summary>TorXakis: MoveAbort</summary>
	public sealed class MoveAbort : ModelAction
	{
        /// <summary>TorXakis: moveAbortId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: MoveAborted</summary>
	public sealed class MoveAborted : ModelAction
	{
        /// <summary>TorXakis: moveAbortedId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: MoveComplete</summary>
	public sealed class MoveComplete : ModelAction
	{
        /// <summary>TorXakis: moveCompleteId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: MoveCompleted</summary>
	public sealed class MoveCompleted : ModelAction
	{
        /// <summary>TorXakis: moveCompletedId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: MoveCreate</summary>
	public sealed class MoveCreate : ModelAction
	{
        /// <summary>TorXakis: moveCreateId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: MoveCreated</summary>
	public sealed class MoveCreated : ModelAction
	{
        /// <summary>TorXakis: moveCreatedId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: MoveDequeue</summary>
	public sealed class MoveDequeue : ModelAction
	{
	}

    /// <summary>TorXakis: MovePause</summary>
	public sealed class MovePause : ModelAction
	{
        /// <summary>TorXakis: movePauseId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: MovePaused</summary>
	public sealed class MovePaused : ModelAction
	{
        /// <summary>TorXakis: movePausedId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: MoveResume</summary>
	public sealed class MoveResume : ModelAction
	{
        /// <summary>TorXakis: moveResumeId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: MoveResumed</summary>
	public sealed class MoveResumed : ModelAction
	{
        /// <summary>TorXakis: moveResumedId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: MoveStart</summary>
	public sealed class MoveStart : ModelAction
	{
        /// <summary>TorXakis: moveStartId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: MoveStarted</summary>
	public sealed class MoveStarted : ModelAction
	{
        /// <summary>TorXakis: moveStartedId</summary>
        public int Id { get; set; }
	}

}
