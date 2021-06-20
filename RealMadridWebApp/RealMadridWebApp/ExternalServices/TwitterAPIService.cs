using TweetSharp;

namespace RealMadridWebApp.ExternalServices {

    public static class TwitterAPIService {

        private static TwitterService service;

        static TwitterAPIService() {

            // Connect to the service and authenticate.
            service = new TwitterService(Keys.TweeterAPIKey, Keys.TweeterAPISecretKey);
            service.AuthenticateWith(Keys.TweeterToken, Keys.TweeterSecretToken);
        }

        public static void SendTweet(string message) {

            service.SendTweet(new SendTweetOptions {
                Status = message
            });

        }
    }
}
