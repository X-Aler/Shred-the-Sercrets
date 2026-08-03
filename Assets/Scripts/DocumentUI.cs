using System;
using TMPro;

namespace DefaultNamespace
{
    public class DocumentUI : IDisposable
    {
        private TMP_Text documentText;
        private DocumentStorage storage;
        public DocumentUI(TMP_Text documentText)
        {
            this.documentText = documentText;

            storage = GameServices.Instance.DocumentStorage;

            UpdateUI(storage.CurrentDocumentsCount, storage.DocumentsCapacity);

            storage.DocumentsUpdated += UpdateUI;

        }
        private void UpdateUI(int currentCount, int capacity)
        {
            documentText.text = $"Количество - {currentCount}/{capacity}";
        }

        public void Dispose()
        {
            if (storage == null) return;

            storage.DocumentsUpdated -= UpdateUI;
        }
    }
}