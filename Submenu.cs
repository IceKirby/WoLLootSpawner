using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoL_Loot_Spawner
{
    public class Submenu
    {
        public string id;
        public string title;
        private List<MenuItem> items = new List<MenuItem>();
        private int index = 0;
        public bool isRootMenu;
        public string parent;

        private int pageSize = 30;

        public Submenu(string id, string title, string parent)
        {
            this.id = id;
            this.parent = parent;
            this.title = title;
            this.isRootMenu = parent == null;

            if (parent != null)
            {
                AddItem("[Back]", new DropItemData(MenuItemType.SUBFOLDER, parent));
            }
        }

        public void MoveCursor(int direction)
        {
            index = Math.Min(Math.Max(index + direction, 0), items.Count-1);
            SoundManager.PlayAudio("MenuMove", 1f, true, -1f, -1f);
        }
        public void MovePage(int direction)
        {
            index = Math.Min(Math.Max(index + direction*pageSize, 0), items.Count - 1);
            SoundManager.PlayAudio("MenuMove", 1f, true, -1f, -1f);
        }

        public void ConfirmSelection()
        {
            GetSelectedItem().RunCommand();
            SoundManager.PlayConfirmAudio();
        }

        public void ReturnToParent()
        {
            LootSpawnerPlugin.menuManager.SetActiveMenu(this.parent);
        }

        public MenuItem AddItem(string label, DropItemData dropData)
        {
            MenuItem newItem = new MenuItem(label, dropData);
            items.Add(newItem);

            return newItem;
        }

        public MenuItem AddItem(string label, List<DropItemData> dropData)
        {
            MenuItem newItem = new MenuItem(label, dropData);
            items.Add(newItem);

            return newItem;
        }

        private MenuItem GetSelectedItem()
        {
            return items[index];
        }

        public string Print()
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("<size=8>" + title + "</size>");

            int currentPage = (int)Math.Floor((double)(index / pageSize));
            int maxPages = (int)Math.Ceiling((double)(items.Count / pageSize));
            int pageStart = currentPage * pageSize;
            int pageEnd = (currentPage + 1) * pageSize;

            if (maxPages > 0)
            {
                output.AppendLine("[Page " + (currentPage + 1) + "/" + (maxPages+1) + "]");
            }
            for (int i = pageStart; i < items.Count && i < pageEnd; i++)
            {   
                output.AppendLine(items[i].Print(i == index));
            }

            return output.ToString();
        }
    }
}
