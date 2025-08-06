using System.Collections.Generic;
using System.Linq;
using Actors.NPC.DialogSystem.DataScripts;

namespace Actors.NPC.NpcTools
{
    public static class ConvertDialogNode
    {
        public static List<DialogNode> GetDialogsNodeFromScrObj(DialogNodeScrObj asset)
        {
            Dictionary<string, DialogNodeForGraph> graphDialogNodes = new();

            foreach (var node in asset.dialogNode)
            {
                graphDialogNodes.TryAdd(node.guid, node);
            }

            Dictionary<string, DialogNode> dialogNodes = new();

            foreach (var node in graphDialogNodes)
            {
                dialogNodes[node.Key] = ConvertToDialogNode(node.Value);
            }

            foreach (var node in graphDialogNodes)
            {
                var parentGuid = node.Value.guid;
                var childGuid = node.Value.childrenGuids;

                if (childGuid != null && dialogNodes.TryGetValue(parentGuid, out var parentNode))
                {
                    foreach (var child in childGuid)
                    {
                        if (dialogNodes.TryGetValue(child, out var childNode))
                        {
                            parentNode.AddNewChild(childNode);
                        }
                    }
                }
            }

            return dialogNodes.Values.Where(x => x.IsStart).ToList();
        }

        private static DialogNode ConvertToDialogNode(DialogNodeForGraph graphNode)
        {
            var dialogNode = new DialogNode(graphNode.playerData, graphNode.npcData, graphNode.condition, graphNode.panelSettings);

            if (graphNode.isStart)
            {
                dialogNode.SetStartNode();
            }
            
            return dialogNode;
        }
    }
}