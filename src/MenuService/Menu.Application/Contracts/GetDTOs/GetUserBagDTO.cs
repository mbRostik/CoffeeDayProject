using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Application.Contracts.GetDTOs
{
    public class GetUserBagDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public int Count { get; set; }

        public float Price { get; set; }
    }
}
