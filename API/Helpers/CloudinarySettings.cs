namespace API.Helpers
{
    /// <summary>
    ///     Settings for cloudinary.com API, image hosting service.
    /// </summary>
    public class CloudinarySettings
    {
        public string CloudName { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }
}