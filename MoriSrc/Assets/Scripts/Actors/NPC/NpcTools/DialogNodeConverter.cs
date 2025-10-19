using System.Collections.Generic;
using System.Linq;
using Actors.NPC.DialogSystem.DataScripts;

namespace Actors.NPC.NpcTools
{
    /// <summary>
/// Static helper class to convert data from ScriptableObject into dialog node structures.
/// </summary>
public static class DialogNodeConverter
{
    /// <summary>
    /// Converts dialog nodes from a ScriptableObject asset into a list of starting DialogNodes.
    /// </summary>
    /// <param name="asset">The ScriptableObject containing dialog node data.</param>
    /// <returns>List of root DialogNodes that start the dialog flow.</returns>
    public static List<DialogNode> ConvertFromAsset(DialogGraphAsset asset)
    {
        // Dictionary for quick lookup of graph nodes by their GUID
        Dictionary<string, SerializedDialogNode> graphNodesByGuid = new();

        // Populate the dictionary with nodes from the asset
        foreach (var node in asset.dialogNode)
        {
            graphNodesByGuid.TryAdd(node.id, node);
        }

        // Dictionary to store converted DialogNodes by GUID
        Dictionary<string, DialogNode> dialogNodes = new();

        // Convert all graph nodes to DialogNodes
        foreach (var kvp in graphNodesByGuid)
        {
            dialogNodes[kvp.Key] = ConvertToDialogNode(kvp.Value);
        }

        // Establish parent-child relationships between DialogNodes
        foreach (var kvp in graphNodesByGuid)
        {
            var parentGuid = kvp.Key;
            var childrenGuids = kvp.Value.childrenGuids;

            if (childrenGuids != null && dialogNodes.TryGetValue(parentGuid, out var parentNode))
            {
                foreach (var childGuid in childrenGuids)
                {
                    if (dialogNodes.TryGetValue(childGuid, out var childNode))
                    {
                        parentNode.AddNewChild(childNode);
                    }
                    else
                    {
                        // Log warning if a child node GUID is missing
                        UnityEngine.Debug.LogWarning($"Child node with GUID {childGuid} not found while linking children.");
                    }
                }
            }
        }
        
        // Return only the root nodes (marked as start nodes)
        return dialogNodes.Values.Where(node => node.IsStart).ToList();
    }
    
    private static DialogNode ConvertToDialogNode(SerializedDialogNode graphNode)
    {
        var dialogNode = new DialogNode(
            graphNode.playerDialog,
            graphNode.npcDialog,
            graphNode.condition,
            graphNode.panelSettings);

        if (graphNode.isStartNode)
        {
            dialogNode.MakAtStartNode();
        }

        return dialogNode;
    }
}
}

