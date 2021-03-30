using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSSQL.DIARY.UI.APP.Models
{
    public class DatabaseServerDetails
    { 
        public int ID { get; set; }
        public string ServerName { get; set; }
        public string DatabaseConnection { get; set; }
    }
}
