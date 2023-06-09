namespace PrivacyPulse_BACK.Constants
{
    public static class Paths
    {
        public static string BasePath = "C:/temp/PrivacyPulse/";

        public static string GetUserImagePath(int userid)
        {
            var dir = $"{BasePath}users/{userid}/images/";
            Directory.CreateDirectory(dir);

            return dir;
        }

        public static string GetProfilePicturePath(int userid)
        {
            return $"{GetUserImagePath(userid)}profile.png";
        }

        public static string GetPostsImageBasePath()
        {
            var dir = $"{BasePath}posts/";
            Directory.CreateDirectory(dir);

            return dir;
        }

        public static string GetPostImagePath(int postId)
        {
            return $"{GetPostsImageBasePath()}{postId}.png";
        }
    }
}
