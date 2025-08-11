using System.Collections.Generic;
using System.Linq;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.NpcTools;
using TMPro;
using UnityEngine;
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
        private DialogObjectSettings _exitButtonSettings = null;
        private List<DialogObjectSettings> _dialogTextObjects;
        private DialogNode _currentDialogNode;
        private DialogNode _startDialogNode;
        private DialogGraphAsset _currentDialogGraphAsset;

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

            SpawnExitButton();

            // Subscribe to FSM events
            _dialogFsm.OnSendActorText += DisplayDialogText;
            _dialogFsm.OnSendDialogNodes += DisplayDialogOptions;

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

        /// <summary>
        /// Displays a single dialog text line (NPC or player) on the UI.
        /// </summary>
        /// <param name="text">The text to display.</param>
        private void DisplayDialogText(string text)
        {
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
        private void SpawnExitButton()
        {
            var instance = Instantiate(dialogTextPrefab, dialogTextParent);
            _exitButtonSettings = new DialogObjectSettings(
                instance,
                instance.GetComponent<TextMeshProUGUI>(),
                instance.GetComponent<Button>());

            _exitButtonSettings.Button.onClick.AddListener(ExitFromDialogMenu);
            _exitButtonSettings.TextMeshProUGUI.text = "Exit";
            _exitButtonSettings.Prefab.SetActive(true);
        }

        /// <summary>
        /// Displays a list of dialog options (player choices) on the UI.
        /// </summary>
        /// <param name="dialogOptions">List of dialog nodes representing player choices.</param>
        private void DisplayDialogOptions(List<DialogNode> dialogOptions)
        {
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

            if (_exitButtonSettings?.Prefab != null)
                Destroy(_exitButtonSettings.Prefab);
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
}
