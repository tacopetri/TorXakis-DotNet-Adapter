using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TorXakisDotNetAdapter.Refinement;

namespace TorXakisDotNetAdapter.Models
{
    /// <summary>Generated from TorXakis model.</summary>
	public sealed class CreateItemBegin : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int createItemBeginId { get; set; }
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class CreateItemEnd : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int createItemEndId { get; set; }
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class CreatePathBegin : ModelAction
	{
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class CreatePathEnd : ModelAction
	{
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class DeleteBegin : ModelAction
	{
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class DeleteEnd : ModelAction
	{
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class NewItem : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int newItemId { get; set; }
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class HideItem : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int hideItemId { get; set; }
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class DisableItem : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int disableItemId { get; set; }
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class LockItem : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int lockItemId { get; set; }
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class ModelSwitchItem : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int modelSwitchItemId { get; set; }
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class AssembledItem : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int assembledItemId { get; set; }
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class ConnectItem : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int connectItemId1 { get; set; }
        /// <summary>Generated from TorXakis model.</summary>
        public int connectItemId2 { get; set; }
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class ConnectedItem : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int connectedItemId1 { get; set; }
        /// <summary>Generated from TorXakis model.</summary>
        public int connectedItemId2 { get; set; }
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class CreatePathFromNodes : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int createPathFromNodesId1 { get; set; }
        /// <summary>Generated from TorXakis model.</summary>
        public int createPathFromNodesId2 { get; set; }
        /// <summary>Generated from TorXakis model.</summary>
        public int createPathFromNodesId3 { get; set; }
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class DeleteItem : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int deleteItemId { get; set; }
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class DeletePath : ModelAction
	{
	}

}
