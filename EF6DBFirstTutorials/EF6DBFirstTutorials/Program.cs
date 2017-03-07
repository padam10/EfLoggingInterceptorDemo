using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6DBFirstTutorials
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*** Please debug code for better understanding ***");
            Console.WriteLine("");
            Console.WriteLine("*** Entity Framework 6 DB-First Tutorials Demo Start ***");
            Console.WriteLine("");

            EF6Demo.AddRange();
            EF6Demo.AsyncQueryAndSave();
            EF6Demo.DBCommandLogging();
            EF6Demo.DbCommandInterceptor();
            EF6Demo.TransactionSupport();
            Console.WriteLine("*** Entity Framework 6 DB-First Tutorials Demo Ends ***");

            Console.ReadKey();
        }

 

        

     
    }
}
