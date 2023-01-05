using BepInEx.Logging;
using PotionCraft.ObjectBased.UIElements;
using PotionCraft.ObjectBased.UIElements.LegendaryRecipe;
using PotionCraft.ObjectBased.UIElements.Tooltip;
using PotionCraft.QuestSystem.DesiredItems;
using PotionCraft.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;


namespace LegendaryRecipePin
{
    public class PinButton : SpriteChangingButton
    {
        static ManualLogSource Log => Plugin.Log;

        public string machineSlot;

        /// <summary>
        /// Update the position of the buttons
        /// </summary>
        /// <param name="name">The name of the button.</param>
        /// <param name="idle">The name of the texture to use for the button</param>
        /// <param name="getTooltip">The name of the tooltip method to use for the button</param>
        /// <param name="slot">The name of item slot object the button is for</param>
        public static T Create<T>(string name, Texture2D idle, Func<TooltipContent> getTooltip, string slot) where T : PinButton
        {
            GameObject obj = new(name)
            {
                layer = LayerMask.NameToLayer("UI")
            };

            var button = obj.AddComponent<T>();
            button.spriteRenderer = obj.AddComponent<SpriteRenderer>();
            button.thisCollider = obj.AddComponent<BoxCollider2D>();
            button.sortingGroup = obj.AddComponent<SortingGroup>();
            button.tooltipContentProvider = obj.AddComponent<TooltipContentProvider>();

            Sprite sprHover = Sprite.Create(idle, new Rect(0, 0, idle.width, idle.height), new Vector2(0.5f, 0.5f));
            button.hoveredSprite = Utilities.pinButtonBGSprite;
            button.pressedSprite = Utilities.pinButtonBGSprite;

            Sprite sprIdle = Sprite.Create(idle, new Rect(0, 0, idle.width, idle.height), new Vector2(0.5f, 0.5f));
            button.lockedSprite = Utilities.pinButtonBlankSprite;
            button.normalSprite = Utilities.pinButtonBlankSprite;

            button.hoveredSprites = new Sprite[0];
            button.pressedSprites = new Sprite[0];
            button.lockedSprites = new Sprite[0];
            button.normalSprites = new Sprite[0];

            button.IgnoreRotationForPivot = true;
            button.showOnlyFingerWhenInteracting = true;

            button.spriteRenderer.sprite = button.normalSprite;
            (button.thisCollider as BoxCollider2D).size = button.spriteRenderer.sprite.bounds.size;

            button.sortingGroup.sortingLayerID = -1461566583;
            button.sortingGroup.sortingLayerName = "LegendaryRecipe";

            button.tooltipContentProvider.fadingType = TooltipContentProvider.FadingType.UIElement;
            button.tooltipContentProvider.positioningSettings = new List<PositioningSettings>()
            {
                new PositioningSettings()
                {
                    bindingPoint = PositioningSettings.BindingPoint.TransformPosition,
                    freezeX = true,
                    freezeY = true,
                    position = new Vector2(0f, 1.2f),
                    tooltipCorner = PositioningSettings.TooltipCorner.LeftTop
                }
            };
            button.tooltipContentProvider.tooltipCollider = button.thisCollider;

            button._getTooltip = getTooltip;

            button.raycastPriorityLevel = -13015f;

            obj.SetActive(true);

            string sName = slot + "ItemSlot";
            var slotObj = Resources.FindObjectsOfTypeAll<LegendaryRecipeItemSlot>().Where(s => s.name == sName).First().gameObject;
            GameObject child = slotObj.transform.GetChild(2).gameObject;
            Transform targetParent = slotObj.transform;
            Vector3 targetPos = child.transform.localPosition;

            button.transform.parent = targetParent;
            button.transform.localPosition = targetPos;

            button.machineSlot = sName;

            return button;
        }

        public Func<TooltipContent> _getTooltip;

        public override void Start()
        {
            base.Start();
        }

        public override void OnButtonReleasedPointerInside()
        {
            base.OnButtonReleasedPointerInside();
            SetCurrentPinned();
        }

        public override TooltipContent GetTooltipContent()
        {
            return _getTooltip?.Invoke();
        }

        /// <summary>
        /// Updates the position of the button
        /// </summary>
        public void MovePos()
        {
            var slotObj = Resources.FindObjectsOfTypeAll<LegendaryRecipeItemSlot>().Where(s => s.name == this.machineSlot).First().gameObject;
            GameObject child = slotObj.transform.GetChild(2).gameObject;
            Vector3 targetPos = child.transform.localPosition;
            this.gameObject.transform.localPosition = targetPos;
        }

        /// <summary>
        /// Pins the potion recipe to the Lab
        /// Sets the current pinned with recipe name and list of effects
        /// </summary>
        public void SetCurrentPinned()
        {
            // Grab current legendary recipe
            LegendaryRecipe recipe = Utilities.Recipe.GetCurrentRecipe();

            // Get the desired effects from the slot
            DesiredItemEffect desired = Utilities.Recipe.GetDesired(this.machineSlot);

            // Set the text
            string text = Utilities.Effects.ToText(desired.effects.ToList());
            PinHolder.SetText(text);

            // Set current pinned
            List<string> effectsList = Utilities.Effects.ToList(desired.effects.ToList());
            Plugin.currentPinned = Tuple.Create(recipe.name, effectsList);
        }
    }
}