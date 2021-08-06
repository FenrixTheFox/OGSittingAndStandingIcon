using MelonLoader;
using OGSittingAndStandingIcon;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;

[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonInfo(typeof(OGSittingAndStandingIconMod), "OG Sitting And Standing Icon", "1.0.2", "Fenrix")]

namespace OGSittingAndStandingIcon
{
    public class OGSittingAndStandingIconMod : MelonMod
    {
        private string BaseTogglePath = "UserInterface/QuickMenu/ShortcutMenu/SitButton/Toggle_States_StandingEnabled";

        public override void OnApplicationStart()
        {
            MelonCoroutines.Start(WaitForUiManagerInit());
        }

        private IEnumerator WaitForUiManagerInit()
        {
            while (VRCUiManager.prop_VRCUiManager_0 == null) yield return null;

            // Load our Asset(s)
            AssetBundle assetBundle;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("OGSittingAndStandingIcon.ogsittingandstandingicon.assetbundle"))
            using (var tempStream = new MemoryStream((int)stream.Length))
            {
                stream.CopyTo(tempStream);

                assetBundle = AssetBundle.LoadFromMemory_Internal(tempStream.ToArray(), 0);
                assetBundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            }

            InstantiateIcon(BaseTogglePath + "/ON", "Seated Play", assetBundle.LoadAsset_Internal("Sitting", Il2CppType.Of<Sprite>()).Cast<Sprite>());
            InstantiateIcon(BaseTogglePath + "/OFF", "Standing Play", assetBundle.LoadAsset_Internal("Standing", Il2CppType.Of<Sprite>()).Cast<Sprite>());
        }

        private void InstantiateIcon(string path, string text, Sprite replacementSprite)
        {
            GameObject gameObject = GameObject.Find(path);
            GameObject calibrateObject = GameObject.Find("UserInterface/QuickMenu/ShortcutMenu/CalibrateButton");

            if (gameObject != null && calibrateObject != null)
            {
                // Strip the background
                GameObject.DestroyImmediate(gameObject.GetComponent<Image>(), true);

                foreach (var transform in gameObject.GetComponentsInChildren<Transform>())
                {
                    if (transform.gameObject != gameObject)
                        GameObject.DestroyImmediate(transform.gameObject, true);
                }

                foreach (var transform in calibrateObject.transform)
                {
                    GameObject.Instantiate(transform.Cast<Transform>(), gameObject.transform);
                }

                // Move and change text
                Text textComp = gameObject.GetComponentInChildren<Text>();
                Transform textTransform = textComp.transform;

                textTransform.localPosition = new Vector3(0f, 140.5f, 0f);
                textTransform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                textComp.text = text;

                Image imageComp = gameObject.GetComponentInChildren<Image>();
                Transform imageTransform = imageComp.transform;

                imageTransform.localPosition = new Vector3(0f, 0f, 0f);
                imageTransform.localScale = new Vector3(1f, 1f, 1f);

                imageComp.sprite = replacementSprite;
            }
        }
    }
}
