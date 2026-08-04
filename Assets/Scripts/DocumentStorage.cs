using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class DocumentStorage
    {
        public int DocumentsCapacity { get; private set; } = 20;
        public int DocumentsSpawnTime { get; private set; } = 5;
        public int CurrentDocumentsCount { get; private set; }
        public bool IsDocumentsNull => CurrentDocumentsCount <= 0;

        public const int DocumentsInOneStack = 10;

        public event Action<int, int> DocumentsUpdated;

        public void AddDocument()
        {
            if (CurrentDocumentsCount >= DocumentsCapacity) return;

            CurrentDocumentsCount++;

            DocumentsUpdated?.Invoke(CurrentDocumentsCount, DocumentsCapacity);
        }

        public bool RemoveDocument()
        {
            if (IsDocumentsNull) return false;

            CurrentDocumentsCount--;

            DocumentsUpdated?.Invoke(CurrentDocumentsCount, DocumentsCapacity);

            return true;
        }
    }
}