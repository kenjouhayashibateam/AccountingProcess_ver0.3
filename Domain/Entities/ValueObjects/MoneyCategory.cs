
namespace Domain.Entities.ValueObjects
{
    public class MoneyCategory
    {
        private readonly int Category;
        private int count;
        public string Name;

        public enum Denomination
        {
            OneYen,
            FiveYen,
            TenYen,
            FiftyYen,
            OneHundredYen,
            FiveHundredYen,
            OneThousandYen,
            FiveThousandYen,
            TenThousandYen,
            OneYenBundle,
            FiveYenBundle,
            TenYenBundle,
            FiftyYenBundle,
            OneHundredYenBundle,
            FiveHundredYenBundle
        }

        public MoneyCategory(int category)
        {
            Category = category;
        }

        public int Count { set => count = value; }
        public int Amount { get => Category * count; }

    }
}
