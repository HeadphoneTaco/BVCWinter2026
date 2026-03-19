using TMPro;
using UnityEngine;

namespace UI
{
    public class CoinUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinText;
        private int _coinCount = 0;

        public void AddCoin()
        {
            _coinCount++;
            coinText.SetText($"Coins: {_coinCount}");
        }
    }
}