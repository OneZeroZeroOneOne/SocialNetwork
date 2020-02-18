namespace SocialNetwork.Dal.Models
{
    public class Setting<T>
    {
        public string Key { get; set; }
        public T Value { get; set; }
    }
}
