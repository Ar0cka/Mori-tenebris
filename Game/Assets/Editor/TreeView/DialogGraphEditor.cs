using System;
using System.Collections.Generic;
using Actors.NPC.DialogSystem.DataScripts;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.TreeView
{
    public class DialogGraphEditor : EditorWindow
    {
        private DialogNodeGraph _dialogNodeGraph;
        private DialogNodeScrObj _asset;

        [MenuItem("CustomTools/Graph/Dialog Editor")]
        public static void Open()
        {
            var window = GetWindow<DialogGraphEditor>();
            window.titleContent = new GUIContent("Dialog graph");
        }

        private void OnEnable()
        {
            rootVisualElement.Clear();
            ConstructGraph();
            GenerateToolGraph();
        }
        
        private void ConstructGraph()
        {
            _dialogNodeGraph = new DialogNodeGraph
            {
                name = "Dialog graph",
            };
            
            _dialogNodeGraph.StretchToParentSize();
            rootVisualElement.Add(_dialogNodeGraph);
        }

        private void GenerateToolGraph()
        {
            var toolbar = new UnityEditor.UIElements.Toolbar();
            
            rootVisualElement.Add(toolbar); 
            
            var nodeCreateButton = new Button(() =>
            {
                var node = new DialogNodeForGraph
                {
                    guid = Guid.NewGuid().ToString(),
                    npcData = new DialogData(),
                    playerData = new DialogData(),
                    condition = null,
                    panelSettings = null,
                    position = new Vector2(100, 100),
                    childrenGuids = new List<string>()
                };
                
                _dialogNodeGraph.CreateNewDialogGraph(node);
            });
            nodeCreateButton.text = "Create new dialog graph";
            
            var saveButton = new Button(() => _dialogNodeGraph.SaveGraph(_asset));
            saveButton.text = "Save";
            
            var loadButton = new Button(() => _dialogNodeGraph.LoadGraph(_asset));
            loadButton.text = "Load";

            var assetField = new ObjectField("Graph Asset")
            {
                objectType = typeof(DialogNodeScrObj),
            };
            assetField.RegisterValueChangedCallback(evt =>
            {
                _asset = evt.newValue as DialogNodeScrObj;
            });
            
            toolbar.Add(nodeCreateButton);
            toolbar.Add(saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(assetField);
        }
    }
}