using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Domain
{
    public class Product
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }

        public byte[] Photo { get; set; }

        public List<CategoryWithProduct> CategoryWithProducts { get; set; }

        public List<Bag> Bags { get; set; }

    }
}
