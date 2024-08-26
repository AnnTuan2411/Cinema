using System;
using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property)]
public class MinimumAgeAttribute : ValidationAttribute
{
    private readonly int _minimumAge;

    public MinimumAgeAttribute(int minimumAge)
    {
        _minimumAge = minimumAge;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime dateOfBirth)
        {
            var age = CalculateAge(dateOfBirth);

            if (age < _minimumAge)
            {
                return new ValidationResult($"Must be at least {_minimumAge} years old.");
            }
        }

        return ValidationResult.Success;
    }

    private int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;

        if (birthDate.Date > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }
}