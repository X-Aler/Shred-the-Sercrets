using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class HudScreenController : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyText;
        [SerializeField] private TMP_Text documentText;
        [SerializeField] private TMP_Text timerText;

        private MoneyUI moneyUI;
        private DocumentUI documentUI;
        private DocumentTimerUI documentTimerUI;

        public void Initialize()
        {
            moneyUI = new MoneyUI(moneyText);
            documentUI = new DocumentUI(documentText);
            documentTimerUI = new DocumentTimerUI(timerText);
        }

        private void OnDestroy()
        {
            moneyUI?.Dispose();
            documentUI?.Dispose();
            documentTimerUI?.Dispose();
        }
    }
}