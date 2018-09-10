using System;
using System.Collections.Generic;

namespace BMS_Check_MovieTicket
{
    class Program
    {
        static void Main(string[] args)
        {
            string MovieName = "The Nun";

            List<string> PreferredTheatre = new List<string>() { "PVR: Inorbit, Cyberabad", "PVR ICON: Hitech, Madhapur, Hyderabad" }; //Give exact names of Movie Theatres as in BookMyShow

            long TimeStart = 201809081400; //Time and Date in format [YYYYMMDDTTTT] Time in 24 Hrs

            long TimeEnd = 201809082300; //Time and Date in format [YYYYMMDDTTTT] Time in 24 Hrs

            BMS.BookTickets(MovieName, PreferredTheatre, TimeStart, TimeEnd);
        }
    }
}
