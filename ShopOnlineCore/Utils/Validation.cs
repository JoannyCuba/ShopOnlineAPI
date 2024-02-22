using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShopOnlineCore.Utils
{
    public class Validation
    {
        public static bool IsEmail(string email)
        {
            // Expresión regular para validar el formato de un correo electrónico
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            // Validar el correo utilizando la expresión regular
            bool esValid = Regex.IsMatch(email, pattern);
            return esValid;
        }
        public static bool IsPhoneNumber(string telefono)
        {
            if (telefono == null)
                return false;
            // Expresión regular para validar el formato de un número de teléfono
            string patron = @"^(\+\d{1,4})?(\d+)$";
            // Validar el teléfono utilizando la expresión regular
            bool esValido = Regex.IsMatch(telefono, patron);
            return esValido;
        }
        public static bool IsValidDomain(string domain)
        {
            if (string.IsNullOrEmpty(domain))
                return false;
            Regex regex = new Regex(@"^[a-zA-Z0-9]+\.[0-9]+\.service$");
            if (regex.IsMatch(domain))
                return true;
            else
                return false;
        }
    }
}
