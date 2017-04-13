namespace Rrs.DataAccess.Database
{
    public static class ConnectionStrings
    {
        public static string WindowsAutoConnectionString(string database) => $"Data Source=localhost;Initial Catalog={database};Integrated Security=SSPI;";
        public static string WindowsAutoConnectionString(string server, string database) => $"Data Source={server};Initial Catalog={database};Integrated Security=SSPI;";
        public static string SqlAuthConnectionString(string server, string database, string user, string password) => $"Data Source={server};Initial Catalog={database};UID={user};pwd={password}";
    }
}
