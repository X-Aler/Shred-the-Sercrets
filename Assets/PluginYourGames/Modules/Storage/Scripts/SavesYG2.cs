// Файл Assets/PluginYourGames/Modules/Storage/Scripts/SavesYG.cs
namespace YG
{
    [System.Serializable]
    public partial class SavesYG
    {
        public int idSave;

        // Ваши данные для сохранения:
        public int coins = 0;
        public int[] upgradeLevels = new int[6];
    }
}