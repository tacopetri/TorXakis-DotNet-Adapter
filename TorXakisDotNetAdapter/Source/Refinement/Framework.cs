using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TorXakisDotNetAdapter.Refinement
{
    /// <summary>
    /// The top-level class that manages all action refinement operations.
    /// </summary>
    public sealed class Framework
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
        /// The collection of contained <see cref="TransitionSystem"/> systems.
        /// </summary>
        public List<TransitionSystem> Systems { get; private set; } = new List<TransitionSystem>();

        /// <summary>
        /// The current <see cref="TransitionSystem"/> system.
        /// </summary>
        public TransitionSystem CurrentSystem { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public Framework(FileInfo model)
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
        /// Adds the given <see cref="TransitionSystem"/> to <see cref="Systems"/>.
        /// </summary>
        public bool AddSystem(TransitionSystem system)
        {
            lock (locker)
            {
                Systems.Add(system);
                return true;
            }
        }

        /// <summary>
        /// Removes the given <see cref="TransitionSystem"/> from <see cref="Systems"/>.
        /// </summary>
        public bool RemoveSystem(TransitionSystem system)
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
            bool transitioned = false;

            // If we are currently inside a refinement, only that system may be advanced.
            if (CurrentSystem != null)
            {
                ISystemAction nextEvent = events.Peek();
                // TODO!
            }
            // Otherwise, we check if a new refinement system can be started with the queued system events and/or model inputs.
            else
            {
                // TODO!
            }

            // If a transition was taken, re-evaluate immediately!
            if (transitioned) CheckSystems();
            // If no transition was taken, but there are still inputs or events queued: this indicates a deadlock!
            else if (inputs.Count > 0 || events.Count > 0)
            {
                string error = "No valid transition!";
                Console.WriteLine(error);
                //throw new Exception(error);
            }
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
