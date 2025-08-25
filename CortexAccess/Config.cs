namespace CortexAccess
{
    static class Config
    {
        /*
         * To get a client id and a client secret, you must connect to your Emotiv
         * account on emotiv.com and create a Cortex app.
         * https://www.emotiv.com/my-account/cortex-apps/
         */

        public static string AppClientId = "epu6nXA0CaWbOXolVglXzXGjw0YDF9Xn76xX6fj4";
        public static string AppClientSecret = "YUTAVc5eJQMkKfrOImFO5qKVk3sl1mz5IIBYGGhOtYpcS7IUgscjZW4vhdmVpE4Ia0sqcQyBxJPP8f3g3M8jObKba2bwVPJcBs63B5d3EIUdDK3mPUtU8V9WstA4UsTs";

		// Test 1
		//public static string AppClientId = "kvcg5h024IQHoNd9NzSMKpXw3Z7jNXC1uCwFb955";
        //public static string AppClientSecret = "jUD5qSy6biOzEheHYochMPEV51cnXWM0CavAFF7ZHn9ZztVwvIzCmCG7gaXmV3ghft2MZbAMEXXQZ8f3OKdWTLU9ZrNldM0ONCG3na98OOIfYcpOEf3e2M6ALUvYen7t";

        // Test2
        //public static string AppClientId = "fWEH76OZtbOD5D5XedV6gkeWHTwO9bA6tBvgngCW";
        //public static string AppClientSecret = "aGdHWptfxrS9F4wzQmNd2X0gpYwHVXQ74f6vsNJqQkMwzcJFnDoeVcdO2CsVC0Auq00Jgcvpioe0JRfXI5ZAUBrQ5K5JO7hqb5W52BnIK7BZxriS3UUrrshKhVC57VQr";


        // If you use an Epoc Flex headset, then you must put your configuration here
        public static string FlexMapping = @"{
                                  'CMS':'TP8', 'DRL':'P6',
                                  'RM':'TP10','RN':'P4','RO':'P8'}";

    }

    public static class WarningCode
    {
        public const int StreamStop = 0;
        public const int SessionAutoClosed = 1;
        public const int UserLogin = 2;
        public const int UserLogout = 3;
        public const int ExtenderExportSuccess = 4;
        public const int ExtenderExportFailed = 5;
        public const int UserNotAcceptLicense = 6;
        public const int UserNotHaveAccessRight = 7;
        public const int UserRequestAccessRight = 8;
        public const int AccessRightGranted = 9;
        public const int AccessRightRejected = 10;
        public const int CannotDetectOSUSerInfo = 11;
        public const int CannotDetectOSUSername = 12;
        public const int ProfileLoaded = 13;
        public const int ProfileUnloaded = 14;
        public const int CortexAutoUnloadProfile = 15;
        public const int UserLoginOnAnotherOsUser = 16;
        public const int EULAAccepted = 17;
        public const int StreamWritingClosed = 18;
        public const int HeadsetConnected = 104;
    }
}
