using CalorieTrackerAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CalorieTrackerAPI.Services // Ensure this matches your actual namespace
{
    public class DataService
    {
        private readonly string _filePath;

        public DataService(string filePath = "days.txt") // Constructor allows for dependency injection
        {
            _filePath = filePath;
        }

        public List<Day> ReadDaysFromFile()
        {
            if (!File.Exists(_filePath)) // Check if file exists
            {
                Console.WriteLine($"File not found: {_filePath}"); // Log file not found
                return new List<Day>(); // Return an empty list if the file doesn't exist
            }

            var lines = File.ReadAllLines(_filePath);
            var days = new List<Day>();

            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line)) // Avoid processing empty lines
                {
                    try
                    {
                        var day = JsonConvert.DeserializeObject<Day>(line);
                        if (day != null) // Check if deserialization was successful
                        {
                            days.Add(day);
                        }
                        else
                        {
                            Console.WriteLine($"Deserialization returned null for line: {line}"); // Log null
                        }
                    }
                    catch (JsonException ex)
                    {
                        // Log the error for debugging purposes
                        Console.WriteLine($"Failed to deserialize line: {line}. Error: {ex.Message}");
                    }
                }
            }

            Console.WriteLine($"Successfully read {days.Count} days from file."); // Log success message
            return days;
        }

        public void AppendDayToFile(Day day)
        {
            if (day == null) // Check if the day object is null
            {
                throw new ArgumentNullException(nameof(day), "Day cannot be null.");
            }

            var dayJson = JsonConvert.SerializeObject(day);
            try
            {
                File.AppendAllText(_filePath, dayJson + Environment.NewLine);
                Console.WriteLine($"Appended to file: {dayJson}"); // Log successful append
            }
            catch (IOException ioEx)
            {
                // Handle any IO exceptions that occur while writing to the file
                Console.WriteLine($"Failed to append to file: {ioEx.Message}");
            }
        }
    }
}
