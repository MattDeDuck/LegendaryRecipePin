using BepInEx.Logging;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace LegendaryRecipePin
{
    public class PinHolder
    {
        static ManualLogSource Log => Plugin.Log;

        public static TextMeshPro pinHolderText;
        public static TMP_SubMesh pinHolderSubMesh;

        /// <summary>
        /// Creates the entire pinholder object for the Lab
        /// </summary>
        public static void MakePinHolder()
        {
            CreatePinHolder();
            AddPinHolderText();
            AddPinHolderMesh();
        }

        /// <summary>
        /// Create the pinholder object
        /// </summary>
        public static void CreatePinHolder()
        {
            var pinHolderObject = new GameObject("Legendary Pin Holder");

            var parentGO = GameObject.Find("Room Lab").transform;
            pinHolderObject.transform.parent = parentGO;

            var sr = pinHolderObject.AddComponent<SpriteRenderer>();

            var sg = pinHolderObject.AddComponent<SortingGroup>();
            sg.sortingLayerID = -1758066705;
            sg.sortingLayerName = "GuiBackground";

            pinHolderObject.layer = LayerMask.NameToLayer("UI");

            pinHolderObject.transform.position = new Vector3(0f, 0f, 0f);
            pinHolderObject.transform.localPosition = new Vector3(16.35f, 3.3f, 0f);

            pinHolderObject.SetActive(true);

            Log.LogInfo("PinHolder object created");
        }

        /// <summary>
        /// Add the text component to the pinholder
        /// </summary>
        public static void AddPinHolderText()
        {
            GameObject textHolder = new GameObject();

            GameObject parent = GameObject.Find("Legendary Pin Holder");
            if (parent is not null)
            {
                textHolder.transform.SetParent(parent.transform);
            }

            textHolder.name = "PinHolder Text";

            textHolder.transform.Translate(parent.transform.position);
            textHolder.layer = 5;

            pinHolderText = textHolder.AddComponent<TextMeshPro>();

            pinHolderText.fontSize = 6;

            pinHolderText.text = "";

            textHolder.SetActive(true);

            Log.LogInfo("PinHolder Text object created");
        }

        /// <summary>
        /// Update the mesh component to the pinholder to support icons
        /// </summary>
        public static void AddPinHolderMesh()
        {
            GameObject textHolder = new GameObject();

            GameObject parent = GameObject.Find("PinHolder Text");
            if (parent is not null)
            {
                textHolder.transform.SetParent(parent.transform);
            }

            textHolder.name = "PinHolder Mesh";

            pinHolderSubMesh = textHolder.AddComponent<TMP_SubMesh>();

            textHolder.SetActive(true);

            Log.LogInfo("PinHolder Mesh object created");
        }

        /// <summary>
        /// Clear the text of the pinholder
        /// </summary>
        public static void Clear()
        {
            GameObject ph = GameObject.Find("PinHolder Text");
            TextMeshPro textTMP = ph.GetComponent<TextMeshPro>();
            textTMP.text = "";
        }

        /// <summary>
        /// Update the position of the buttons
        /// </summary>
        public static void SetText(string text)
        {
            GameObject ph = GameObject.Find("PinHolder Text");
            TextMeshPro textTMP = ph.GetComponent<TextMeshPro>();
            textTMP.text = text;
        }
    }
}
