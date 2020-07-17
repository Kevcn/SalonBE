using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.V1.Requests
{
    public class UpdateStockRequest
    {
        public string Name { get; set; }
        public string Company { get; set; }
    }
}
