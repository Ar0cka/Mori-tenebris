using System;
using System.Collections.Generic;
using Actors.NPC.DialogSystem.DataScripts;
using UnityEngine;

namespace Actors.NPC.DialogSystem.TestUI
{
    public class TestDialogUI : MonoBehaviour
    {
        public Action<string> OnSendActorText;
        public Action<List<DialogNode>> OnSendDialogNodes;
        public Action<DialogNode> OnStartDialog;
        public Action OnClick;
    }
}