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
        public List<Reservation> Reservations { get; set; }

        public Hotel()
        {
            Rooms = new HashSet<Room>();
            Guests = new HashSet<Guest>();
            Reservations = new List<Reservation>();
        }

        public void AddRoom(Room room) => Rooms.Add(room);

        public void RemoveRoom(Room room) => Rooms.Remove(room);

        public void AddGuest(Guest guest) => Guests.Add(guest);

        public void RemoveGuest(Guest guest) => Guests.Remove(guest);

        public void ReserveRoom(Room room, DateTime startDate, params Guest[] guests)
        {
            if (!Rooms.Contains(room)) { Rooms.Add(room); }
            
            foreach(var g in guests)
                if (!Guests.Contains(g)) { Guests.Add(g); }

            Reservations.Add(new Reservation(room, startDate, startDate, guests));
        }

        public void ReserveRoom(Room room, DateTime startDate, DateTime endDate, params Guest[] guests)
        {
            if (!Rooms.Contains(room)) { Rooms.Add(room); }

            foreach (var g in guests)
                if (!Guests.Contains(g)) { Guests.Add(g); }

            Reservations.Add(new Reservation(room, startDate, endDate, guests));
        }

        public Room[] EmptyRooms(DateTime date)
        {
            return Rooms.Except(Reservations.Where(i => i.StartDate >= date && i.EndDate <= date).Select(i => i.Room)).ToArray();
        }
        
        public Room[] EmptyRooms(DateTime startDate, DateTime endDate)
        {
            return Rooms.Except(Reservations.Where(i => i.StartDate <= endDate && i.EndDate >= startDate).Select(i => i.Room)).ToArray();
        }

        public void EndReservation(Reservation reservation)
        {
            Reservations.Remove(reservation);

            foreach (var g in reservation.Guests)
            {
                if (!Reservations.SelectMany(i => i.Guests).Contains(g)) { RemoveGuest(g); }
            }
        }
    }
}
