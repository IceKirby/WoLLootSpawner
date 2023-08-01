using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoL_Loot_Spawner
{
    public class MenuManager
    {
        private List<Submenu> submenus = new List<Submenu>();

        private string activeId;

        public MenuManager()
        {
            Submenu main = CreateSubmenu("main", "Main Menu", null);

            Submenu relics0 = CreateSubmenu("relics", "Relics", "main");
            Submenu relics1 = CreateSubmenu("relics_atk", "Offense", "relics");
            Submenu relics2 = CreateSubmenu("relics_def", "Defense", "relics");
            Submenu relics3 = CreateSubmenu("relics_misc", "Misc", "relics");
            Submenu relics4 = CreateSubmenu("relics_doc", "Doctor", "relics");
            Submenu relics5 = CreateSubmenu("relics_cursed", "Cursed", "relics");
            Submenu relics6 = CreateSubmenu("relics_fusion", "Fusion", "relics");
            Submenu relics7 = CreateSubmenu("relics_bundle", "Bundle", "relics");
            
            Submenu arcana0 = CreateSubmenu("arcana", "Arcana", "main");
            Submenu arcanaE0 = CreateSubmenu("arcana_enhanced", "Arcana (Enhanced)", "main");

            Submenu arcana1 = CreateSubmenu("arcana_basic", "Basic Arcana", "arcana");
            Submenu arcana2 = CreateSubmenu("arcana_dash", "Dash Arcana", "arcana");
            Submenu arcana3 = CreateSubmenu("arcana_standard", "Standard Arcana", "arcana");
            Submenu arcana4 = CreateSubmenu("arcana_combo", "Bundles", "arcana");

            Submenu arcanaE1 = CreateSubmenu("arcana_enhanced_basic", "Basic Arcana", "arcana_enhanced");
            Submenu arcanaE2 = CreateSubmenu("arcana_enhanced_dash", "Dash Arcana", "arcana_enhanced");
            Submenu arcanaE3 = CreateSubmenu("arcana_enhanced_standard", "Standard Arcana", "arcana_enhanced");
            Submenu arcanaE4 = CreateSubmenu("arcana_enhanced_combo", "Bundles", "arcana_enhanced");

            Submenu basicArcanaFire = CreateSubmenu("arcana_basic_fire", "Fire (Basic)", "arcana_basic");
            Submenu basicArcanaAir = CreateSubmenu("arcana_basic_air", "Air (Basic)", "arcana_basic");
            Submenu basicArcanaEarth = CreateSubmenu("arcana_basic_earth", "Earth (Basic)", "arcana_basic");
            Submenu basicArcanaLightning = CreateSubmenu("arcana_basic_lightning", "Lightning (Basic)", "arcana_basic");
            Submenu basicArcanaWater = CreateSubmenu("arcana_basic_water", "Water (Basic)", "arcana_basic");
            
            Submenu basicArcanaFireE = CreateSubmenu("arcana_enhanced_basic_fire", "Fire (Basic)", "arcana_enhanced_basic");
            Submenu basicArcanaAirE = CreateSubmenu("arcana_enhanced_basic_air", "Air (Basic)", "arcana_enhanced_basic");
            Submenu basicArcanaEarthE = CreateSubmenu("arcana_enhanced_basic_earth", "Earth (Basic)", "arcana_enhanced_basic");
            Submenu basicArcanaLightningE = CreateSubmenu("arcana_enhanced_basic_lightning", "Lightning (Basic)", "arcana_enhanced_basic");
            Submenu basicArcanaWaterE = CreateSubmenu("arcana_enhanced_basic_water", "Water (Basic)", "arcana_enhanced_basic");
            Submenu basicArcanaChaosE = CreateSubmenu("arcana_enhanced_basic_chaos", "Chaos (Basic)", "arcana_enhanced_basic");

            Submenu dashArcanaFire = CreateSubmenu("arcana_dash_fire", "Fire (Dash)", "arcana_dash");
            Submenu dashArcanaAir = CreateSubmenu("arcana_dash_air", "Air (Dash)", "arcana_dash");
            Submenu dashArcanaEarth = CreateSubmenu("arcana_dash_earth", "Earth (Dash)", "arcana_dash");
            Submenu dashArcanaLightning = CreateSubmenu("arcana_dash_lightning", "Lightning (Dash)", "arcana_dash");
            Submenu dashArcanaWater = CreateSubmenu("arcana_dash_water", "Water (Dash)", "arcana_dash");

            Submenu dashArcanaFireE = CreateSubmenu("arcana_enhanced_dash_fire", "Fire (Dash)", "arcana_enhanced_dash");
            Submenu dashArcanaAirE = CreateSubmenu("arcana_enhanced_dash_air", "Air (Dash)", "arcana_enhanced_dash");
            Submenu dashArcanaEarthE = CreateSubmenu("arcana_enhanced_dash_earth", "Earth (Dash)", "arcana_enhanced_dash");
            Submenu dashArcanaLightningE = CreateSubmenu("arcana_enhanced_dash_lightning", "Lightning (Dash)", "arcana_enhanced_dash");
            Submenu dashArcanaWaterE = CreateSubmenu("arcana_enhanced_dash_water", "Water (Dash)", "arcana_enhanced_dash");
            Submenu dashArcanaChaosE = CreateSubmenu("arcana_enhanced_dash_chaos", "Chaos (Dash)", "arcana_enhanced_dash");

            Submenu standardArcanaFire = CreateSubmenu("arcana_standard_fire", "Fire (Standard)", "arcana_standard");
            Submenu standardEnhancedArcanaFire = CreateSubmenu("arcana_standard_enhanced_fire", "Fire (Enhanced Standard)", "arcana_enhanced_standard");

            Submenu standardArcanaAir = CreateSubmenu("arcana_standard_air", "Air (Standard)", "arcana_standard");
            Submenu standardEnhancedArcanaAir = CreateSubmenu("arcana_standard_enhanced_air", "Air (Enhanced Standard)", "arcana_enhanced_standard");

            Submenu standardArcanaEarth = CreateSubmenu("arcana_standard_earth", "Earth (Standard)", "arcana_standard");
            Submenu standardEnhancedArcanaEarth = CreateSubmenu("arcana_standard_enhanced_earth", "Earth (Enhanced Standard)", "arcana_enhanced_standard");

            Submenu standardArcanaLightning = CreateSubmenu("arcana_standard_lightning", "Lightning (Standard)", "arcana_standard");
            Submenu standardEnhancedArcanaLightning = CreateSubmenu("arcana_standard_enhanced_lightning", "Lightning (Enhanced Standard)", "arcana_enhanced_standard");

            Submenu standardArcanaWater = CreateSubmenu("arcana_standard_water", "Water (Standard)", "arcana_standard");
            Submenu standardEnhancedArcanaWater = CreateSubmenu("arcana_standard_enhanced_water", "Water (Enhanced Standard)", "arcana_enhanced_standard");

            Submenu standardEnhancedArcanaChaos = CreateSubmenu("arcana_standard_enhanced_chaos", "Chaos (Enhanced Standard)", "arcana_enhanced_standard");
            
            Submenu other = CreateSubmenu("other", "Other", "main");

            MenuListsGenerator.BuildRelicPage(relics1, (string key, Item value) => {
                return value.category == Item.Category.Offense && !value.isCursed && !value.isGroupItem && !LootManager.excludeItemDict.ContainsKey(key) && !DoctorNpc.itemList.Contains(key);
            });
            MenuListsGenerator.BuildRelicPage(relics2, (string key, Item value) => {
                return value.category == Item.Category.Defense && !value.isCursed && !value.isGroupItem && !LootManager.excludeItemDict.ContainsKey(key) && !DoctorNpc.itemList.Contains(key);
            });
            MenuListsGenerator.BuildRelicPage(relics3, (string key, Item value) => {
                return value.category == Item.Category.Misc && !value.isCursed && !value.isGroupItem && !LootManager.excludeItemDict.ContainsKey(key) && !DoctorNpc.itemList.Contains(key);
            });
            MenuListsGenerator.BuildRelicPage(relics4, (string key, Item value) => {
                return DoctorNpc.itemList.Contains(key);
            });
            MenuListsGenerator.BuildRelicPage(relics5, (string key, Item value) => {
                return value.isCursed && !value.isGroupItem;
            });
            MenuListsGenerator.BuildComboRelicPage(relics6, LootManager.excludeItemDict);
            MenuListsGenerator.BuildComboRelicPage(relics7, GroupItemManager.groupsDict);

            MenuListsGenerator.BuildArcanaPage(basicArcanaFire, ElementType.Fire, true, false, false, false, true);
            MenuListsGenerator.BuildArcanaPage(basicArcanaAir, ElementType.Air, true, false, false, false, true);
            MenuListsGenerator.BuildArcanaPage(basicArcanaEarth, ElementType.Earth, true, false, false, false, true);
            MenuListsGenerator.BuildArcanaPage(basicArcanaLightning, ElementType.Lightning, true, false, false, false, true);
            MenuListsGenerator.BuildArcanaPage(basicArcanaWater, ElementType.Water, true, false, false, false, true);
            
            MenuListsGenerator.BuildArcanaPage(basicArcanaFireE, ElementType.Fire, true, false, false, true, false);
            MenuListsGenerator.BuildArcanaPage(basicArcanaAirE, ElementType.Air, true, false, false, true, false);
            MenuListsGenerator.BuildArcanaPage(basicArcanaEarthE, ElementType.Earth, true, false, false, true, false);
            MenuListsGenerator.BuildArcanaPage(basicArcanaLightningE, ElementType.Lightning, true, false, false, true, false);
            MenuListsGenerator.BuildArcanaPage(basicArcanaWaterE, ElementType.Water, true, false, false, true, false);

            MenuListsGenerator.BuildArcanaPage(basicArcanaChaosE, ElementType.Chaos, true, false, false, true, false);

            MenuListsGenerator.BuildArcanaPage(dashArcanaFire, ElementType.Fire, false, true, false, false, true);
            MenuListsGenerator.BuildArcanaPage(dashArcanaAir, ElementType.Air, false, true, false, false, true);
            MenuListsGenerator.BuildArcanaPage(dashArcanaEarth, ElementType.Earth, false, true, false, false, true);
            MenuListsGenerator.BuildArcanaPage(dashArcanaLightning, ElementType.Lightning, false, true, false, false, true);
            MenuListsGenerator.BuildArcanaPage(dashArcanaWater, ElementType.Water, false, true, false, false, true);
            
            MenuListsGenerator.BuildArcanaPage(dashArcanaFireE, ElementType.Fire, false, true, false, true, false);
            MenuListsGenerator.BuildArcanaPage(dashArcanaAirE, ElementType.Air, false, true, false, true, false);
            MenuListsGenerator.BuildArcanaPage(dashArcanaEarthE, ElementType.Earth, false, true, false, true, false);
            MenuListsGenerator.BuildArcanaPage(dashArcanaLightningE, ElementType.Lightning, false, true, false, true, false);
            MenuListsGenerator.BuildArcanaPage(dashArcanaWaterE, ElementType.Water, false, true, false, true, false);

            MenuListsGenerator.BuildArcanaPage(dashArcanaChaosE, ElementType.Chaos, false, true, false, true, false);

            MenuListsGenerator.BuildArcanaPage(standardArcanaFire, ElementType.Fire, false, false, true, false, true);
            MenuListsGenerator.BuildArcanaPage(standardEnhancedArcanaFire, ElementType.Fire, false, false, true, true, false);
            
            MenuListsGenerator.BuildArcanaPage(standardArcanaAir, ElementType.Air, false, false, true, false, true);
            MenuListsGenerator.BuildArcanaPage(standardEnhancedArcanaAir, ElementType.Air, false, false, true, true, false);
            
            MenuListsGenerator.BuildArcanaPage(standardArcanaEarth, ElementType.Earth, false, false, true, false, true);
            MenuListsGenerator.BuildArcanaPage(standardEnhancedArcanaEarth, ElementType.Earth, false, false, true, true, false);
            
            MenuListsGenerator.BuildArcanaPage(standardArcanaLightning, ElementType.Lightning, false, false, true, false, true);
            MenuListsGenerator.BuildArcanaPage(standardEnhancedArcanaLightning, ElementType.Lightning, false, false, true, true, false);
            
            MenuListsGenerator.BuildArcanaPage(standardArcanaWater, ElementType.Water, false, false, true, false, true);
            MenuListsGenerator.BuildArcanaPage(standardEnhancedArcanaWater, ElementType.Water, false, false, true, true, false);
            
            MenuListsGenerator.BuildArcanaPage(standardEnhancedArcanaChaos, ElementType.Chaos, false, false, true, true, false);

            MenuListsGenerator.BuildArcanaCombosPage(arcana4, arcanaE4);

            MenuListsGenerator.GenerateOtherMenu(other);

            GenerateFolderNavigation();
            SetActiveMenu("main");
        }

        public void SetActiveMenu(string id)
        {
            activeId = id;
        }

        public void MoveCursor(int direction)
        {
            GetActiveMenu().MoveCursor(direction);
        }
        public void MovePage(int direction)
        {
            GetActiveMenu().MovePage(direction);
        }

        public void ConfirmSelection()
        {
            GetActiveMenu().ConfirmSelection();
        }

        public void CancelSelection()
        {
            Submenu active = GetActiveMenu();
            if (active.isRootMenu)
            {
                LootSpawnerPlugin.DisplaySpawnerMenu(false);
            } else
            {
                active.ReturnToParent();
            }
        }

        public string Print()
        {
            return GetActiveMenu().Print();
        }

        public Submenu GetActiveMenu()
        {
            foreach (Submenu menu in submenus)
            {
                if (menu.id == activeId)
                {
                    return menu;
                }
            }
            return submenus[0];
        }

        public Submenu FindById(string id)
        {
            foreach (Submenu menu in submenus)
            {
                if (menu.id == id)
                {
                    return menu;
                }
            }
            return null;
        }

        public void ReturnToRoot()
        {
            foreach (Submenu menu in submenus)
            {
                if (menu.isRootMenu)
                {
                    SetActiveMenu(menu.id);
                    break;
                }
            }
        }

        public Submenu CreateSubmenu(string id, string title, string parent)
        {
            Submenu newMenu = new Submenu(id, title, parent);
            submenus.Add(newMenu);
            return newMenu;
        }

        private void GenerateFolderNavigation()
        {
            foreach (Submenu menu in submenus)
            {
                Submenu parentMenu = FindById(menu.parent);
                if (parentMenu != null)
                {
                    parentMenu.AddItem(menu.title, new DropItemData(MenuItemType.SUBFOLDER, menu.id));
                }
            }
        }
    }
}
