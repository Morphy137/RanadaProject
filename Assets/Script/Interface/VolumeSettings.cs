using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Script.Interface
{
    /// <summary>
    /// Controla la configuración de volumen del juego mediante sliders de UI.
    /// Sincroniza los valores con SoundManager y PlayerPrefs para persistencia.
    /// </summary>
    public class VolumeSettings : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Sliders de Volumen")]
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;

        [Header("Textos de Valores")]
        [SerializeField] private TextMeshProUGUI bgmValueText;
        [SerializeField] private TextMeshProUGUI sfxValueText;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// Inicializa los sliders con los valores guardados de volumen.
        /// </summary>
        private void Start()
        {
            InitializeSliders();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Configura los sliders con los valores actuales de volumen y establece los listeners.
        /// </summary>
        private void InitializeSliders()
        {
            if (SoundManager.Instance != null)
            {
                float bgmVolume = PlayerPrefs.GetFloat("BGM", 0.8f);
                float sfxVolume = PlayerPrefs.GetFloat("SFX", 0.8f);

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

        /// <summary>
        /// Callback cuando el slider de BGM cambia de valor.
        /// </summary>
        /// <param name="value">Nuevo valor del volumen</param>
        private void OnBGMVolumeChanged(float value)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.ChangeBGMVolume(value);
                UpdateBGMText(value);
            }
        }

        /// <summary>
        /// Callback cuando el slider de SFX cambia de valor.
        /// </summary>
        /// <param name="value">Nuevo valor del volumen</param>
        private void OnSFXVolumeChanged(float value)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.ChangeSFXVolume(value);
                UpdateSFXText(value);
            }
        }

        /// <summary>
        /// Actualiza el texto que muestra el porcentaje de volumen BGM.
        /// </summary>
        /// <param name="value">Valor del volumen (0-1)</param>
        private void UpdateBGMText(float value)
        {
            if (bgmValueText != null)
            {
                bgmValueText.text = $"{Mathf.RoundToInt(value * 100)}%";
            }
        }

        /// <summary>
        /// Actualiza el texto que muestra el porcentaje de volumen SFX.
        /// </summary>
        /// <param name="value">Valor del volumen (0-1)</param>
        private void UpdateSFXText(float value)
        {
            if (sfxValueText != null)
            {
                sfxValueText.text = $"{Mathf.RoundToInt(value * 100)}%";
            }
        }

        /// <summary>
        /// Limpia los listeners de los sliders al destruir el objeto.
        /// </summary>
        private void OnDestroy()
        {
            if (bgmSlider != null)
                bgmSlider.onValueChanged.RemoveListener(OnBGMVolumeChanged);

            if (sfxSlider != null)
                sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Refresca los valores de los sliders desde las preferencias guardadas.
        /// Útil para sincronizar cuando se abren paneles de configuración.
        /// </summary>
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
        #endregion
    }
}
