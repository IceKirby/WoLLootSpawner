using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WoL_Loot_Spawner
{
    public class MenuListsGenerator
    {
        public static List<string> allSkillsId;

        public static void BuildRelicPage(Submenu menu, Func<string, Item, bool> filter)
        {
            List<DropItemData> output = new List<DropItemData>();

            Dictionary<string, Item> dict = LootManager.completeItemDict;

            foreach(KeyValuePair<string, Item> entry in dict)
            {
                if (filter(entry.Key, entry.Value))
                {
                    output.Add(new DropItemData(MenuItemType.ITEM, entry.Key));
                }
            }

            output = output.OrderBy(x => TextManager.GetItemName(x.id)).ToList();

            foreach (DropItemData item in output)
            {
                menu.AddItem(TextManager.GetItemName(item.id), item);
            }
        }
        public static void BuildComboRelicPage(Submenu menu, Dictionary<string, List<string>> dict)
        {
            List<Tuple<string, List<DropItemData>>> output = new List<Tuple<string, List<DropItemData>>>();

            foreach(KeyValuePair<string, List<string>> entry in dict)
            {
                string label = TextManager.GetItemName(entry.Key);
                List<DropItemData> items = new List<DropItemData>();
                foreach (string key in entry.Value)
                {
                    items.Add(new DropItemData(MenuItemType.ITEM, key));
                }
                output.Add(new Tuple<string, List<DropItemData>>(label, items));
            }

            output = output.OrderBy(x => x.First).ToList();

            foreach (Tuple<string, List<DropItemData>> combo in output)
            {
                menu.AddItem(combo.First, combo.Second);
            }
        }

        public static void BuildArcanaPage(Submenu menu, ElementType element, bool isBasic, bool isDash, bool isStandard, bool includeEnhanced, bool includeNormal)
        {
            if (allSkillsId == null)
            {
                allSkillsId = GetAllSkillsId();
            }

            List<DropItemData> output = new List<DropItemData>();
            Player player = GameUI.P1Hud.player;

            foreach (string key in allSkillsId)
            {
                Player.SkillState skill = player.GetSkill(key);
                if (skill == null)
                {
                    Debug.Log("Skill not found: " + key);
                    continue;
                }
                if (element == skill.element && isBasic == skill.isBasic && isDash == skill.isDash && isStandard == !(skill.isDash || skill.isBasic))
                {
                    if (includeNormal)
                    {
                        output.Add(new DropItemData(MenuItemType.SKILL, key));
                    }
                    if (includeEnhanced)
                    {
                        output.Add(new DropItemData(MenuItemType.SKILL, key + "Empowered"));
                    }
                }
            }

            output = output.OrderBy(x => GetSkillName(x.id)).ToList();

            foreach (DropItemData skill in output)
            {   
                menu.AddItem(GetSkillName(skill.id), skill);
            }
        }

        public static void BuildArcanaCombosPage(Submenu menu, Submenu menuEnhanced)
        {
            AddArcanaCombo(menu, menuEnhanced, "Agents", new List<string> {
                Player.UseWaterMinion.staticID,
                Player.UseEarthMinion.staticID,
                Player.UseFireMinion.staticID,
                Player.UseWindMinion.staticID,
                Player.UseLightningMinion.staticID,
                Player.UseChaosMinion.staticID + "Empowered"
            }, true);

            AddArcanaCombo(menu, menuEnhanced, "Seekers", new List<string> {
                Player.UseFlameSeekers.staticID,
                Player.UseEarthSeekers.staticID,
                Player.UseWindSeekers.staticID,
                Player.UseThunderSeekers.staticID,
                Player.SummonIceSeekersState.staticID
            }, true);

            AddArcanaCombo(menu, menuEnhanced, "Wards", new List<string> {
                Player.UseFireWard.staticID,
                Player.UseEarthWard.staticID,
                Player.UseLightningWard.staticID,
                Player.UseWindWard.staticID,
                Player.UseWaterWard.staticID,
                Player.UseChaosWard.staticID + "Empowered"
            }, true);
            
            AddArcanaCombo(menu, menuEnhanced, "Dragon", new List<string> {
                Player.UseDragonBreath.staticID,
                Player.UseAquaDragon.staticID,
                Player.ShootFireArc.staticID,
                Player.UseShockDragon.staticID,
                Player.UseDragonStomp.staticID,
                Player.UseDragonGrade.staticID + "Empowered"
            }, true);

            AddArcanaCombo(menu, menuEnhanced, "Basic Buff", new List<string> {
                Player.BerserkState.staticID,
                Player.UseAquaCooling.staticID,
                Player.UseEarthEnhance.staticID,
                Player.UseElectricAura.staticID,
                Player.UseWindDefense.staticID,
                Player.UseChaosWeapons.staticID + "Empowered"
            }, true);

        }

        public static void AddArcanaCombo(Submenu menu, Submenu menuEnhanced, string label, List<string> ids, bool includeEmpoweredVersion)
        {
            List<DropItemData> output = new List<DropItemData>();

            foreach (string id in ids)
            {
                output.Add(new DropItemData(MenuItemType.SKILL, id));
            }

            menu.AddItem(label, output);

            if (includeEmpoweredVersion)
            {
                List<string> empoweredIds = ids.ConvertAll<string>(obj => obj.EndsWith("Empowered") ? obj : obj + "Empowered");
                AddArcanaCombo(menuEnhanced, null, label, empoweredIds, false);
            }
        }

        public static List<string> GetAllSkillsId()
        {
            List<string> output = new List<string>();

            foreach (KeyValuePair<int, List<string>> entry in LootManager.skillTierDict)
            {
                output.AddRange(entry.Value);
            }
            output.AddRange(LootManager.chaosSkillList);

            return output;
        }

        public static string GetSkillName(string id)
        {
            string label = "[ERROR]";
            if (id.EndsWith("Empowered"))
            {
                label = TextManager.GetSkillName(id.Substring(0, id.Length - 9)) + " (Enhanced)";
            }
            else
            {
                label = TextManager.GetSkillName(id);
            }
            return label;
        }

        public static void GenerateOtherMenu(Submenu menu)
        {
            menu.AddItem("Health Orb x2", new DropItemData(MenuItemType.HEALTH, null, 2));
            menu.AddItem("Health Orb x10", new DropItemData(MenuItemType.HEALTH, null, 10));
            menu.AddItem("Gold x10", new DropItemData(MenuItemType.GOLD, null, 10));
            menu.AddItem("Gold x50", new DropItemData(MenuItemType.GOLD, null, 50));
            menu.AddItem("Gold x100", new DropItemData(MenuItemType.GOLD, null, 100));
            menu.AddItem("Gold x500", new DropItemData(MenuItemType.GOLD, null, 500));
            menu.AddItem("Gem x1", new DropItemData(MenuItemType.GEM, null, 1));
            menu.AddItem("Gem x5", new DropItemData(MenuItemType.GEM, null, 5));
            menu.AddItem("Gem x10", new DropItemData(MenuItemType.GEM, null, 10));
            menu.AddItem("Gem x50", new DropItemData(MenuItemType.GEM, null, 50));
        }
    }
}
