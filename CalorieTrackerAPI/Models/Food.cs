namespace CalorieTrackerAPI.Models
{
    public class Food
    {
        public int FoodId { get; set; }        // Primary Key for the Food
        public string FoodName { get; set; }   // Name of the food (char(30) in DB)
        public int Calories { get; set; }      // Calories for the food item
    }
}