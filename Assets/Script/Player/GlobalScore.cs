using UnityEngine;
using UnityEngine.UI;

namespace Script.Player
{
    /// <summary>
    /// Almacén global de estadísticas del jugador durante una partida.
    /// Mantiene datos estáticos accesibles desde cualquier script para el seguimiento
    /// de puntuación, combos, precisión y estado general del juego.
    /// </summary>
    public class GlobalScore : MonoBehaviour
    {
        #region Puntuación y Combos
        /// <summary>Puntuación total acumulada del jugador</summary>
        public static int score;

        /// <summary>Número total de combos conseguidos durante la partida</summary>
        public static int totalCombo;

        /// <summary>Combo actual activo del jugador</summary>
        public static int currentCombo;

        /// <summary>Combo más alto alcanzado en la partida</summary>
        public static int highestCombo;
        #endregion

        #region Estadísticas de Precisión
        /// <summary>Número de notas falladas por el jugador</summary>
        public static int missesHit;

        /// <summary>Número de golpes perfectos (máxima precisión)</summary>
        public static int perfectHits;

        /// <summary>Número de golpes excelentes (alta precisión)</summary>
        public static int greatHits;

        /// <summary>Número de golpes buenos (precisión media)</summary>
        public static int gooodHits;

        /// <summary>Número total de notas en la canción actual</summary>
        public static int totalNotes;
        #endregion

        #region Estado del Juego
        /// <summary>Indica si la canción ha comenzado y el juego está activo</summary>
        public static bool songStarted;
        #endregion
    }
}
