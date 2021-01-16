using WeKan.Domain.Common.Interfaces;

namespace WeKan.Domain.Common.Models
{
    public class Entity : IEntity
    {
        public int Id { get; internal set; }
        public int CreatedByUserId { get; private set; }
    }
}
