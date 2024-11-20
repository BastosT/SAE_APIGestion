using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_APIGestionTests.Controllers
{
    public abstract class BaseTest
    {
        protected GlobalDBContext Context;

        [TestInitialize]
        public void BaseInitialize()
        {
            // Configuration commune pour tous les tests
            var builder = new DbContextOptionsBuilder<GlobalDBContext>()
                .UseNpgsql("Server=localhost;port=5432;database=sae_rasp;uid=postgres;password=postgres");
            Context = new GlobalDBContext(builder.Options);
        }

    }
}
