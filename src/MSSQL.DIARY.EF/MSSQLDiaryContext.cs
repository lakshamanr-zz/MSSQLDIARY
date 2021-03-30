using Microsoft.EntityFrameworkCore;
using MSSQL.DIARY.COMN.Models;

namespace MSSQL.DIARY.EF
{
    public partial class MssqlDiaryContext
    {
        public static bool IsAlreadyLogin;

        public MssqlDiaryContext(string aIstrDbConnections = null)
        {
            IstrDbConnections = aIstrDbConnections;
        }

        public MssqlDiaryContext(DbContextOptions<MssqlDiaryContext> options) : base(options)
        {
        }

        public static ServerLogin LoginDetails { get; set; }

        public string IstrDbConnections { get; set; }

        public string GetDatabaseName
        {
            get
            {
                using (var con = Database.GetDbConnection())
                {
                    return con.Database;
                }
            }
        }
        public string GetServerName 
        {
            get 
            {
                return IstrDbConnections?.Split(';')[0].Replace("Data Source =", "");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if (!optionsBuilder.IsConfigured)
            {
                try
                {
                       if (IstrDbConnections.IsNullOrWhiteSpace())
                        {
                          //  optionsBuilder.UseSqlServer($"Server=DESKTOP-NFUD15G\\SQLEXPRESS;Database=AdventureWorks2016;User Id=mssql; Password=mssql;Trusted_Connection=false;");
                        }
                        else 
                        {
                            optionsBuilder.UseSqlServer(IstrDbConnections);
                        } 
                }
                catch (System.Exception)
                { 
                }
              }
        }
    }
}