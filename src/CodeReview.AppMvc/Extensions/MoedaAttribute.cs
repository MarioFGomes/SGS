using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CodeReview.AppMvc.Extensions
{
    public class MoedaAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var moeda = Convert.ToDecimal(value, new CultureInfo("pt-AO"));
            }
            catch (Exception)
            {
                return new ValidationResult("Moeda em Formato inválido");
            }
            return ValidationResult.Success;
        }

    }
}