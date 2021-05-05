using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NixProject
{
    class Reservation
    {
        public Room Room { get; set; }
        private DateTime startDate;
        private DateTime endDate;
        public ReservationStatus Status { get; set; }

        public DateTime StartDate
        {
            get { return startDate; }
            private set
            {
                if (value < DateTime.UtcNow)
                    throw new ArgumentOutOfRangeException($"{nameof(value)} must be not today.");

                startDate = value;
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            private set
            {
                if(StartDate == default)
                    throw new ArgumentException("Start date must be set before end date.");
                if (value < StartDate)
                    throw new ArgumentOutOfRangeException($"{nameof(value)} must be later then start date.");

                endDate = value;
            }
        }

        public Reservation(Room room, DateTime startDate, DateTime endDate)
        {
            this.Room = room;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Status = ReservationStatus.Waiting;
        }
    }
}
