using System.Text.RegularExpressions;

namespace MotorcycleDeliveryRentWebAPI.Api.Validators
{
    public class ValidatorAdminDriver
    {
        public static void Plate(string plate)
        {
            var regex = new Regex(@"^[a-zA-Z]{1,3}\-?\d{1,4}\-?[a-zA-Z]?$");
            if (!regex.IsMatch(plate))
            {
                throw new Exception("Plate need be valid");
            }
        }
        public static void Email(string email)
        {
            var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

            if (!regex.IsMatch(email))
            {
                throw new Exception("E-mail need be valid");
            }
        }
        public static void Password(string password)
        {
            var regex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

            if (!regex.IsMatch(password))
            {
                throw new Exception("The password must be at least: 8 characters, 1 uppercase, 1 lowercase, 1 number and 1 especial character");
            }
        }
        public static void Cnpj(string cnpj)
        {
            var regex = new Regex(@"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}|\d{14}$");

            if (!regex.IsMatch(cnpj))
            {
                throw new Exception("The CNPJ must be in the format xx.xxx.xxx/xxxx-xx or must contain 14 numeric digits.");
            }

            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            int[] multipliers1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multipliers2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            if (cnpj.Length != 14)
            {
                throw new Exception("The CNPJ must contain 14 numeric digits.");
            }

            string tempCnpj = cnpj.Substring(0, 12);
            int sum = 0;

            for (int i = 0; i < 12; i++)
            {
                sum += int.Parse(tempCnpj[i].ToString()) * multipliers1[i];
            }

            int remainder = sum % 11;
            remainder = remainder < 2 ? 0 : 11 - remainder;

            string digit = remainder.ToString();
            tempCnpj += digit;
            sum = 0;

            for (int i = 0; i < 13; i++)
            {
                sum += int.Parse(tempCnpj[i].ToString()) * multipliers2[i];
            }

            remainder = sum % 11;
            remainder = remainder < 2 ? 0 : 11 - remainder;

            digit += remainder.ToString();

            if (!cnpj.EndsWith(digit))
            {
                throw new Exception("The CNPJ's verification digit is invalid.");
            }
        }
    }
}
