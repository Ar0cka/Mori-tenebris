using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.TreeView
{
    public class DialogGraphEditor : EditorWindow
    {
        private DialogGraphView _dialogGraphView;

        [MenuItem("CustomTools/Graph/Dialog Editor")]
        public static void Open()
        {
            var window = GetWindow<DialogGraphEditor>();
            window.titleContent = new GUIContent("Dialog graph");
        }

        private void OnEnable()
        {
            ConstructGraph();
            GenerateToolGraph();
        }

        private void ConstructGraph()
        {
            _dialogGraphView = new DialogGraphView
            {
                name = "Dialog graph",
            };
            
            _dialogGraphView.StretchToParentSize();
            rootVisualElement.Add(_dialogGraphView);
        }

        private void GenerateToolGraph()
        {
            var toolbar = new UnityEditor.UIElements.Toolbar();
            
            var nodeCreateButton = new Button(() => 
            {
                _dialogGraphView.CreateNewDialogGraph();
            });
            nodeCreateButton.text = "Create new dialog graph";
            toolbar.Add(nodeCreateButton);
            rootVisualElement.Add(_dialogGraphView);
        }
    }

    public class DialogGraphView : GraphView
    {
        public void CreateNewDialogGraph()
        {
            
        }
    }
}