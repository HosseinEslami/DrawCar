using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Text coinAmount;

        private void OnEnable() => CurrencyManager.ONBalanceChanged += UpdateCoinUI;

        private void OnDisable() => CurrencyManager.ONBalanceChanged -= UpdateCoinUI;

        private void Start()
        {
            UpdateCoinUI();
        }

        private void UpdateCoinUI()
        {
            coinAmount.text = "Coin " + CurrencyManager.GetBalance("Coin");
        }
    }
}
