using IbanNet;

namespace Web.Utilidades
{
    public class Utilidades
    {
        public static decimal CalcularIva(decimal importe, string iva)
        {
            return (importe - (importe / decimal.Parse(iva, System.Globalization.CultureInfo.InvariantCulture)));
        }

        public static decimal CalcularBase(decimal importe, string iva)
        {
            return (importe / decimal.Parse(iva, System.Globalization.CultureInfo.InvariantCulture));
        }
        
        public static bool ValidateIban(string iban)
        {
            string Iban = iban.Replace(" ", "");

            IbanValidator validator = new IbanValidator();
            ValidationResult validationResult = validator.Validate(Iban);

            return validationResult.IsValid;
            
        }

    }
}
