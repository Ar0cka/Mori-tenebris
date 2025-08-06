using System.Collections.Generic;
using System.Linq;
using Actors.NPC.DialogSystem.DataScripts;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;

namespace Editor.TreeView
{
     public class DialogNodeGraph : GraphView
    {
        public DialogNodeGraph()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            Insert(0, new GridBackground());
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new EdgeConnector<Edge>(new MyEdgeCon(this)));

            graphViewChanged += OnGraphViewChange;
        }

        private GraphViewChange OnGraphViewChange(GraphViewChange change)
        {
            if (change.edgesToCreate != null)
            {
                foreach (var edge in change.edgesToCreate)
                {
                    var fromNode = edge.output.node as DialogNodeView;
                    var toNode = edge.input.node as DialogNodeView;

                    if (fromNode != null && toNode != null)
                    {
                        if (!fromNode.dialogNodeData.childrenGuids.Contains(toNode.dialogNodeData.guid))
                        {
                            fromNode.dialogNodeData.childrenGuids.Add(toNode.dialogNodeData.guid);
                        }
                    }
                }
            }
            
            return change;
        }
        
        public void CreateNewDialogGraph(DialogNodeForGraph nodeData)
        {
            var node = new DialogNodeView(nodeData);
            AddElement(node);
        }
        
        public void SaveGraph(DialogNodeScrObj asset)
        {
            asset.dialogNode.Clear();

            foreach (var node in nodes.OfType<DialogNodeView>())
            {
                var data = node.GetData();
                
                var connections = edges.Where(edge => edge.output.node == node);
                data.childrenGuids = connections
                    .Select(edge => ((DialogNodeView)edge.input.node).guid)
                    .ToList();

                asset.dialogNode.Add(data);
            }

            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
        }
        
        public void LoadGraph(DialogNodeScrObj asset)
        {
            DeleteElements(graphElements.ToList());
            
            var nodeLookup = new Dictionary<string, DialogNodeView>();

            foreach (var data in asset.dialogNode)
            {
                var node = new DialogNodeView(data);
                AddElement(node);
                nodeLookup[data.guid] = node;
            }
            
            foreach (var data in asset.dialogNode)
            {
                if (!nodeLookup.TryGetValue(data.guid, out var fromNode)) continue;

                foreach (var childGuid in data.childrenGuids)
                {
                    if (!nodeLookup.TryGetValue(childGuid, out var toNode)) continue;

                    var output = fromNode.outputContainer.Q<Port>();
                    var input = toNode.inputContainer.Q<Port>();

                    var edge = new Edge
                    {
                        output = output,
                        input = input
                    };

                    edge.output.Connect(edge);
                    edge.input.Connect(edge);
                    AddElement(edge);
                }
            }
        }
        
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList();
        }
    }

    public class MyEdgeCon : IEdgeConnectorListener
    {
        private GraphView _graphView;

        public MyEdgeCon(GraphView graphView)
        {
            _graphView = graphView;
        }
        
        public void OnDrop(GraphView graphView, Edge edge)
        {
            if (edge.input != null && edge.output != null)
            {
                edge.input.Connect(edge);
                edge.output.Connect(edge);
                graphView.AddElement(edge);
            }
            
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
            throw new System.NotImplementedException();
        }
    }
}