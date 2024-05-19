using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Domain
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] Photo { get; set; }

        public List<CategoryWithProduct> CategoryWithProducts { get; set; }
    }
}
