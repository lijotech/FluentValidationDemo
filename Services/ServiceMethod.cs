using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApiProject.Services
{
    public class ServiceMethod: IServiceMethod
    {
        public  Task<bool> ValidateIdentificationNumber(string identificationNumber)
        {
            return Task.FromResult( identificationNumber=="123456789");
        }
    }
}
