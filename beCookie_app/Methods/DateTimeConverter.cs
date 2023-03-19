namespace beCookie_app.Methods
{
    public class DateTimeConverter
    {
        public static string GetDateTimeString() {
            var time = DateTime.Now;
            time.AddHours(5);
            return time.ToString("dd/MM/yyyy HH:mm"); 
        }

        public static DateTime GetDateTimeFromStr(string datetime)
        {
            var arr1 = datetime.Split();
            var result = new DateTime(Convert.ToInt32(arr1[0].Split('/')[2]), Convert.ToInt32(arr1[0].Split('/')[1]),
                Convert.ToInt32(arr1[0].Split('/')[0]), Convert.ToInt32(arr1[1].Split('/')[0]), Convert.ToInt32(arr1[1].Split('/')[1]),
                Convert.ToInt32(arr1[1].Split('/')[2]));
            return result;
        }
    }
}
