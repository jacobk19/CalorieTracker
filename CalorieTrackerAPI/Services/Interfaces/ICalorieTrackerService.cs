using System.Collections.Generic;
using CalorieTrackerAPI.Models;

namespace CalorieTrackerAPI.Services.Interfaces
{
    public interface ICalorieTrackerService
    {
        int CalculateTotalCalories(List<Food> foods); // Business logic for calorie calculation
    }
}