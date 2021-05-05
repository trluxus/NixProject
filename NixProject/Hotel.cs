using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NixProject
{
    class Hotel
    {
        public HashSet<Room> Rooms { get; set; }
        public HashSet<Guest> Guests { get; set; }
        public Dictionary<Reservation, List<Guest>> Reservations { get; set; }

        public Hotel()
        {
            Rooms = new HashSet<Room>();
            Guests = new HashSet<Guest>();
            Reservations = new Dictionary<Reservation, List<Guest>>();
        }

        public void AddRoom(Room room) => Rooms.Add(room);

        public void RemoveRoom(Room room) => Rooms.Remove(room);

        public void AddGuest(Guest guest) => Guests.Add(guest);

        public void RemoveGuest(Guest guest) => Guests.Remove(guest);

        public void ReserveRoom(Reservation reservation, params Guest[] guests)
        {
            if (guests.Length > reservation.Room.Capacity)
                throw new ArgumentOutOfRangeException("Number of guests is more then capacity of the room.");

            if (!Rooms.Contains(reservation.Room)) { Rooms.Add(reservation.Room); }
            
            foreach(var g in guests)
                if (!Guests.Contains(g)) { Guests.Add(g); }

            Reservations.Add(reservation, guests.ToList());
        }

        public List<Room> EmptyRooms(DateTime date)
        {
            return Rooms.Except(Reservations.Where(i => i.Key.StartDate >= date && i.Key.EndDate <= date).Select(i => i.Key.Room)).ToList();
        }
        
        public List<Room> EmptyRooms(DateTime startDate, DateTime endDate)
        {
            return Rooms.Except(Reservations.Where(i => i.Key.StartDate <= endDate && i.Key.EndDate >= startDate).Select(i => i.Key.Room)).ToList();
        }

        public void EndReservation(Reservation reservation)
        {
            var l = Reservations[reservation];

            Reservations.Remove(reservation);

            foreach (var g in l)
                if (!Reservations.Values.SelectMany(i => i).Contains(g)) { RemoveGuest(g); }  
        }

        public void CheckIn(Reservation reservation, params Guest[] guests)
        {
            if (guests.Length > reservation.Room.Capacity)
                throw new ArgumentOutOfRangeException("Number of guests is more then capacity of the room.");

            if (Reservations.ContainsKey(reservation))
            {
                Reservations[reservation] = guests.ToList();
                Reservations.Keys.Where(i => i.Equals(reservation)).First().Status = ReservationStatus.Success;
            }
            else
            {
                ReserveRoom(reservation, guests);
                Reservations.Keys.Where(i => i.Equals(reservation)).First().Status = ReservationStatus.Success;
            }
        }

        public void CheckOut(Reservation reservation)
        {
            if (Reservations.ContainsKey(reservation))
            {
                Reservations.Keys.Where(i => i.Equals(reservation)).First().Status = ReservationStatus.Completed;
            }
        }
    }
}
