using CophiPoint.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CophiPoint.Api
{
    public class DataConflictException : Exception
    {
        public PurchaseOrder Proposal { get; private set; }

        public DataConflictException(PurchaseOrder proposal)
        {
            this.Proposal = proposal;
        }
    }
}
