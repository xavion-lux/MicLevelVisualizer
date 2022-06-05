﻿using MelonLoader;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MicLevel
{
    public static class BuildInfo
    {
        public const string Name = "MicLevel";
        public const string Author = "Xavi";
        public const string Version = "1.0.0";
        public const string DownloadLink = "https://github.com/xavion-lux/MicLevel/releases";
    }

    public class Mod : MelonMod
    {

        private GameObject slider;

        public static MelonPreferences_Entry<string> orientation;

        public override void OnApplicationLateStart()
        {
            base.OnApplicationLateStart();

            MelonPreferences.CreateCategory(BuildInfo.Name, BuildInfo.Name);
            orientation = MelonPreferences.CreateEntry<string>(BuildInfo.Name, "slider_orientation", "horizontal", "Set slider orientation");
            UIExpansionKit.API.ExpansionKitApi.RegisterSettingAsStringEnum(BuildInfo.Name, "slider_orientation", new[] { ("horizontal", "Horizontal"), ("vertical", "Vertical") });

            orientation.OnValueChanged += (_, val) => SetOrientation(val);

            MelonCoroutines.Start(MakeUI());
        }

        public IEnumerator MakeUI()
        {
            while(VRCUiManager.prop_VRCUiManager_0 == null || UnityEngine.Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>() == null) yield return null;

            // find slider reference
            var sliderRef = GameObject.Find("UserInterface").GetComponentInChildren<VRC.UI.Elements.QuickMenu>(true).field_Public_Transform_0.Find("Window/QMParent/Menu_AudioSettings/Content/Mic/InputLevel/Sliders/MicLevelSlider").gameObject;

            // smol slider if we don't wait here
            while(sliderRef.GetComponent<RectTransform>().sizeDelta.x == 0) yield return null;  

            // instantiating slider
            slider = GameObject.Instantiate(sliderRef, GameObject.Find("UserInterface/UnscaledUI/HudContent_Old/Hud").transform);
            try { GameObject.DestroyImmediate(slider.GetComponentInChildren<SliderBinding>()); } catch { } //optional
            SetOrientation(orientation.Value);

            // changing slider opacity
            var sliderImage = slider.transform.Find("Slider/Fill Area/Fill").GetComponent<Image>();
            sliderImage.color = new Color(sliderImage.color.r, sliderImage.color.g, sliderImage.color.b, 0.6f);
            var sliderBackgroundImage = slider.transform.Find("Slider/Background").GetComponent<Image>();
            sliderBackgroundImage.color = new Color(sliderBackgroundImage.color.r, sliderBackgroundImage.color.g, sliderBackgroundImage.color.b, 0.6f);

            // synchronize value between reference slider and our slider
            sliderRef.GetComponentInChildren<Slider>().onValueChanged.AddListener(new Action<float>((value) => slider.GetComponentInChildren<Slider>().value = value));
        }

        public void SetOrientation(string val)
        {
            if(val.ToLower() == "horizontal")
            {
                slider.transform.localScale = new Vector3(0.12f, 0.3f, 1f);
                slider.transform.localPosition = new Vector3(-363f, -470f, 0f);
                slider.transform.localRotation = new Quaternion(0, 0, 0, 1);
            }
            else
            {
                slider.transform.localScale = new Vector3(0.11f, 0.3f, 1f);
                slider.transform.localPosition = new Vector3(-418.5f, -415f, 0f);
                slider.transform.localRotation = new Quaternion(0, 0, 90, 1);
            }
        }
    }
}
