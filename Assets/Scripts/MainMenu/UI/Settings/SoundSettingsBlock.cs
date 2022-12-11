﻿using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace GameJamEntry.MainMenu.UI {
	public class SoundSettingsBlock : GameComponent {
		[NotNull] [SerializeField] Slider Slider;

		SystemSettingsController _controller;
		MixerParamName           _mixerParamName;
		
		public void Init(SystemSettingsController settingsController, MixerParamName paramName) {
			_mixerParamName = paramName;
			_controller     = settingsController;
			Slider.onValueChanged.RemoveAllListeners();
			Slider.onValueChanged.AddListener(OnSliderValueChanged);
			Slider.value = _controller.GetNormalizedVolume(_mixerParamName);
		}

		void OnSliderValueChanged(float value) {
			_controller.SetNormalizedVolume(_mixerParamName, value);	
		}
	}
}