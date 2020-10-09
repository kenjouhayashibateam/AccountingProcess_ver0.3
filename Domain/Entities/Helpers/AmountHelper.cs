
namespace Domain.Entities.Helpers
{
    public class AmountHelper
    {
        public static string AmountWithUnit(int amount)
        {
            return $"{amount:N0} {Properties.Resources.Unit}";
        }
    }
}
