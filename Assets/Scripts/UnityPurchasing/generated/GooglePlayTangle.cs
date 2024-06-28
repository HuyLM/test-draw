// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("klriyPp9c0gzaqpBcSVd5Dz3FaxkANLmLfvNoawrT3Oqk6YahNuK/5wfER4unB8UHJwfHx6gaVVycCzcwh8e/R9Jng92+4nn0sWNGGYr5NkQkSu5tuoXotiaHBK3R/Qcraedni6cHzwuExgXNJhWmOkTHx8fGx4d1V7AWmnwhA5MQm+WOOvbghoaNWyC1+5+T6A7S+vntXhIct3m/3zCH9uhlSqfCkva9FK1iCjUg3JfaPmiQrSMaBe907WXxXqrn+6YeW/Pp84CW1irQl/m8S4xokyOvGOitbiBe4k4LINfnmSzU3MLQfA0VR7UtQiGSJD37GMcc4wrjU0cCu+bC88Gq2oaY+RD1Mnl7v9RHnEaOhpKVOk6KkROxJoWt8BJ5RwdHx4f");
        private static int[] order = new int[] { 12,4,4,5,10,12,9,9,9,11,10,11,12,13,14 };
        private static int key = 30;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
