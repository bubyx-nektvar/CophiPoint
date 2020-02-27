using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.Helpers
{
    public class FatalApplicationFailException:Exception
    {
        public FatalApplicationFailException(string message, ErrorCodes code)
            :base(message)
        { }
    }
}
