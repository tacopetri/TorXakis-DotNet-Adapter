using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TorXakisDotNetAdapter.Refinement
{
    /// <summary>
    /// The top-level class that manages all refinement.
    /// </summary>
    public sealed class RefinementFramework
    {
        #region Definitions

        // TODO: Implement!

        #endregion
        #region Variables & Properties

        /// <summary>
        /// A lock object to make this class thread-safe.
        /// </summary>
        private readonly object locker = new object();

        /// <summary>
        /// The managed <see cref="TorXakisAdapter"/> instance.
        /// </summary>
        public TorXakisAdapter Adapter { get; private set; }

        /// <summary>
        /// The collection of contained <see cref="RefinementSystem"/> systems.
        /// </summary>
        public List<RefinementSystem> Systems { get; private set; } = new List<RefinementSystem>();

        /// <summary>
        /// The current <see cref="RefinementSystem"/> system.
        /// </summary>
        public RefinementSystem CurrentSystem { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public RefinementFramework(FileInfo model)
        {
            Adapter = new TorXakisAdapter(model);
            Adapter.Started += Adapter_Started;
            Adapter.InputReceived += Adapter_InputReceived;
        }

        /// <summary><see cref="Object.ToString"/></summary>
        public override string ToString()
        {
            string result = GetType().Name
                + "\n" + nameof(Adapter) + ": " + Adapter
                + "\n" + nameof(Systems) + " (" + Systems.Count + "): " + string.Join(", ", Systems.Select(x => x.Name).ToArray())
                + "\n" + nameof(CurrentSystem) + ": " + CurrentSystem?.Name;
            return result;
        }

        #endregion
        #region Adapter

        /// <summary><see cref="TorXakisAdapter.Start"/></summary>
        public void Start()
        {
            lock (locker)
            {
                Adapter.Start();
            }
        }

        /// <summary><see cref="TorXakisAdapter.Stop"/></summary>
        public void Stop()
        {
            lock (locker)
            {
                Adapter.Stop();
            }
        }

        /// <summary><see cref="TorXakisAdapter.Started"/></summary>
        public event Action Started;

        /// <summary><see cref="TorXakisAdapter.Started"/></summary>
        private void Adapter_Started()
        {
            lock (locker)
            {
                Started?.Invoke();
            }
        }

        /// <summary><see cref="TorXakisAdapter.InputReceived"/></summary>
        private void Adapter_InputReceived(TorXakisAction action)
        {
            if (action.Type == ActionType.Input && action.Channel == TorXakisModel.InputChannel)
            {
                ModelAction input = ModelAction.Deserialize(action.Data);
                HandleModelInput(input);
            }
        }

        #endregion
        #region Systems

        /// <summary>
        /// Adds the given <see cref="RefinementSystem"/> to <see cref="Systems"/>.
        /// </summary>
        public bool AddSystem(RefinementSystem system)
        {
            lock (locker)
            {
                Systems.Add(system);
                return true;
            }
        }

        /// <summary>
        /// Removes the given <see cref="RefinementSystem"/> from <see cref="Systems"/>.
        /// </summary>
        public bool RemoveSystem(RefinementSystem system)
        {
            lock (locker)
            {
                return Systems.Remove(system);
            }
        }

        /// <summary>
        /// Checks the <see cref="inputs"/> and <see cref="events"/> queues.
        /// Determines if the <see cref="CurrentSystem"/> can be advanced.
        /// Determines if one of the <see cref="Systems"/> can be started.
        /// </summary>
        private void CheckSystems()
        {
            // TODO!
        }

        #endregion
        #region Inputs & Outputs

        /// <summary>
        /// The queue of waiting <see cref="ModelAction"/> inputs.
        /// </summary>
        private readonly Queue<ModelAction> inputs = new Queue<ModelAction>();

        /// <summary>
        /// Handles the given <see cref="ModelAction"/> input.
        /// </summary>
        public void HandleModelInput(ModelAction modelAction)
        {
            lock (locker)
            {
                inputs.Enqueue(modelAction);
                CheckSystems();
            }
        }

        /// <summary>
        /// Sends the given <see cref="ModelAction"/> output.
        /// </summary>
        public void SendModelOutput(ModelAction modelAction)
        {
            lock (locker)
            {
                string serialized = modelAction.Serialize();
                TorXakisAction output = TorXakisAction.FromOutput(TorXakisModel.OutputChannel, serialized);
                Adapter.SendOutput(output);
            }
        }

        /// <summary>
        /// The queue of waiting <see cref="ISystemAction"/> events.
        /// </summary>
        private readonly Queue<ISystemAction> events = new Queue<ISystemAction>();


        /// <summary>
        /// Handles the given <see cref="ISystemAction"/> event.
        /// </summary>
        public void HandleSystemEvent(ISystemAction systemAction)
        {
            lock (locker)
            {
                events.Enqueue(systemAction);
                CheckSystems();
            }
        }

        /// <summary>
        /// Signals that the SUT should execute the given <see cref="ISystemAction"/> command.
        /// </summary>
        public event Action<ISystemAction> ExecuteSystemCommand;

        /// <summary>
        /// Sends the given <see cref="ISystemAction"/> command.
        /// </summary>
        public void SendSystemCommand(ISystemAction systemAction)
        {
            lock (locker)
            {
                ExecuteSystemCommand?.Invoke(systemAction);
            }
        }

        #endregion
    }
}
