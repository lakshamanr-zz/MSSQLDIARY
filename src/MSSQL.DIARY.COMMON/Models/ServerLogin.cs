namespace MSSQL.DIARY.COMN.Models
{
    public class ServerLogin
    {
        public string istrServerName { get; set; }
        public string istrDatabaseName { get; set; }
        public string istrUserName { get; set; }
        public string istrPassword { get; set; }
        public bool iblnIsLogin { get; set; }
        public string currentUser { get; set; }
        public bool iblnIsAdmin { get; set; }
    }
}