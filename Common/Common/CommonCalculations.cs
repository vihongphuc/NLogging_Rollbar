using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class CommonCalculations
    {
        public static decimal ValidTurnOver(decimal amount, decimal result)
        {
            if (amount == result)
                return 0;

            return amount;
        }
    }
}
