using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.NpcTools;
using ConsoleApp.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Actors.NPC.DialogSystem.TestUI
{
    /// <summary>
    /// Test UI controller for dialog system.
    /// Manages dialog text display, player choices, and dialog flow control.
    /// </summary>
    public class TestDialogUI : MonoBehaviour
    {
        [SerializeField] private int maxDialogTextCount = 5;
        [SerializeField] private GameObject dialogPanel;
        [SerializeField] private GameObject dialogTextPrefab;
        [SerializeField] private Transform dialogTextParent;

        private DialogFSM _dialogFsm;
        private List<DialogObjectSettings> _dialogTextObjects;
        private DialogNode _currentDialogNode;
        private DialogNode _startDialogNode;

        private Stack<DialogNode> _historyNodes = new Stack<DialogNode>();
        private Dictionary<string, OptionalButtons> _optionalButtonsMap = new();
        
        private DialogGraphAsset _currentDialogGraphAsset;
        private bool _displayDialogNode = false;

        /// <summary>
        /// Initializes the dialog UI with the dialog FSM and the starting dialog graph asset.
        /// Should be called once during bootstrap or setup.
        /// </summary>
        /// <param name="dialogFsm">Dialog finite state machine instance.</param>
        /// <param name="startDialogConfig">Starting dialog graph asset.</param>
        public void Initialize(DialogFSM dialogFsm, DialogGraphAsset startDialogConfig)
        {
            if (!AreComponentsValid() || dialogFsm == null)
            {
                Debug.LogError("Dialog UI initialization failed: Missing components or null FSM.");
                enabled = false;
                return;
            }

            _dialogFsm = dialogFsm;

            dialogPanel.SetActive(false);
            
            // Prepare pool of dialog text UI objects
            _dialogTextObjects = new List<DialogObjectSettings>();

            for (int i = 0; i < maxDialogTextCount; i++)
            {
                var instance = Instantiate(dialogTextPrefab, dialogTextParent);

                var dialogObject = new DialogObjectSettings(
                    instance,
                    instance.GetComponent<TextMeshProUGUI>(),
                    instance.GetComponent<Button>());

                instance.SetActive(false);
                _dialogTextObjects.Add(dialogObject);
            }
            
            SpawnDopButtons();

            // Subscribe to FSM events
            _dialogFsm.OnSendActorText += DisplayDialogText;
            _dialogFsm.OnSendDialogNodes += DisplayDialogOptions;
            _dialogFsm.OnLastDialogNode += ReturnToLastDialogNode;

            _currentDialogGraphAsset = startDialogConfig;

            // Convert dialog graph asset to runtime dialog nodes and pick the first node to start
            _startDialogNode = DialogNodeConverter.ConvertFromAsset(_currentDialogGraphAsset).First();
            _currentDialogNode = _startDialogNode;
        }
        
        /// <summary>
        /// Starts the dialog by activating the dialog panel and invoking the FSM start event.
        /// </summary>
        public void StartDialog()
        {
            if (!dialogPanel.activeInHierarchy)
                dialogPanel.SetActive(true);

            Debug.Log("Current player dialog node = " + _currentDialogNode.PlayerDialogData.text
            + $"Current child count = {_currentDialogNode.GetNextNodes()?.Count}");
            
            _dialogFsm.OnStartDialog?.Invoke(_currentDialogNode);
        }

        /// <summary>
        /// Handles exiting the dialog menu, clears UI and notifies FSM.
        /// </summary>
        public void ExitFromDialogMenu()
        {
            ClearAllDialogText();
            dialogPanel.SetActive(false);
            _dialogFsm.OnExitFromDialog?.Invoke();
            _currentDialogNode = _startDialogNode;
        }

        public void ReturnToMenu()
        {
            _currentDialogNode = _startDialogNode;
            _dialogFsm.OnStartDialog?.Invoke(_currentDialogNode);
        }

        public void ReturnToLastDialogNode()
        {
            _currentDialogNode = _historyNodes.Pop();
            DisplayDialogOptions(_currentDialogNode.GetNextNodes());
        }
        
        /// <summary>
        /// Displays a single dialog text line (NPC or player) on the UI.
        /// </summary>
        /// <param name="text">The text to display.</param>
        private void DisplayDialogText(string text)
        {
            _displayDialogNode = true;
            
            ClearAllDialogText();

            // Find the first inactive dialog text UI object
            var dialogObject = _dialogTextObjects.FirstOrDefault(x => !x.Prefab.activeInHierarchy);
            if (dialogObject == null)
            {
                Debug.LogWarning("No available dialog text UI objects to display text.");
                return;
            }
            
            dialogObject.Button.interactable = false;
            dialogObject.Prefab.SetActive(true);
            dialogObject.TextMeshProUGUI.text = text;
        }

        /// <summary>
        /// Spawns and configures the exit button for the dialog UI.
        /// </summary>
        private void SpawnDopButtons()
        {
            var optionalButtons = new Dictionary<string, UnityAction>
            {
                {"Last node", ReturnToLastDialogNode},
                {"Exit", ExitFromDialogMenu}
            };
            
            foreach (var item in optionalButtons)
            {
                var instance = Instantiate(dialogTextPrefab, dialogTextParent);
                
                instance.gameObject.name = item.Key;
                
                var dialogButton = new DialogObjectSettings(
                    instance,
                    instance.GetComponent<TextMeshProUGUI>(),
                    instance.GetComponent<Button>());

                dialogButton.Button.onClick.AddListener(item.Value);
                dialogButton.TextMeshProUGUI.text = item.Key;
                dialogButton.Prefab.SetActive(true);

                Func<bool> isVisable = item.Key == "Last node" 
                    ? new Func<bool>(() => _historyNodes.Count > 0 && !_displayDialogNode) 
                    : new Func<bool>(() => !_displayDialogNode);
                
                _optionalButtonsMap[item.Key] = new OptionalButtons(dialogButton, isVisable);
            }
        }

        /// <summary>
        /// Displays a list of dialog options (player choices) on the UI.
        /// </summary>
        /// <param name="dialogOptions">List of dialog nodes representing player choices.</param>
        private void DisplayDialogOptions(List<DialogNode> dialogOptions)
        {
            _displayDialogNode = false;
            
            ClearAllDialogText();

            if (dialogOptions == null || dialogOptions.Count == 0)
                return;

            for (int i = 0; i < dialogOptions.Count && i < _dialogTextObjects.Count; i++)
            {
                var dialogObject = _dialogTextObjects[i];
                var dialogNode = dialogOptions[i];

                dialogObject.Prefab.SetActive(true);
                dialogObject.TextMeshProUGUI.text = dialogNode.PlayerDialogData.text;

                var button = dialogObject.Button;

                // Clear existing listeners to avoid stacking
                button.onClick.RemoveAllListeners();

                // Capture local variable for closure
                var capturedNode = dialogNode;
                button.onClick.AddListener(() => SetNewDialogNode(capturedNode));
            }
        }

        /// <summary>
        /// Sets the current dialog node and restarts the dialog UI for it.
        /// </summary>
        /// <param name="dialogNode">The selected dialog node.</param>
        private void SetNewDialogNode(DialogNode dialogNode)
        {
            if (dialogNode == null) return;

            if (dialogNode.Condition.CurrentConditionType != ConditionType.Quest) 
                _historyNodes.Push(_currentDialogNode);
            
            _currentDialogNode = dialogNode;
            StartDialog();
        }

        /// <summary>
        /// Deactivates all dialog text UI elements and clears their text.
        /// </summary>
        private void ClearAllDialogText()
        {
            if (_dialogTextObjects == null || _dialogTextObjects.Count == 0) return;

            foreach (var dialogObject in _dialogTextObjects)
            {
                dialogObject.Prefab.SetActive(false);
                dialogObject.TextMeshProUGUI.text = string.Empty;
                dialogObject.Button.interactable = true;
            }

            foreach (var item in _optionalButtonsMap)
            {
                item.Value.Button.Prefab.SetActive(item.Value.IsVisible());
            }
        }

        /// <summary>
        /// Validates required UI components are assigned.
        /// </summary>
        /// <returns>True if all components are valid; otherwise false.</returns>
        private bool AreComponentsValid()
        {
            return dialogPanel != null && dialogTextPrefab != null && dialogTextParent != null;
        }
        
        /// <summary>
        /// Cleanup instantiated UI objects when application quits.
        /// </summary>
        private void OnApplicationQuit()
        {
            if (_dialogTextObjects != null)
            {
                foreach (var dialogObject in _dialogTextObjects)
                {
                    if (dialogObject.Prefab != null)
                        Destroy(dialogObject.Prefab);
                }
            }
        }
    }

    /// <summary>
    /// Wrapper class to store references to UI components for a single dialog text element.
    /// </summary>
    public class DialogObjectSettings
    {
        public GameObject Prefab { get; }
        public TextMeshProUGUI TextMeshProUGUI { get; }
        public Button Button { get; }

        public DialogObjectSettings(GameObject prefab, TextMeshProUGUI textMeshProUGUI, Button button)
        {
            Prefab = prefab;
            TextMeshProUGUI = textMeshProUGUI;
            Button = button;
        }
    }

    public class OptionalButtons
    {
        public DialogObjectSettings Button { get; private set; }
        public Func<bool> IsVisible { get; private set; }

        public OptionalButtons(DialogObjectSettings button, Func<bool> isVisible)
        {
            Button = button;
            IsVisible = isVisible;
        }
    }
}
