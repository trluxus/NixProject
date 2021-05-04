using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NixProject
{
    class Room
    {
        public int Number { get; set; }
        public decimal Price { get; set; }
        public int Capacity { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Room room &&
                   Number == room.Number;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Number);
        }
    }
}
