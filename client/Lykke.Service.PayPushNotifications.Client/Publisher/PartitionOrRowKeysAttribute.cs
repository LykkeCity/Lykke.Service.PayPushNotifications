using Common;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Lykke.Service.PayPushNotifications.Client.Publisher
{
    public class PartitionOrRowKeysAttribute : ValidationAttribute
    {
        public PartitionOrRowKeysAttribute() : base("The {0} field must contains a valid azure keys only.")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var key = value as string[];
            if (key == null || !key.Any())
            {
                return ValidationResult.Success;
            }

            if (key.All(StringUtils.IsValidPartitionOrRowKey))
            {
                return ValidationResult.Success;
            }

            if (validationContext == null)
            {
                return new ValidationResult($"\"{value.ToJson()}\" contains invalid azure key.");
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName });
            }
        }
    }
}
