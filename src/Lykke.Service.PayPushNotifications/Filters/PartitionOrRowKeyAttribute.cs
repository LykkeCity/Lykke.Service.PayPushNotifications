using Common;
using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PayPushNotifications.Filters
{
    public class PartitionOrRowKeyAttribute : ValidationAttribute
    {
        public PartitionOrRowKeyAttribute() : base("The {0} field must be a valid azure key.")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var key = value as string;
            if (key == null)
            {
                return ValidationResult.Success;
            }

            if (StringUtils.IsValidPartitionOrRowKey(key))
            {
                return ValidationResult.Success;
            }

            if (validationContext == null)
            {
                return new ValidationResult($"\"{value}\" is invalid azure key.");
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName });
            }
        }
    }
}
