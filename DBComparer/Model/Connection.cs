using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBComparer.Model
{

    public class DataBaseComparer
    {
        public DataBaseComparer()
        {
            databaseA = new Database();
            databaseB = new Database();
        }
        public Database databaseA { get; set; }
        public Database databaseB { get; set; }
    }
    public class Connection
    {
        public string name { get; set; }
        public string connection { get; set; }
    }


    public class Database
    {
        public Connection connection;
        public List<KeyValue> storedProcedures { get; set; }
        public List<KeyValue> tables { get; set; }
    }
    public class KeyValue
    {
        public string definition { get; set; }
        public string name { get; set; }
    }

    public class StringCompare
    {
        public string field { get; set; }
        public string difA { get; set; }
        public string difB { get; set; }
    }

    public class AppData
    {
        public int Age { get; set; }
    }
}

