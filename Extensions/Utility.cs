using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestWebApiProject.Extensions
{
    public static class Utility
    {
        /// <summary>
        /// Validation rule :Starting with 04 or 09 and , should require 10 numbers      
        /// </summary>
        /// <param name="userMobile"></param>
        /// <returns></returns>
        public static bool ValidateMobileNumber(string userMobile)
        {
            bool isValidMobileNumber = false;
            if (!string.IsNullOrEmpty(userMobile))
                isValidMobileNumber = new Regex(@"^0(9|4)\d{8}$").IsMatch(userMobile);
            return isValidMobileNumber;
        }
    }
}
