using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.Contracts.GetDTOs
{
    public class GetUserOrdersDTO
    {
        public int Id { get; set; }

        public float Summ { get; set; }

        public DateTime Date { get; set; }
    }
}
