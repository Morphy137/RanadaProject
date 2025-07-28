using UnityEngine;
using UnityEngine.UI;
using Script.Interface;

namespace Script.Interface
{
    public class VolumeSettings : MonoBehaviour
    {
        [Header("Sliders de Volumen")]
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;

        [Header("Textos de Valores (Opcional)")]
        [SerializeField] private TMPro.TextMeshProUGUI bgmValueText;
        [SerializeField] private TMPro.TextMeshProUGUI sfxValueText;

        private void Start()
        {
            InitializeSliders();
        }

        private void InitializeSliders()
        {
            // Obtener valores actuales del SoundManager
            if (SoundManager.Instance != null)
            {
                float bgmVolume = PlayerPrefs.GetFloat("BGM", 0.8f);
                float sfxVolume = PlayerPrefs.GetFloat("SFX", 0.8f);

                // Configurar sliders si están asignados
                if (bgmSlider != null)
                {
                    bgmSlider.value = bgmVolume;
                    bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
                    UpdateBGMText(bgmVolume);
                }

                if (sfxSlider != null)
                {
                    sfxSlider.value = sfxVolume;
                    sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
                    UpdateSFXText(sfxVolume);
                }
            }
            else
            {
                Debug.LogWarning("SoundManager not found. Volume settings may not work properly.");
            }
        }

        private void OnBGMVolumeChanged(float value)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.ChangeBGMVolume(value);
                UpdateBGMText(value);
            }
        }

        private void OnSFXVolumeChanged(float value)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.ChangeSFXVolume(value);
                UpdateSFXText(value);
            }
        }

        private void UpdateBGMText(float value)
        {
            if (bgmValueText != null)
            {
                bgmValueText.text = $"{Mathf.RoundToInt(value * 100)}%";
            }
        }

        private void UpdateSFXText(float value)
        {
            if (sfxValueText != null)
            {
                sfxValueText.text = $"{Mathf.RoundToInt(value * 100)}%";
            }
        }

        private void OnDestroy()
        {
            // Cleanup listeners
            if (bgmSlider != null)
                bgmSlider.onValueChanged.RemoveListener(OnBGMVolumeChanged);

            if (sfxSlider != null)
                sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
        }

        // Método público para refrescar los valores desde otros scripts
        public void RefreshValues()
        {
            if (SoundManager.Instance != null)
            {
                float bgmVolume = PlayerPrefs.GetFloat("BGM", 0.8f);
                float sfxVolume = PlayerPrefs.GetFloat("SFX", 0.8f);

                if (bgmSlider != null)
                {
                    bgmSlider.value = bgmVolume;
                    UpdateBGMText(bgmVolume);
                }

                if (sfxSlider != null)
                {
                    sfxSlider.value = sfxVolume;
                    UpdateSFXText(sfxVolume);
                }
            }
        }
    }
}
