using System;
using System.Collections.Generic;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.DialogSystem.DialogStates;
using Actors.NPC.NpcStateSystem;
using UnityEngine;

namespace Actors.NPC.DialogSystem
{
    /// <summary>
    /// Finite State Machine (FSM) responsible for managing dialog states and transitions.
    /// Controls dialog flow, triggers events for UI interaction and dialog processing.
    /// </summary>
    public class DialogFSM : IDisposable
    {
        // Currently active dialog state
        private DialogState _activeState;

        // Mapping from state type to DialogState instance for quick state switching
        private readonly Dictionary<Type, DialogState> _stateMap = new();

        #region Events

        /// <summary>
        /// Event triggered to send text from an actor (NPC or player) to the UI or other listeners.
        /// </summary>
        public Action<string> OnSendActorText;

        /// <summary>
        /// Event triggered to provide a list of dialog nodes representing the next possible dialog options.
        /// </summary>
        public Action<List<DialogNode>> OnSendDialogNodes;

        /// <summary>
        /// Event triggered to signal the start of dialog with a specific dialog node.
        /// </summary>
        public Action<DialogNode> OnStartDialog;

        /// <summary>
        /// Event triggered when exiting the dialog system, allowing listeners to respond accordingly.
        /// </summary>
        public Action OnExitFromDialog;

        /// <summary>
        /// Event triggered to open a special UI panel, for example, an inventory or quest panel.
        /// </summary>
        public Action<SpecialPanelType> OnOpenSpecialPanel;

        /// <summary>
        /// Event triggered to close the currently open UI panel.
        /// </summary>
        public Action OnClosePanel;

        /// <summary>
        /// Event triggered on user click interactions within the dialog UI.
        /// </summary>
        public Action OnClick;

        #endregion

        // Flag to ensure FSM enters idle state only once until explicitly reset
        private bool _hasEnteredIdleState = false;

        /// <summary>
        /// Constructor subscribes to OnExitFromDialog event to automatically transition to idle state.
        /// </summary>
        public DialogFSM()
        {
            OnExitFromDialog += EnterToIdleState;
        }

        /// <summary>
        /// Transitions FSM into the IdleDialogState if it has not already entered it.
        /// Prevents redundant re-entries into idle.
        /// </summary>
        public void EnterToIdleState()
        {
            if (_hasEnteredIdleState) return;

            _hasEnteredIdleState = true;

            ChangeState<IdleDialogState>();
        }

        /// <summary>
        /// Adds a new dialog state instance to the FSM's internal state map.
        /// Ignores null values and logs a warning.
        /// </summary>
        /// <param name="dialogState">The dialog state instance to add.</param>
        public void AddState(DialogState dialogState)
        {
            if (dialogState == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("DialogFSM.AddState called with null dialogState.");
#endif
                return;
            }

            _stateMap.TryAdd(dialogState.GetType(), dialogState);
        }

        /// <summary>
        /// Switches the current state of the FSM to the specified state type.
        /// Prevents switching if already in the target state.
        /// If the state type is not registered, logs a warning.
        /// </summary>
        /// <typeparam name="T">The target DialogState type.</typeparam>
        /// <param name="dialogNode">Optional DialogNode passed to the new state's Enter method.</param>
        public void ChangeState<T>(DialogNode dialogNode = null) where T : DialogState
        {
            var targetType = typeof(T);

            if (_activeState?.GetType() == targetType) return;

            if (_stateMap.TryGetValue(targetType, out var newState))
            {
                _activeState?.Exit();
                _activeState = newState;
                _activeState.Enter(dialogNode);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"DialogFSM: State {targetType.Name} not found in state map.");
#endif
            }
        }

        /// <summary>
        /// Updates the currently active state. Should be called regularly (e.g., each frame) to drive state logic.
        /// </summary>
        public void Update()
        {
            _activeState?.Update();
        }

        /// <summary>
        /// Resets the idle state flag to allow re-entering the idle state in the future.
        /// </summary>
        public void ExitIdleState()
        {
            _hasEnteredIdleState = false;
        }

        /// <summary>
        /// Unsubscribes from events and clears event handlers to prevent memory leaks.
        /// Call when disposing the FSM.
        /// </summary>
        public void Dispose()
        {
            OnExitFromDialog -= EnterToIdleState;

            // Clearing event handlers to avoid potential memory leaks
            OnSendActorText = null;
            OnSendDialogNodes = null;
            OnStartDialog = null;
            OnExitFromDialog = null;
            OnOpenSpecialPanel = null;
            OnClosePanel = null;
            OnClick = null;
        }
    }
}
