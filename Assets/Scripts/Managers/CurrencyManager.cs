using System;
using UnityEngine;

namespace Managers
{
    public class CurrencyManager : MonoBehaviour
    {
        public const string Coin = "Coin";
        public const string Gem = "Gem";
        public const int CoinStartAmount = 10;
        public const int GemStartAmount = 1;
        public static Action ONBalanceChanged;
        public static Action<int> ONBalanceIncreased;
        public static Action<int> ONBalanceDecreased;

        public static int GetBalance(string type)
        {
            int balance = 0;
            switch (type)
            {
                case "Coin":
                    balance = PlayerPrefs.GetInt(Coin, CoinStartAmount);
                    break;
                case "Gem":
                    balance = PlayerPrefs.GetInt(Gem, GemStartAmount);
                    break;
            }

            return balance;
        }

        public static void SetBalance(int value, string type)//bool saveToCloud = true)
        {
            //playerPrefs
            switch (type)
            {
                case "Coin":
                    PlayerPrefs.SetInt(Coin, value);
                    break;
                case "Gem":
                    PlayerPrefs.SetInt(Gem, value);
                    break;
            }

            //google Save
            // if(!saveToCloud || !CloudSaveManager.Instance.cloudSave.loggedInToGoogle) return;
            // CloudSaveManager.Instance.State.coinAmount = GetBalance();
            // CloudSaveManager.Instance.cloudSave.Save();
        }

        public static void Earn(int value, string type)//, bool saveToCloud = true)
        {
            int current = GetBalance(type);
            SetBalance(current + value, type);

            ONBalanceChanged?.Invoke();
            ONBalanceIncreased?.Invoke(value);

            //Game Analytics Event
            //GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "coin", value,"earn " + type , "1");
        }

        public static bool Spend(int value, string type)
        {
            int current = GetBalance(type);
            if (current < value) return false;

            SetBalance(current - value, type);
            if (ONBalanceChanged != null) ONBalanceChanged();
            if (ONBalanceDecreased != null) ONBalanceDecreased(value);
        
            //Game Analytics Event
            //GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "coin", value,"spend " + type,"1");
        
            return true;
        }
    }
}
