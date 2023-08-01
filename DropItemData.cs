using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WoL_Loot_Spawner
{
    public class DropItemData
    {
        public MenuItemType type;
        public string id;
        private int amount;

        public DropItemData(MenuItemType type, string id, int amount=1)
        {
            this.type = type;
            this.id = id;
            this.amount = amount;
        }

        public void GenerateItem(Vector2 position)
        {
            switch (this.type)
            {   
                case MenuItemType.ITEM:
                    SpawnItem(position);
                    break;
                case MenuItemType.SKILL:
                    SpawnSkill(position);
                    break;
                case MenuItemType.GOLD:
                    SpawnGold(position);
                    break;
                case MenuItemType.GEM:
                    SpawnGem(position);
                    break;
                case MenuItemType.HEALTH:
                    SpawnHealth(position);
                    break;
            }
        }

        private void SpawnItem(Vector2 position)
        {
            GameController.itemSpawner.SpawnItem(ItemSpawner.PoolType.ItemDrop, position, true, 1.5f, this.id);
        }

        private void SpawnSkill(Vector2 position)
        {
            GameController.itemSpawner.SpawnItem(ItemSpawner.PoolType.SkillDrop, position, true, 1.5f, this.id);
        }
        private void SpawnGold(Vector2 position)
        {
            GameController.itemSpawner.SpawnGoldValue(this.amount, position);
        }
        private void SpawnGem(Vector2 position)
        {
            GameController.itemSpawner.SpawnItemWithCount(ItemSpawner.PoolType.Platinum, position, this.amount, true, 1.5f);
        }
        private void SpawnHealth(Vector2 position)
        {
            GameController.itemSpawner.SpawnItemWithCount(ItemSpawner.PoolType.HealthPowerup, position, this.amount, true, 1.5f);
        }
    }

    public enum MenuItemType
    {
        SUBFOLDER,
        ITEM,
        SKILL,
        GOLD,
        GEM,
        HEALTH
    }
}
