using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var conn = new SQLiteConnection("Data Source=mydb.db"))
            {
                conn.Open();
                var creatComm = conn.CreateCommand();

                creatComm.CommandText = @"
                                CREATE TABLE IF NOT EXISTS lekvar (
                                   id INTEGER  PRIMARY KEY  AUTOINCREMENT ,
                                   uveg INTEGER NOT NULL,
                                   tipus VARCHAR(1000) NOT NULL                                     
                                    )";
                creatComm.ExecuteNonQuery();
                Console.WriteLine("Adja meg hogy mekora üvegbe tete a lekvárt.");
                var meret = Console.ReadLine();
                Console.WriteLine("Adja meg hogy milyen lekvárt tipus");
                var tipus = Console.ReadLine();

                var inserComm = conn.CreateCommand();
                inserComm.CommandText = @"
INSERT INTO lekvar (uveg,tipus)
VALUES(@uveg,@tipus) ";

                inserComm.Parameters.AddWithValue("@uveg", meret);
                inserComm.Parameters.AddWithValue("@tipus", tipus);
                int erinttesor = inserComm.ExecuteNonQuery();
             

                var iszliter = conn.CreateCommand();
                iszliter.CommandText = @"
SELECT SUM(uveg)
FROM lekvar";

                var tipusLiter = conn.CreateCommand();
                tipusLiter.CommandText = @"
SELECT tipus, SUM(uveg)
FROM lekvar
GROUP BY tipus";

                var tipusAVg = conn.CreateCommand();
                tipusAVg.CommandText = @"SELECT avg(uveg) FROM lekvar";

                var select = conn.CreateCommand();
                select.CommandText = @"SELECT id,tipus,uveg FROM lekvar";

                using (var reader = select.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object o1 = reader["id"];
                        // int eid = reader.GetInt32(0);
                        string etipus = reader.GetString(1);
                        string eliter = reader.GetString(2);
                        Console.WriteLine("{0}. {1}-{2}", 1, etipus, eliter);
                    }
                }

                Console.WriteLine(erinttesor);
                Console.ReadKey();
            }
        }
    }
}
