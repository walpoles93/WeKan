namespace WeKan.Domain.Common.Interfaces
{
    public interface IEntity
    {
        public int Id { get; }
        public string CreatedByUserId { get; }
    }
}
