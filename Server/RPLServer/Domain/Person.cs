using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Person
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public decimal Wealth { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<Payment> Payments { get; set; }
        public List<Achievement> Achievements { get; set; }
    }
}
