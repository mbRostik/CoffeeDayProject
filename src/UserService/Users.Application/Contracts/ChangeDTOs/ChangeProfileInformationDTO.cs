using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.Contracts.ChangeDTOs
{
    public class ChangeProfileInformationDTO
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";

        public string Preferences { get; set; } = "";

        public DateTime DateOfBirth { get; set; } = DateTime.Now;
    }
}
