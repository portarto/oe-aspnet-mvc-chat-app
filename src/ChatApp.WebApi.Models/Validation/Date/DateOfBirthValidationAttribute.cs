using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.WebApi.Models.Validation.Date
{
    internal class DateOfBirthValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var minDate = DateTime.Now.AddYears(-100);
            var maxDate = DateTime.Now;

            var dt = Convert.ToDateTime(value);
            return dt >= minDate && dt <= maxDate; 
        }
    }
}
