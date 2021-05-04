using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NixProject
{
    class Guest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime BirthDate { get; set; }
        public string ResidenceAddress { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Guest guest &&
                   Name == guest.Name &&
                   Surname == guest.Surname &&
                   Patronymic == guest.Patronymic;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Surname, Patronymic);
        }
    }
}
