using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Updates and displays the player's current coin total in the UI.
    /// </summary>
    public class CoinUI : MonoBehaviour
    {
        /// <summary>
        /// Text element used to show the coin count.
        /// </summary>
        [SerializeField] private TMP_Text coinText;

        /// <summary>
        /// Tracks the current number of collected coins.
        /// </summary>
        private int _coinCount;

        /// <summary>
        /// Increments the coin count by one and refreshes the UI text.
        /// </summary>
        public void AddCoin()
        {
            _coinCount++;
            coinText.SetText($"Coins: {_coinCount}");
        }
    }
}