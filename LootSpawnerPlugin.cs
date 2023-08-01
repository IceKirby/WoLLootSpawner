using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using BepInEx.Configuration;

namespace WoL_Loot_Spawner
{
    [BepInPlugin("com.RiceKirby.LootSpawner", "Loot Spawner", "1.0.0")]
    public class LootSpawnerPlugin : BaseUnityPlugin
    {
        public static bool isDisplaying = false;
        public static ConfigEntry<string> toggleKey;
        public static MenuManager menuManager;

        private static ChaosQuickStopwatch navTimer = new ChaosQuickStopwatch(0.3f);
        private static ChaosQuickStopwatch autoNavTimer = new ChaosQuickStopwatch(0.075f);
        private static bool initNavDelayReset = true;

        void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(LootSpawnerPlugin));
            toggleKey = Config.Bind("Keybindings", "DisplaySpawnerMenu", "f1", "The key used to display the spawner menu. Name of the key, as used by https://docs.unity3d.com/ScriptReference/Input.GetKeyDown.html");
        }

        [HarmonyPatch(typeof(LowerHUD), "Update")]
        [HarmonyPostfix]
        static void UpdateMenu(LowerHUD __instance)
        {
            
            if (__instance != GameUI.P1Hud || !__instance.player)
            {
                return;
            }
            if (GameUI.pauseMenu.paused)
            {
                return;
            }

            Player player = __instance.player;

            if (LootSpawnerPlugin.toggleKey.Value != "" && Input.GetKeyDown(LootSpawnerPlugin.toggleKey.Value) && isMenuAvailable(player))
            {
                DisplaySpawnerMenu(!isDisplaying);
            }

            GameObject hubObj = Traverse.Create(__instance).Field("hudObj").GetValue() as GameObject;
            Transform hudTrans = hubObj.transform;
            Transform statsMenu = hudTrans.Find("SpawnerMenu");
            if (statsMenu == null) return;
            if (statsMenu.gameObject.activeInHierarchy)
            {
                var inputVal = GetMoveDirection();
                if (inputVal != 0)
                {
                    menuManager.MoveCursor(inputVal);
                }
                var inputValH = ((!InputController.GetAnyDeviceDirection(InputDirection.Left, true)) ? ((!InputController.GetAnyDeviceDirection(InputDirection.Right, true)) ? 0 : 1) : -1);
                if (inputValH != 0)
                {
                    menuManager.MovePage(inputValH);
                }

                if (player.inputDevice.GetButtonUp("Interact") || player.inputDevice.GetButtonUp("Confirm"))
                {
                    menuManager.ConfirmSelection();
                }
                if (Input.GetKeyUp(KeyCode.Escape) || player.inputDevice.GetButtonUp("Cancel"))
                {
                    menuManager.CancelSelection();
                }

                statsMenu.Find("Content").GetComponent<Text>().text = menuManager.Print();
            }
        }

        private static int GetMoveDirection()
        {
            var inputVal = ((!InputController.GetAnyDeviceDirection(InputDirection.Down, false)) ? ((!InputController.GetAnyDeviceDirection(InputDirection.Up, false)) ? 0 : -1) : 1);
            if (inputVal == 0)
            {
                initNavDelayReset = true;
                navTimer.IsRunning = false;
                autoNavTimer.IsRunning = false;
                return 0;
            }
            if (navTimer.IsRunning || autoNavTimer.IsRunning)
            {
                return 0;
            }
            if (initNavDelayReset)
            {
                initNavDelayReset = false;
                navTimer.IsRunning = true;
            }
            else
            {
                autoNavTimer.IsRunning = true;
            }
            return inputVal;
        }

        public static void DisplaySpawnerMenu(bool display)
        {
            GameObject hubObj = Traverse.Create(GameUI.P1Hud).Field("hudObj").GetValue() as GameObject;
            Transform hudTrans = hubObj.transform;
            Transform statsMenuTransform = hudTrans.Find("SpawnerMenu");

            if (statsMenuTransform == null)
            {
                if (menuManager == null)
                {
                    menuManager = new MenuManager();
                }

                var munroSmall = ChaosBundle.Get<Font>("Assets/Fonts/MunroSmall.ttf");
                var statsMenuBuilder = new GameObject("SpawnerMenu");

                // Outer Box
                var outerBoxRectTransform = statsMenuBuilder.AddComponent<RectTransform>();
                var outerBoxRenderer = statsMenuBuilder.AddComponent<CanvasRenderer>();

                var outerBoxImage = statsMenuBuilder.AddComponent<Image>();
                outerBoxImage.sprite = IconManager.GetCDBorder(0, true);
                outerBoxImage.color = new Color(0.110f, 0.153f, 0.192f, 0.941f);
                outerBoxImage.type = Image.Type.Sliced;

                outerBoxRectTransform.anchorMin = new Vector2(0.75f, 0);
                outerBoxRectTransform.anchorMax = new Vector2(1, 1);
                outerBoxRectTransform.offsetMin = new Vector2(10, 10);
                outerBoxRectTransform.offsetMax = new Vector2(-10, -10);

                //Title
                var titleObj = new GameObject("Title");
                titleObj.transform.SetParent(statsMenuBuilder.transform);
                var titleCanvasRenderer = titleObj.AddComponent<CanvasRenderer>();
                var titleRectTransform = titleObj.AddComponent<RectTransform>();
                var titleOutline = titleObj.AddComponent<Outline>();
                var titleText = titleObj.AddComponent<Text>();

                titleText.text = "Loot Spawner";
                titleText.color = new Color(1, 1, 1, 0.8f);
                titleText.font = munroSmall;
                titleText.fontSize = 11;
                titleText.fontStyle = FontStyle.Normal;

                titleOutline.effectDistance = new Vector2(0.5f, 0.5f);
                titleRectTransform.anchorMin = new Vector2(0, 0);
                titleRectTransform.anchorMax = new Vector2(0, 1);
                titleRectTransform.offsetMin = new Vector2(5, -4);
                titleRectTransform.offsetMax = new Vector2(200, -4);

                // Content
                var contentObj = new GameObject("Content");
                contentObj.transform.SetParent(statsMenuBuilder.transform);
                var contentCanvasRenderer = contentObj.AddComponent<CanvasRenderer>();
                var contentRectTransform = contentObj.AddComponent<RectTransform>();
                var contentText = contentObj.AddComponent<Text>();

                contentText.font = munroSmall;
                contentText.fontSize = 6;
                contentText.fontStyle = FontStyle.Normal;
                contentText.color = new Color(0.7176471f, 0.7529412f, 0.7647059f, 1);
                contentText.supportRichText = true;

                contentRectTransform.anchorMin = new Vector2(0, 0);
                contentRectTransform.anchorMax = new Vector2(1, 1);
                contentRectTransform.offsetMin = new Vector2(5, 5);
                contentRectTransform.offsetMax = new Vector2(-5, -18);

                //Instantiating
                statsMenuTransform = Instantiate(statsMenuBuilder, hudTrans).transform;
                statsMenuTransform.name = statsMenuBuilder.name;
            }

            isDisplaying = display;
            statsMenuTransform.gameObject.SetActive(display);
            GameController.PauseAllPlayers(display);
            SoundManager.PlayMenuAudio(display);
        }

        [HarmonyPatch(typeof(LowerHUD), "SetEquipMenuStatus")]
        [HarmonyPrefix]
        static void ToggleMenu(bool givenStatus, LowerHUD __instance)
        {
            if (givenStatus)
            {
                DisplaySpawnerMenu(false);
            }
        }

        static bool isMenuAvailable(Player player)
        {
            return player != null && player.enabled && (player.currentStateName == "Idle" || player.currentStateName == "Run" || player.currentStateName == "Paused");
        }

        public static void SetPluginUsed()
        {
            GameUI uiObj = GameUI.instance;
            Transform taintedTransform = uiObj.transform.Find("TaintedMark");

            if (taintedTransform == null)
            {
                var taintedMarkObj = new GameObject("TaintedMark");

                // Outer Box
                var outerBoxRectTransform = taintedMarkObj.AddComponent<RectTransform>();
                var outerBoxRenderer = taintedMarkObj.AddComponent<CanvasRenderer>();
                outerBoxRectTransform.anchorMin = new Vector2(0.9f, 0);
                outerBoxRectTransform.anchorMax = new Vector2(1, 1);

                //Title
                var titleObj = new GameObject("Title");
                titleObj.transform.SetParent(taintedMarkObj.transform);
                var titleCanvasRenderer = titleObj.AddComponent<CanvasRenderer>();
                var titleRectTransform = titleObj.AddComponent<RectTransform>();
                var titleOutline = titleObj.AddComponent<Outline>();
                var titleText = titleObj.AddComponent<Text>();

                var munroSmall = ChaosBundle.Get<Font>("Assets/Fonts/MunroSmall.ttf");
                titleText.text = "Loot Spawner Activated";
                titleText.color = new Color(1, 1, 1);
                titleText.font = munroSmall;
                titleText.fontSize = 6;
                titleText.fontStyle = FontStyle.Normal;
                titleText.alignment = TextAnchor.LowerRight;
                titleOutline.effectDistance = new Vector2(0.5f, 0.5f);

                var taintedMarkInstance = Instantiate(taintedMarkObj, uiObj.transform);
                RectTransform taintedRect = taintedMarkInstance.GetComponent<RectTransform>();
                taintedRect.offsetMin = new Vector2(-53, -170);
                taintedRect.offsetMax = new Vector2(0, 0);
                taintedMarkInstance.transform.name = taintedMarkObj.name;
            }
        }
        
    }
}