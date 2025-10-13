using System;
using System.Collections.Generic;
using ConsoleApp.Runtime;
using Player.Inventory;
using UnityEngine;

namespace DefaultNamespace
{
    public class PanelController
    {
        private Dictionary<Type, object> _panels = new();

        public void RegisterNewPanelType<TValue>(TValue value)
        {
            if (!_panels.TryAdd(typeof(TValue), null))
            {
                ConsoleLogger.Error("_panel contains duplicate panel");
            }
        }
        public void UpdatePanel<TValue>(IPanelOpen<TValue> panel)
        {
            _panels[typeof(TValue)] = panel;
            Debug.Log($"Update panel {typeof(TValue).Name}");
        }

        public void OpenPanel<TValue>(TValue panelValue)
        {
            if (_panels.TryGetValue(typeof(TValue), out var panel))
            {
                ((IPanelOpen<TValue>)panel).Open(panelValue);
            }
        }
    }
}