using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.Contracts.GetDTOs
{
    public class GetUserOrderDetailsDTO
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public byte[] ProductPhoto { get; set; }
        public float ProductPrice { get; set; }

        public int Count { get; set; }
    }
}
