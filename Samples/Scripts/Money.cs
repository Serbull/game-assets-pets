
namespace Serbull.GameAssets.Pets.Samples
{
    public class Money : ICurrency
    {
        private readonly SaveData _saveData;

        public Money()
        {
            _saveData = GameManager.Instance.SaveData;
        }

        public long Amount => _saveData.Money;

        public void Add(long amount)
        {
            _saveData.Money += (int)amount;
        }

        public void Spend(long amount)
        {
            _saveData.Money -= (int)amount;
        }
    }
}
