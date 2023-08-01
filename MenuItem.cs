using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WoL_Loot_Spawner
{
    public class MenuItem
    {
        private string label;
        private List<DropItemData> dropData;

        public MenuItem(string label, DropItemData dropData)
        {
            this.label = label;
            this.dropData = new List<DropItemData>();
            this.dropData.Add(dropData);
        }

        public MenuItem(string label, List<DropItemData> dropData)
        {
            this.label = label;
            this.dropData = dropData;
        }

        public void RunCommand()
        {
            Vector2 position = GameController.activePlayers[0].transform.position;
            bool wasItemGenerated = false;
            foreach (DropItemData drop in dropData)
            {
                if (drop.type == MenuItemType.SUBFOLDER)
                {
                    MoveToFolder(drop);
                }
                else
                {   
                    drop.GenerateItem(position);
                    SoundManager.PlayAudio("DropSpell", 1f, false, -1f, -1f);
                    wasItemGenerated = true;
                }
            }

            if (wasItemGenerated)
            {
                LootSpawnerPlugin.SetPluginUsed();
            }
        }
        private void MoveToFolder(DropItemData dropData)
        {
            LootSpawnerPlugin.menuManager.SetActiveMenu(dropData.id);
        }

        public string Print(bool isSelected)
        {
            if (isSelected)
            {
                return " <color=#f2c50e>> " + label + "</color>";
            } else
            {
                return "  " + label;
            }
        }
    }

    
}
