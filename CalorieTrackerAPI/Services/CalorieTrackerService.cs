using CalorieTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalorieTrackerAPI.Services // Ensure this matches your actual namespace
{
    public class CalorieTrackerService
    {
        /// <summary>
        /// Calculates the total calories from a list of foods.
        /// </summary>
        /// <param name="foods">A list of food items.</param>
        /// <returns>The total calories.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the foods list is null.</exception>
        public int CalculateTotalCalories(List<Food> foods)
        {
            if (foods == null) // Check for null to avoid exceptions
            {
                throw new ArgumentNullException(nameof(foods), "Food list cannot be null.");
            }

            // Use LINQ to sum up the calories
            return foods.Sum(f => f.Calories);
        }
    }
}