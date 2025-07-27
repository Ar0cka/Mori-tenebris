using System;
using System.Collections.Generic;
using System.Linq;
using Actors.NPC.DialogSystem.DataScripts;
using TMPro;
using UI;
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
        
        [SerializeField] private DialogFsmRealize dialogFsmRealize;
        private DialogFSM _dialogFSM;
        
        private List<DialogObjectSettings> _dialogTextObjList;
        private DialogNode _currentDialogNode;
        
        private bool _isInitialize;

        public void Initialize() //Добавить в Bootstrap
        {
            _dialogFSM = dialogFsmRealize.GetDialogFsm();
            _dialogTextObjList = new List<DialogObjectSettings>();
            
            for (int i = 0; i < maxCountDialogText; i++)
            {
                var item = Instantiate(textDialogPrefab, textDialogParent);
                
                DialogObjectSettings dialogTestObj =
                    new DialogObjectSettings(item, item.GetComponent<TextMeshProUGUI>(), item.GetComponent<Button>());
                
                item.SetActive(false);
                
                _dialogTextObjList.Add(dialogTestObj);
            }
            
            _dialogFSM.OnSendActorText += TakeCurrentDialogText;
            _dialogFSM.OnSendDialogNodes += NextDialogList;

            _currentDialogNode = dialogNodeScrObj.GetCurrentDialogNode();

            _isInitialize = true;
        }
        public void StartDialog()
        {
            if (!_isInitialize)
                Initialize();
            
            if (!dialogPanel.activeInHierarchy)
                dialogPanel.SetActive(true);
            
            _dialogFSM.OnStartDialog?.Invoke(_currentDialogNode);
        }
        private void TakeCurrentDialogText(string text)
        {
            OffAllDialogText();
            
            DialogObjectSettings dialogObjectSettings =
                _dialogTextObjList.First(x => x.Prefab.activeInHierarchy == false);

            dialogObjectSettings.Prefab.SetActive(true);
            dialogObjectSettings.TextMeshProUGUI.text = text;
        }
        private void NextDialogList(List<DialogNode> dialogList)
        {
            OffAllDialogText();

            for (int i = 0; i < dialogList.Count; i++)
            {
                DialogObjectSettings dialogTextObj = _dialogTextObjList[i];
                DialogNode currentNode = dialogList[i];
                
                dialogTextObj.Prefab.SetActive(true);
                dialogTextObj.TextMeshProUGUI.text = currentNode.playerDialogData.text;

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
            foreach (var item in _dialogTextObjList)
            {
                item.Prefab.SetActive(false);
                item.TextMeshProUGUI.text = "";
            }
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