using Domain.Repositories;

namespace Infrastructure
{
    public static class DefaultInfrastructure
    {
        public static IDataOutput GetDefaultDataOutput()
        {
            return new ExcelOutputInfrastructure();
        }

        public static IDataBaseConnect GetDefaultDataBaseConnect()
        {
            return new SQLServerConnectInfrastructure();
        }
    }
}
