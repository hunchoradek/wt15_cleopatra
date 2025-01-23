using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Cleopatra.API.Helpers
{
    public static class WorkingHoursHelper
    {
        // Funkcja parsująca godziny pracy z JSON
        public static Dictionary<string, List<(TimeSpan Start, TimeSpan End)>> ParseWorkingHours(string workingHoursJson)
        {
            var workingHours = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(workingHoursJson);
            var parsedHours = new Dictionary<string, List<(TimeSpan Start, TimeSpan End)>>();

            foreach (var day in new[] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" })
            {
                if (!workingHours.ContainsKey(day))
                {
                    parsedHours[day] = new List<(TimeSpan Start, TimeSpan End)>();
                    continue;
                }

                var hoursList = new List<(TimeSpan Start, TimeSpan End)>();

                foreach (var period in workingHours[day])
                {
                    var times = period.Split('-');
                    var start = TimeSpan.Parse(times[0]);
                    var end = TimeSpan.Parse(times[1]);
                    hoursList.Add((start, end));
                }

                parsedHours[day] = hoursList;
            }

            return parsedHours;
        }


        // Funkcja obliczająca dostępne godziny w danym dniu
        public static List<(TimeSpan Start, TimeSpan End)> CalculateAvailableHours(
            List<(TimeSpan Start, TimeSpan End)> workingHours,
            List<(TimeSpan Start, TimeSpan End)> bookedAppointments)
        {
            var availableHours = new List<(TimeSpan Start, TimeSpan End)>();

            foreach (var period in workingHours)
            {
                var currentStart = period.Start;

                // Posortuj rezerwacje, które zachodzą na aktualny przedział godzin pracy
                var sortedAppointments = bookedAppointments
                    .Where(a => a.Start < period.End && a.End > period.Start) // Rezerwacje pokrywające się z przedziałem pracy
                    .OrderBy(a => a.Start)
                    .ToList();

                foreach (var appointment in sortedAppointments)
                {
                    // Jeśli rezerwacja zaczyna się po aktualnym początku, dodaj dostępny przedział
                    if (appointment.Start > currentStart)
                    {
                        availableHours.Add((currentStart, appointment.Start));
                    }

                    // Zaktualizuj początek do końca bieżącej rezerwacji
                    currentStart = appointment.End > currentStart ? appointment.End : currentStart;
                }

                // Dodaj ostatni wolny przedział, jeśli taki istnieje
                if (currentStart < period.End)
                {
                    availableHours.Add((currentStart, period.End));
                }
            }



            return availableHours;
        }




    }
}
