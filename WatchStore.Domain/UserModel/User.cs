using System.Text.Json.Serialization;

namespace WatchStore.Domain
{
    public class User 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [JsonIgnore] public string Password { get; set; }


        public virtual ShoppingCart UserCart { get; set; }
    }
}
