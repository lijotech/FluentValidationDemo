using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApiProject.Extensions
{
    public class Enums
    {
        public enum GenderTypes
        {
            [Description("MALE")]
            MALE,
            [Description("FEMALE")]
            FEMALE,
            [Description("OTHER")]
            OTHER
        }
    }
}
