using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TorXakisDotNetAdapter.Refinement;

namespace TorXakisDotNetAdapter.Models
{
    /// <summary>TorXakis: CreateItemBegin</summary>
	public sealed class CreateItemBegin : ModelAction
	{
        /// <summary>TorXakis: createItemBeginId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: CreateItemEnd</summary>
	public sealed class CreateItemEnd : ModelAction
	{
        /// <summary>TorXakis: createItemEndId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: CreatePathBegin</summary>
	public sealed class CreatePathBegin : ModelAction
	{
	}

    /// <summary>TorXakis: CreatePathEnd</summary>
	public sealed class CreatePathEnd : ModelAction
	{
	}

    /// <summary>TorXakis: DeleteBegin</summary>
	public sealed class DeleteBegin : ModelAction
	{
	}

    /// <summary>TorXakis: DeleteEnd</summary>
	public sealed class DeleteEnd : ModelAction
	{
	}

    /// <summary>TorXakis: NewItem</summary>
	public sealed class NewItem : ModelAction
	{
        /// <summary>TorXakis: newItemId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: HideItem</summary>
	public sealed class HideItem : ModelAction
	{
        /// <summary>TorXakis: hideItemId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: DisableItem</summary>
	public sealed class DisableItem : ModelAction
	{
        /// <summary>TorXakis: disableItemId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: LockItem</summary>
	public sealed class LockItem : ModelAction
	{
        /// <summary>TorXakis: lockItemId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: ModelSwitchItem</summary>
	public sealed class ModelSwitchItem : ModelAction
	{
        /// <summary>TorXakis: modelSwitchItemId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: AssembledItem</summary>
	public sealed class AssembledItem : ModelAction
	{
        /// <summary>TorXakis: assembledItemId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: ConnectItem</summary>
	public sealed class ConnectItem : ModelAction
	{
        /// <summary>TorXakis: connectItemId1</summary>
        public int Id1 { get; set; }
        /// <summary>TorXakis: connectItemId2</summary>
        public int Id2 { get; set; }
	}

    /// <summary>TorXakis: ConnectedItem</summary>
	public sealed class ConnectedItem : ModelAction
	{
        /// <summary>TorXakis: connectedItemId1</summary>
        public int Id1 { get; set; }
        /// <summary>TorXakis: connectedItemId2</summary>
        public int Id2 { get; set; }
	}

    /// <summary>TorXakis: CreatePathFromNodes</summary>
	public sealed class CreatePathFromNodes : ModelAction
	{
        /// <summary>TorXakis: createPathFromNodesId1</summary>
        public int Id1 { get; set; }
        /// <summary>TorXakis: createPathFromNodesId2</summary>
        public int Id2 { get; set; }
        /// <summary>TorXakis: createPathFromNodesId3</summary>
        public int Id3 { get; set; }
	}

    /// <summary>TorXakis: DeleteItem</summary>
	public sealed class DeleteItem : ModelAction
	{
        /// <summary>TorXakis: deleteItemId</summary>
        public int Id { get; set; }
	}

    /// <summary>TorXakis: DeletePath</summary>
	public sealed class DeletePath : ModelAction
	{
	}

}
