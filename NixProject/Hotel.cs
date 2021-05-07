using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NixProject
{
    [Serializable]
    class Hotel
    {
        public HashSet<Room> Rooms { get; set; }
        public HashSet<Guest> Guests { get; set; }
        public Dictionary<Reservation, List<int>> Reservations { get; set; }

        public Hotel()
        {
            if (File.Exists("Rooms.dat")) {
                FileStream fs = new FileStream("Rooms.dat", FileMode.Open);
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    Rooms = (HashSet<Room>)formatter.Deserialize(fs);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }
            else 
                Rooms = new HashSet<Room>();

            if (File.Exists("Guests.dat")) {
                FileStream fs = new FileStream("Guests.dat", FileMode.Open);
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    Guests = (HashSet<Guest>)formatter.Deserialize(fs);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }
            else
                Guests = new HashSet<Guest>();

            if (File.Exists("Reservations.dat")) {
                FileStream fs = new FileStream("Reservations.dat", FileMode.Open);
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    Reservations = (Dictionary<Reservation, List<int>>)formatter.Deserialize(fs);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }
            else
                Reservations = new Dictionary<Reservation, List<int>>();
            
            
        }

        public void AddRoom(Room room)
        {
            Rooms.Add(room);
            RoomsTrasaction();
        }

        public void RemoveRoom(Room room)
        {
            var del = Reservations.Keys.Where(i => i.RoomNumber == room.Number);

            foreach (var r in del)
                Reservations.Remove(r);

            ReservationsTrasaction();

            Rooms.Remove(room);
            RoomsTrasaction();
        }

        public void AddGuest(Guest guest)
        {
            Guests.Add(guest);
            GuestsTrasaction();
        }

        public void RemoveGuest(Guest guest)
        {
            var del = Reservations.Where(i => i.Value.Contains(guest.ID));

            foreach (var r in del)
                Reservations[r.Key].Remove(guest.ID);

            ReservationsTrasaction();

            Guests.Remove(guest);
            GuestsTrasaction();
        }

        public void ReserveRoom(Reservation reservation, params int[] guests)
        {
            if (!Rooms.Select(i => i.Number).Contains(reservation.RoomNumber))
                throw new Exception("Selected for reservation room doesn`t exist.");

            if (guests.Length > Rooms.Where(i => i.Number == reservation.RoomNumber).Select(i => i.Capacity).First())
                throw new ArgumentOutOfRangeException("Number of guests is more then capacity of the room.");

            if (Reservations.Where(i => i.Key.RoomNumber == reservation.RoomNumber).
                Where(i => i.Key.StartDate > reservation.EndDate || i.Key.EndDate < reservation.StartDate).Any())
                throw new Exception("Proposed reservation for selected room intersets with existing reservation for this room.");

            foreach (var g in guests)
                if (!Guests.Select(i => i.ID).Contains(g))
                    throw new Exception("Proposed guest for reservation doesn`t exist.");

            Reservations.Add(reservation, guests.ToList());

            ReservationsTrasaction();
        }

        public IEnumerable<Room> EmptyRooms(DateTime date)
        {
            var res = Reservations.Where(i => i.Key.StartDate >= date && i.Key.EndDate <= date).
                Select(i => i.Key.RoomNumber);

            return Rooms.Except(Rooms.Where(i => res.Contains(i.Number)));
        }
        
        public IEnumerable<Room> EmptyRooms(DateTime startDate, DateTime endDate)
        {
            var res = Reservations.Where(i => i.Key.StartDate <= endDate && i.Key.EndDate >= startDate).
                Select(i => i.Key.RoomNumber);

            return Rooms.Except(Rooms.Where(i => res.Contains(i.Number)));
        }

        public void EndReservation(int id)
        {
            Reservations.Remove(Reservations.Where(i => i.Key.ID == id).Select(i => i.Key).First());

            ReservationsTrasaction();
        }

        public void CheckIn(int id, params int[] guests)
        {
            if (guests.Length > Rooms.Where(i => i.Number == Reservations.Where(i => i.Key.ID == id).
            Select(i => i.Key.RoomNumber).First()).
            Select(i => i.Capacity).First())
                throw new ArgumentOutOfRangeException("Number of guests is more then capacity of the room.");

            foreach (var g in guests)
                if (!Guests.Select(i => i.ID).Contains(g))
                    throw new Exception("Proposed guest for reservation doesn`t exist.");

            if (Reservations.Select(i => i.Key.ID).Any())
            {
                var key = Reservations.Where(i => i.Key.ID == id).First().Key;
                Reservations[key] = guests.ToList();
                Reservations.Keys.Where(i => i.ID == id).First().Status = ReservationStatus.Success;
            }
            else
                throw new Exception("Reservation with this id doesn`t exist.");

            ReservationsTrasaction();
        }

        public void CheckOut(int id)
        {
            if (Reservations.ContainsKey(Reservations.Where(i => i.Key.ID == id).First().Key))
            {
                Reservations.Keys.Where(i => i.ID == id).First().Status = ReservationStatus.Completed;
                ReservationsTrasaction();
            }
        }

        private void RoomsTrasaction()
        {
            FileStream fs = new FileStream("Rooms.dat", FileMode.OpenOrCreate);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, Rooms);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        private void GuestsTrasaction()
        {
            FileStream fs = new FileStream("Guests.dat", FileMode.OpenOrCreate);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, Guests);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        private void ReservationsTrasaction()
        {
            FileStream fs = new FileStream("Reservations.dat", FileMode.OpenOrCreate);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, Reservations);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }
    }
}
