namespace SocialNetwork.ConfigSetting.Dal.Models
{
    public class Setting<T>
    {
        public string Key { get; set; }
        public T Value { get; set; }
    }
}
