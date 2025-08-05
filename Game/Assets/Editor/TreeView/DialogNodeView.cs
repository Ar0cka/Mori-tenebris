using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.NpcStateSystem;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.TreeView
{
    public class DialogNodeView : Node
    {
        public string guid;
        public DialogNodeForGraph dialogNodeData;
        
        public Port InputPort { get; private set; }
        public Port OutputPort { get; private set; }

        public DialogNodeView(DialogNodeForGraph dialogNodeData)
        {
            this.dialogNodeData = dialogNodeData;
            guid = dialogNodeData.guid;

            title = dialogNodeData.npcData.text;
            
            BuildNodeUI(dialogNodeData);
            
            SetPosition(new Rect(dialogNodeData.position, new Vector2(200, 150)));
            
            InputPort = CreatePort(Orientation.Horizontal, Direction.Input);
            InputPort.portName = "Add parent";
            inputContainer.Add(InputPort);
            Debug.Log("InputPort created: " + InputPort);

            OutputPort  = CreatePort(Orientation.Horizontal, Direction.Output);
            OutputPort.portName = "Add child";
            outputContainer.Add(OutputPort);
            Debug.Log("OutputPort created: " + OutputPort);
            
            RefreshExpandedState();
            RefreshPorts();
        }

        private void BuildNodeUI(DialogNodeForGraph data)
        {
            // --- NPC Text ---
            var npcTextField = new TextField("NPC Text")
            {
                value = data.npcData?.text ?? ""
            };
            npcTextField.RegisterValueChangedCallback(evt =>
            {
                data.npcData.text = evt.newValue;
            });
            mainContainer.Add(npcTextField);

            // --- Player Text ---
            var playerTextField = new TextField("Player Text")
            {
                value = data.playerData?.text ?? ""
            };
            playerTextField.RegisterValueChangedCallback(evt =>
            {
                data.playerData.text = evt.newValue;
            });
            mainContainer.Add(playerTextField);

            // --- Condition Fields ---
            if (data.condition == null)
                data.condition = new DialogCondition();

            var conditionTypeEnum = new EnumField("Condition Type", data.condition.CurrentConditionType);
            conditionTypeEnum.RegisterValueChangedCallback(evt =>
            {
                data.condition.CurrentConditionType = (ConditionType)evt.newValue;
            });
            mainContainer.Add(conditionTypeEnum);

            var actionTypeEnum = new EnumField("Action Type", data.condition.ActionType);
            actionTypeEnum.RegisterValueChangedCallback(evt =>
            {
                data.condition.ActionType = (DialogActionType)evt.newValue;
            });
            mainContainer.Add(actionTypeEnum);

            var reputationField = new IntegerField("Reputation")
            {
                value = data.condition.ReputationNum
            };
            reputationField.RegisterValueChangedCallback(evt =>
            {
                data.condition.ReputationNum = evt.newValue;
            });
            mainContainer.Add(reputationField);

            // --- Special Panel Fields ---
            if (data.panelSettings == null)
                data.panelSettings = new DialogSpecialPanelSettings();

            var panelTypeEnum = new EnumField("Special Panel", data.panelSettings.specialPanelType);
            panelTypeEnum.RegisterValueChangedCallback(evt =>
            {
                data.panelSettings.specialPanelType = (SpecialPanelType)evt.newValue;
            });
            mainContainer.Add(panelTypeEnum);
        }

        private Port CreatePort(Orientation orientation, Direction direction)
        {
            return InstantiatePort(orientation, direction, Port.Capacity.Multi, typeof(object));
        }
        
        public DialogNodeForGraph GetData()
        {
            dialogNodeData.position = GetPosition().position;
            return dialogNodeData;
        }
    }
}