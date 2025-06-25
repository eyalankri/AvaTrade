 
namespace Common
{    
    public interface IEntity
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
