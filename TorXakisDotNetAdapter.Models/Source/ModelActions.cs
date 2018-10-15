using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TorXakisDotNetAdapter.Models
{
    /// <summary>Generated from TorXakis model.</summary>
	public sealed class CreateItemBegin : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int createItemBeginId;
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class CreateItemEnd : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int createItemEndId;
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
        public int newItemId;
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class HideItem : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int hideItemId;
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class DisableItem : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int disableItemId;
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class LockItem : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int lockItemId;
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class ModelSwitchItem : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int modelSwitchItemId;
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class AssembledItem : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int assembledItemId;
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class ConnectItem : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int connectItemId1;
        /// <summary>Generated from TorXakis model.</summary>
        public int connectItemId2;
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class ConnectedItem : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int connectedItemId1;
        /// <summary>Generated from TorXakis model.</summary>
        public int connectedItemId2;
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class CreatePathFromNodes : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int createPathFromNodesId1;
        /// <summary>Generated from TorXakis model.</summary>
        public int createPathFromNodesId2;
        /// <summary>Generated from TorXakis model.</summary>
        public int createPathFromNodesId3;
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class DeleteItem : ModelAction
	{
        /// <summary>Generated from TorXakis model.</summary>
        public int deleteItemId;
	}

    /// <summary>Generated from TorXakis model.</summary>
	public sealed class DeletePath : ModelAction
	{
	}

}
