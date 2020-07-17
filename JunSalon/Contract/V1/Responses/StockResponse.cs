using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.V1.Responses
{
    public class StockResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
    }
}
