using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class DocumentStorage
    {
        public int DocumentsCapacity { get; private set; } = 20;
        public int DocumentsSpawnTime { get; private set; } = 30;
        public int CurrentDocumentsCount { get; private set; }

        public const int DocumentsInOneStack = 10;

        public event Action<int, int> DocumentsUpdated;

        public void AddDocument()
        {
            if (CurrentDocumentsCount >= DocumentsCapacity) return;

            CurrentDocumentsCount++;

            DocumentsUpdated?.Invoke(CurrentDocumentsCount, DocumentsCapacity);
        }

        public void RemoveDocument()
        {
            if (CurrentDocumentsCount <= 0) return;

            CurrentDocumentsCount--;

            DocumentsUpdated?.Invoke(CurrentDocumentsCount, DocumentsCapacity);
        }
    }
}