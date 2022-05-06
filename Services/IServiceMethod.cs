using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApiProject.Services
{
    public interface IServiceMethod
    {
        Task<bool> ValidateIdentificationNumber(string identificationNumber);
    }
}
