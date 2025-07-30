using System.ComponentModel.DataAnnotations;

public class RequiredIfCompanyAttribute : ValidationAttribute
{
    //value here is the field that is being validated
    //validationcontext is the whole model that is being validated
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var user = (ApplicationUser)validationContext.ObjectInstance;
        if (user.IsCompany && string.IsNullOrWhiteSpace(value as string))
        {
            return new ValidationResult("Field is required");
        }

        return ValidationResult.Success;
    }
}