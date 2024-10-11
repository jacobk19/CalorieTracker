using System;
using System.Collections.Generic;

namespace CalorieTrackerAPI.Models
{
    public class Day
    {
        public int Id { get; set; }           // Primary Key for the Day
        public int UserId { get; set; }        // Foreign Key for the User
        public DateTime Date { get; set; }     // Date for the Day
        public List<Food> Foods { get; set; }  // List of foods consumed on this day

        public Day()
        {
            Foods = new List<Food>();
        }
    }
}