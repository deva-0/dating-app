using System;

namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        ///     Calculates age.
        /// </summary>
        /// <param name="dob">Date of birth</param>
        /// <returns>Current age</returns>
        public static int CalculateAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}