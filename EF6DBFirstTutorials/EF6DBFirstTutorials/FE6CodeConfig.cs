using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6DBFirstTutorials
{
    

    public class FE6CodeConfig : DbConfiguration
    {
        public FE6CodeConfig()
        {
           
            //this.SetDefaultConnectionFactory(new System.Data.Entity.Infrastructure.SqlConnectionFactory());
            //this.SetProviderServices("System.Data.SqlClient", System.Data.Entity.SqlServer.SqlProviderServices.Instance);

            //following is for code first only
            //this.SetDatabaseInitializer<SchoolDBEntities>(new  CreateDatabaseIfNotExists<SchoolDBEntities>());

            //this.AddInterceptor(new EFCommandInterceptor());
            //this.SetPluralizationService(new MyPluralizationService());
        }

        
    }
}










            //this.AddInterceptor(new EFCommandInterceptor());
