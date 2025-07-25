using System;
using System.Collections.Generic;
using Actors.NPC.DialogSystem.DataScripts;
using Unity.VisualScripting;
using UnityEngine;

namespace Actors.NPC.DialogSystem
{
    public class NpcDialogController : MonoBehaviour
    {
        //Должен быть скрипт который инициализирует базовые статы NPC и от него уже отталкиваться при дальнейшей смене веток
        [SerializeField] private List<DialogData> startsDialogDatas;
        [SerializeField] private GameObject dialogPrefab;
        
        private event Action<string, List<DialogData>> _dialogStateChange;

        private DialogNode _currentNode;

        public void InitializeController()
        { 
            //Срабатывает при активации диалога. Подгружает в меню диалога нужные первоначальные текста и добавляет листенеры к ним

            if (dialogPrefab.activeInHierarchy == false)
            {
                dialogPrefab.SetActive(true);
            }
            
            //TODO: Берем скрипт который контролирует слоты для диалогов и создает новые из обхекта dialogPrefab 
            //TODO: Добавить ивенты для подгрузки UI компонентов
        }

        public void ChangeBranch()
        {
            
        }
    }
}