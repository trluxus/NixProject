using System;
using System.Collections.Generic;
using System.Linq;

namespace NixProject
{
    class Program
    {
        private static Hotel hotel;

        private static bool AddGuest()
        {
            Console.WriteLine("Guest addition.\nInput Name.");
            var name = Console.ReadLine();

            Console.WriteLine("Input Surname.");
            var surname = Console.ReadLine();

            Console.WriteLine("Input Patronymic.");
            var patronymic = Console.ReadLine();

            var birthDate = DateTime.MinValue;
            Console.WriteLine("Input Birth date.");
            try
            {
                birthDate = DateTime.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.");
                return false;
            }
            
            Console.WriteLine("Input Residence address.\n");
            var residenceAddress = Console.ReadLine();

            try
            {
                var id = 0;

                if (hotel.Guests.Count == 0)
                    id = 0;
                else
                    id = hotel.Guests.Last().ID + 1;

                hotel.AddGuest(new Guest() { ID = id, Name = name, Surname = surname, Patronymic = patronymic, BirthDate = birthDate, ResidenceAddress = residenceAddress });
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.");
                return false;
            }

            return true;
        }

        private static bool RemoveGuest()
        {
            var id = 0;

            try
            {
                Console.WriteLine("Guest removal.\nInput ID.");
                id = Int32.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.");
                return false;
            }

            try
            {
                hotel.RemoveGuest(hotel.Guests.Where(i => i.ID == id).First());
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.");
                return false;
            }

            return true;
        }

        private static bool AddRoom()
        {
            var number = 0;
            var price = 0;
            var capacity = 0;

            try
            {
                Console.WriteLine("Room addition.\nInput Number.");
                number = int.Parse(Console.ReadLine());

                Console.WriteLine("Input Price.");
                price = int.Parse(Console.ReadLine());

                Console.WriteLine("Input Capacity.");
                capacity = int.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.");
                return false;
            }

            Console.WriteLine("Input Type.");
            var type = Console.ReadLine();

            try
            {
                hotel.AddRoom(new Room() { Number = number, Price = price, Capacity = capacity, Type = type});
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.");
                return false;
            }

            return true;
        }

        private static bool RemoveRoom()
        {
            var id = 0;

            try
            {
                Console.WriteLine("Room removal.\nInput Number.");
                id = int.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.");
                return false;
            }

            try
            {
                hotel.RemoveRoom(hotel.Rooms.Where(i => i.Number == id).First());
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.");
                return false;
            }

            return true;
        }

        private static bool ReserveRoom()
        {
            var number = 0;
            try
            {
                Console.WriteLine("Room reservation.\nInput Room number.");
                number = int.Parse(Console.ReadLine());

            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.");
                return false;
            }

            var startDate = DateTime.MinValue;
            var endDate = DateTime.MinValue;      
            try
            {
                Console.WriteLine("Input Start date.");           
                startDate = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("Input End date.");
                endDate = DateTime.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.");
                return false;
            }

            int[] guests;
            try
            {
                Console.WriteLine("Input Guest id`s separated by spaces.");
                guests = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.");
                return false;
            }

            try
            {
                var id = 0;

                if (hotel.Reservations.Count == 0)
                    id = 0;
                else
                    id = hotel.Reservations.Last().Key.ID + 1;

                hotel.ReserveRoom(new Reservation(id, number, startDate, endDate), guests);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.");
                return false;
            }

            return true;
        }

        private static bool EmtyRoomsByDate()
        {
            var date = DateTime.MinValue;
            try
            {
                Console.WriteLine("Input Date.");
                date = DateTime.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.");
                return false;
            }

            var r = hotel.EmptyRooms(date);

            foreach (var i in r)
            {
                Console.WriteLine($"{i.Number}");
            }

            return true;
        }

        private static bool EmtyRoomsByDateRange()
        {
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MinValue;           
            try
            {
                Console.WriteLine("Input Srart date.");
                startDate = DateTime.Parse(Console.ReadLine());
                
                Console.WriteLine("Input End date.");
                endDate = DateTime.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.");
                return false;
            }

            var r = hotel.EmptyRooms(startDate, endDate);

            foreach(var i in r)
            {
                Console.WriteLine($"{i.Number}");
            }

            return true;
        }

        private static bool EndReservation()
        {
            var id = 0;

            try
            {
                Console.WriteLine("End reservation.\nInput ID.");
                id = Int32.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.");
                return false;
            }

            hotel.EndReservation(id);

            return true;
        }

        private static bool ChekIn()
        {
            var id = 0;

            try
            {
                Console.WriteLine("CheckIn.\nInput ID.");
                id = Int32.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.");
                return false;
            }

            int[] guests;
            try
            {
                Console.WriteLine("Input actual Guest id`s separated by spaces.");
                guests = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.");
                return false;
            }

            hotel.CheckIn(id, guests);

            return true;
        }

        private static bool CheckOut()
        {
            var id = 0;

            try
            {
                Console.WriteLine("CheckOut.\nInput ID.");
                id = Int32.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.");
                return false;
            }

            hotel.CheckOut(id);

            return true;
        }

        static void Main(string[] args)
        {
            var s = true;

            while (s)
            {
                Console.WriteLine("Select action:\n\t1. Add room.\n\t2. Remove room\n\t3. Add guest\n\t4. Remove guest");
                Console.WriteLine("Select action:\n\t5. Reserve room.\n\t6. Empty rooms by date\n\t7. Empty rooms by date range\n\t8. End reservation");
                Console.WriteLine("Select action:\n\t9. Chek in.\n\t10. Chek out\n\t11. End work");
                switch (Console.ReadLine())
                {
                    case "1":
                        AddRoom();
                        break;
                    case "2":
                        RemoveRoom();
                        break;
                    case "3":
                        AddGuest();
                        break;
                    case "4":
                        RemoveGuest();
                        break;
                    case "5":
                        ReserveRoom();
                        break;
                    case "6":
                        EmtyRoomsByDate();
                        break;
                    case "7":
                        EmtyRoomsByDateRange();
                        break;
                    case "8":
                        EndReservation();
                        break;
                    case "9":
                        ChekIn();
                        break;
                    case "10":
                        CheckOut();
                        break;
                    case "11":
                        s = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
            
        }
    }
}
