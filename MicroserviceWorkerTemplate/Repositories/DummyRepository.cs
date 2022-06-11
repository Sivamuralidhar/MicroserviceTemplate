using MicroserviceWorkerTemplate.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceWorkerTemplate.Repositories
{
    public class DummyRepository : IDummyRepository
    {
        public void SavingDataHere()
        {
            throw new NotImplementedException();
        }
    }
}
