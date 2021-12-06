using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Text coinAmount;
        [SerializeField] private GameObject winImage, boardPanel, board;


        private void Start()
        {
            UpdateCoinUI();
            CurrencyManager.ONBalanceChanged += UpdateCoinUI;
            GameManager.Instance.gameOver.AddListener(Win);
        }

        private void OnDisable()
        {
            CurrencyManager.ONBalanceChanged -= UpdateCoinUI;
            GameManager.Instance.gameOver.RemoveListener(Win);
        }

        private void UpdateCoinUI()
        {
            coinAmount.text = "Gas " + CurrencyManager.GetBalance("Coin");
        }

        private void Win()
        {
            winImage.SetActive(true);
            board.SetActive(false);
            boardPanel.SetActive(false);
            Camera.main.GetComponent<Animator>().SetBool("Win", true);
        }
    }
}
