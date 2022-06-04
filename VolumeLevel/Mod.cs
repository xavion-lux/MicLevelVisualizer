using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MicLevel
{
    public class Mod : MelonMod
    {
        public override void OnApplicationLateStart()
        {
            base.OnApplicationLateStart();

            MelonCoroutines.Start(WaitForUI());
        }

        public IEnumerator WaitForUI()
        {
            while(VRCUiManager.prop_VRCUiManager_0 == null || UnityEngine.Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>() == null)
            {
                yield return null;
            }

            var sliderRef = GameObject.Find("UserInterface").GetComponentInChildren<VRC.UI.Elements.QuickMenu>(true).field_Public_Transform_0.Find("Window/QMParent/Menu_AudioSettings/Content/Mic/InputLevel/Sliders/MicLevelSlider").gameObject;

            while(sliderRef.GetComponent<RectTransform>().sizeDelta.x == 0) yield return null;

            var slider = GameObject.Instantiate(sliderRef, GameObject.Find("UserInterface/UnscaledUI/HudContent_Old/Hud").transform);

            try  { GameObject.DestroyImmediate(slider.GetComponentInChildren<SliderBinding>()); } catch { } //optional

            slider.transform.localScale = new Vector3(0.12f, 0.3f, 1f);
            slider.transform.localPosition = new Vector3(-363f, -470f, 0f);

            //var sliderRefImage = sliderRef.transform.Find("Slider/Fill Area/Fill").GetComponent<Image>();
            var sliderImage = slider.transform.Find("Slider/Fill Area/Fill").GetComponent<Image>();
            sliderImage.color = new Color(sliderImage.color.r, sliderImage.color.g, sliderImage.color.b, 0.6f);

            var sliderBackgroundImage = slider.transform.Find("Slider/Background").GetComponent<Image>();
            sliderBackgroundImage.color = new Color(sliderBackgroundImage.color.r, sliderBackgroundImage.color.g, sliderBackgroundImage.color.b, 0.6f);

            var sliderComp = slider.GetComponentInChildren<Slider>();
            sliderRef.GetComponentInChildren<Slider>().onValueChanged.AddListener(new Action<float>((value) =>
            {
                sliderComp.value = value;
                //sliderImage.color = sliderRefImage.color;
            }));
        }
    }
}
