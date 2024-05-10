using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.Contracts.SendDTOs
{
    public class GetUserProfileDTO
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";


        public string Preferences { get; set; } = "";

        public byte[] Photo { get; set; } = [];

        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        public string SexId { get; set; } = "UnIdentify";
    }
}
