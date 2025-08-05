using System;
using System.Collections.Generic;
using System.Linq;
using Actors.NPC.DialogSystem.DataScripts;
using TMPro;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Actors.NPC.DialogSystem.TestUI
{
    public class TestDialogUI : MonoBehaviour
    {
        [SerializeField] private DialogNodeScrObj dialogNodeScrObj;
        [SerializeField] private int maxCountDialogText;
        [SerializeField] private GameObject dialogPanel;
        [SerializeField] private GameObject textDialogPrefab;
        [SerializeField] private Transform textDialogParent;
        
        private DialogFSM _dialogFSM;

        private DialogObjectSettings _exitButtonSettings = null;
        
        private List<DialogObjectSettings> _dialogTextObjList;
        private DialogNode _currentDialogNode;

        public void Initialize(DialogFSM dialogFsm) //Добавить в Bootstrap
        {
            if (!ValidComponents() || dialogFsm == null)
            {
                Debug.Log("Error initialize dialog ui system");
                enabled = false;
                return;
            }
            
            _dialogFSM = dialogFsm;
            
            _dialogTextObjList = new List<DialogObjectSettings>();
            
            for (int i = 0; i < maxCountDialogText; i++)
            {
                var item = Instantiate(textDialogPrefab, textDialogParent);
                
                DialogObjectSettings dialogTestObj =
                    new DialogObjectSettings(item, item.GetComponent<TextMeshProUGUI>(), item.GetComponent<Button>());
                
                item.SetActive(false);
                
                _dialogTextObjList.Add(dialogTestObj);
            }
            
            SpawnExitButton();
            
            _dialogFSM.OnSendActorText += TakeCurrentDialogText;
            _dialogFSM.OnSendDialogNodes += NextDialogList;

            //_currentDialogNode = dialogNodeScrObj.GetCurrentDialogNode();
        }
        public void StartDialog()
        {
            if (!dialogPanel.activeInHierarchy)
                dialogPanel.SetActive(true);
            
            _dialogFSM.OnStartDialog?.Invoke(_currentDialogNode);
        }
        public void ExitFromDialogMenu()
        {
            OffAllDialogText();
            dialogPanel.SetActive(false);
            _dialogFSM.OnExitFromDialog?.Invoke();
            //_currentDialogNode = dialogNodeScrObj.GetCurrentDialogNode();
        }
        private void TakeCurrentDialogText(string text)
        {
            OffAllDialogText();
            
            DialogObjectSettings dialogObjectSettings =
                _dialogTextObjList.First(x => x.Prefab.activeInHierarchy == false);

            dialogObjectSettings.Prefab.SetActive(true);
            dialogObjectSettings.TextMeshProUGUI.text = text;
        }

        private void SpawnExitButton()
        {
            var item = Instantiate(textDialogPrefab, textDialogParent);
            _exitButtonSettings = new DialogObjectSettings(item, item.GetComponent<TextMeshProUGUI>(), item.GetComponent<Button>());
            _exitButtonSettings.Button.onClick.AddListener(ExitFromDialogMenu);
            _exitButtonSettings.TextMeshProUGUI.text = "Exit";
            _exitButtonSettings.Prefab.SetActive(true);
        }
        
        private void NextDialogList(List<DialogNode> dialogList)
        {
            OffAllDialogText();

            if (dialogList.Count <= 0) return;

            for (int i = 0; i < dialogList.Count; i++)
            {
                DialogObjectSettings dialogTextObj = _dialogTextObjList[i];
                DialogNode currentNode = dialogList[i];
                
                dialogTextObj.Prefab.SetActive(true);
                dialogTextObj.TextMeshProUGUI.text = currentNode.PlayerDialogData.text;

                Button currentButton = dialogTextObj.Button;
                
                currentButton.onClick.RemoveAllListeners();
                currentButton.onClick.AddListener(() => SetNewDialogNode(currentNode));
            }
        }
        private void SetNewDialogNode(DialogNode dialogNode)
        {
            if (dialogNode == null) return;
            
            _currentDialogNode = dialogNode;
            
            StartDialog();
        }
        private void OffAllDialogText()
        {
            if (_dialogTextObjList.Count == 0) return;
            
            foreach (var item in _dialogTextObjList)
            {
                item.Prefab.SetActive(false);
                item.TextMeshProUGUI.text = "";
            }
        }   
        private bool ValidComponents()
        {
            return dialogPanel != null || textDialogPrefab != null || textDialogParent != null ||
                   dialogNodeScrObj != null;
        }

        private void OnApplicationQuit()
        {
            foreach (var item in _dialogTextObjList)
            {
                Destroy(item.Prefab);
            }
            
            Destroy(_exitButtonSettings.Prefab);
        }
    }

    public class DialogObjectSettings
    {
        public GameObject Prefab;
        public TextMeshProUGUI TextMeshProUGUI;
        public Button Button;

        public DialogObjectSettings(GameObject prefab, TextMeshProUGUI textMeshProUGUI, Button button)
        {
            Prefab = prefab;
            if (textMeshProUGUI != null) TextMeshProUGUI = textMeshProUGUI;
            if (button != null) Button = button;
        }
    }
}