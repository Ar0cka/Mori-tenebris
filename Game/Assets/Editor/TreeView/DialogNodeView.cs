    using Actors.NPC.DialogSystem.DataScripts;
    using Actors.NPC.NpcStateSystem;
    using Actors.NPC.SpecialPanel;
    using UnityEditor.Experimental.GraphView;
    using UnityEditor.Search;
    using UnityEngine;
    using UnityEngine.UIElements;

    namespace Editor.TreeView
    {
        public class DialogNodeView : Node
        {
            public string guid;
            public SerializedDialogNode SerializedDialogNodeData;
            
            private Button _addButton;
            private Button _removeButton;
            
            public Port InputPort { get; private set; }
            public Port OutputPort { get; private set; }

            public DialogNodeView(SerializedDialogNode serializedDialogNodeData)
            {
                this.SerializedDialogNodeData = serializedDialogNodeData;
                guid = serializedDialogNodeData.id;

                title = serializedDialogNodeData.playerDialog.text;
                
                BuildNodeUI(serializedDialogNodeData);
                
                SetPosition(new Rect(serializedDialogNodeData.position, new Vector2(400, 300)));
                
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

            private void BuildNodeUI(SerializedDialogNode data)
            {
                // --- NPC Text ---
                var npcFoldout = new Foldout
                {
                    text = "Npc Dialog",
                    value = true
                };
                
                var controls = new VisualElement { style = { flexDirection = FlexDirection.Row, alignSelf = Align.Center} };
                
                _addButton = new Button(() =>
                {
                    data.npcDialog.Add(new DialogData());
                    RefreshNpcList(data, npcFoldout);
                })
                {
                    text = "+"
                };

                _removeButton = new Button(() =>
                {
                    data.npcDialog.RemoveAt(data.npcDialog.Count - 1);
                    RefreshNpcList(data, npcFoldout);
                })
                {
                    text = "-"
                };
                
                controls.Add(_addButton);
                controls.Add(_removeButton);
                npcFoldout.Add(controls);
                
                mainContainer.Add(npcFoldout);
                RefreshNpcList(data, npcFoldout);

                // --- Player Text ---
                var playerFoldout = new Foldout
                {
                    text = "Player Dialog",
                    value = true
                };
                
                var playerTextField = new TextField("Player Text")
                {
                    value = data.playerDialog?.text ?? ""
                };
                playerTextField.RegisterValueChangedCallback(evt =>
                {
                    data.playerDialog.text = evt.newValue;
                });
                playerFoldout.Add(playerTextField);

                var playerTimeCode = new TextField("Player Text Time Code")
                {
                    value = data.playerDialog?.timeCode.ToString() ?? ""
                };
                playerTimeCode.RegisterValueChangedCallback(evt =>
                {
                    data.playerDialog.timeCode = int.Parse(evt.newValue);
                });
                playerFoldout.Add(playerTimeCode);
                mainContainer.Add(playerFoldout);

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
            
            public SerializedDialogNode GetData()
            {
                SerializedDialogNodeData.position = GetPosition().position;
                return SerializedDialogNodeData;
            }

            private void RefreshNpcList(SerializedDialogNode data, Foldout parentFoldout)
            {
                parentFoldout.Clear();
                
                for (int i = 0; i < data.npcDialog.Count; i++)
                {
                    var dialog = data.npcDialog[i];
                    var npcFoldout = new Foldout { text = $"NPC #{i + 1}" };

                    var npcTextField = new TextField("NPC Text") { value = dialog.text ?? "" };
                    npcTextField.RegisterValueChangedCallback(evt => dialog.text = evt.newValue);
                    npcFoldout.Add(npcTextField);

                    var npcTimeCode = new IntegerField("NPC Time Code") { value = dialog.timeCode };
                    npcTimeCode.RegisterValueChangedCallback(evt => dialog.timeCode = evt.newValue);
                    npcFoldout.Add(npcTimeCode);

                    var npcReputation = new EnumField("Reputation state", data.npcDialog[i].dialogReputation);
                    npcReputation.RegisterValueChangedCallback(evt => dialog.dialogReputation = (NpcReputationEnum)evt.newValue);
                    npcFoldout.Add(npcReputation);

                    parentFoldout.Add(npcFoldout);
                }
                
                var controls = new VisualElement { style = { flexDirection = FlexDirection.Row, alignSelf = Align.FlexEnd} };
                
                controls.Add(_addButton);
                controls.Add(_removeButton);
                parentFoldout.Add(controls);
            }
        }
    }