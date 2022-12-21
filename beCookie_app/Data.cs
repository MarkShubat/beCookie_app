using beCookie_app.Models;

namespace beCookie_app
{
    public class Data
    {
        public static User currentUser = new User();
        public static string verifCode = "";
        public static User userToEnter = new User();
    }
}
