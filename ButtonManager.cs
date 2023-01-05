using BepInEx.Logging;
using PotionCraft.ObjectBased.UIElements.Tooltip;

namespace LegendaryRecipePin
{
    public class ButtonManager
    {
        static ManualLogSource Log => Plugin.Log;

        // Machine level 1
        public static PinButton RightRetortItemSlotButton;
        public static PinButton RightDripperItemSlotButton;
        public static PinButton RhombusVesselItemSlotButton;
        public static PinButton TriangularVesselItemSlotButton;
        public static PinButton DoubleVesselItemSlotButton;

        // Machine level 2
        public static PinButton SpiralVesselItemSlotButton;
        public static PinButton LeftRetortItemSlotButton;
        public static PinButton LeftDripperItemSlotButton;
        public static PinButton FloorVesselItemSlotButton;

        // Machine level 3
        public static PinButton TripletVesselLeftItemSlotButton;
        public static PinButton TripletVesselCenterItemSlotButton;
        public static PinButton TripletVesselRightItemSlotButton;

        /// <summary>
        /// Creates the buttons for the 12 alchemy machine recipe slots
        /// </summary>
        public static void CreateMachineButtons()
        {
            Log.LogInfo("Creating Buttons");

            RightRetortItemSlotButton = PinButton.Create<PinButton>("RightRetortItemSlotButton", Utilities.pinButtonBlankTex, PinToolTip, "RightRetort");
            RightDripperItemSlotButton = PinButton.Create<PinButton>("RightDripperItemSlotButton", Utilities.pinButtonBlankTex, PinToolTip, "RightDripper");
            RhombusVesselItemSlotButton = PinButton.Create<PinButton>("RhombusVesselItemSlotButton", Utilities.pinButtonBlankTex, PinToolTip, "RhombusVessel");
            TriangularVesselItemSlotButton = PinButton.Create<PinButton>("TriangularVesselItemSlotButton", Utilities.pinButtonBlankTex, PinToolTip, "TriangularVessel");
            DoubleVesselItemSlotButton = PinButton.Create<PinButton>("DoubleVesselItemSlotButton", Utilities.pinButtonBlankTex, PinToolTip, "DoubleVessel");

            SpiralVesselItemSlotButton = PinButton.Create<PinButton>("SpiralVesselItemSlotButton", Utilities.pinButtonBlankTex, PinToolTip, "SpiralVessel");
            LeftRetortItemSlotButton = PinButton.Create<PinButton>("LeftRetortItemSlotButton", Utilities.pinButtonBlankTex, PinToolTip, "LeftRetort");
            LeftDripperItemSlotButton = PinButton.Create<PinButton>("LeftDripperItemSlotButton", Utilities.pinButtonBlankTex, PinToolTip, "LeftDripper");
            FloorVesselItemSlotButton = PinButton.Create<PinButton>("FloorVesselItemSlotButton", Utilities.pinButtonBlankTex, PinToolTip, "FloorVessel");

            TripletVesselLeftItemSlotButton = PinButton.Create<PinButton>("TripletVesselLeftItemSlotButton", Utilities.pinButtonBlankTex, PinToolTip, "TripletVesselLeft");
            TripletVesselCenterItemSlotButton = PinButton.Create<PinButton>("TripletVesselCenterItemSlotButton", Utilities.pinButtonBlankTex, PinToolTip, "TripletVesselCenter");
            TripletVesselRightItemSlotButton = PinButton.Create<PinButton>("TripletVesselRightItemSlot", Utilities.pinButtonBlankTex, PinToolTip, "TripletVesselRight");
        }

        /// <summary>
        /// Tooltip used for the buttons upon mouseover
        /// </summary>
        public static TooltipContent PinToolTip()
        {
            return new TooltipContent
            {
                header = "Pin Potion in the Lab"
            };
        }
    }
}
