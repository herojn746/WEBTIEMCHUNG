namespace TiemChung.Entity
{
    public abstract class Entity
    {
        protected Entity()
        {
            //Id = Guid.NewGuid().ToString("N");
            CreateTimes = DateTime.Now;

        }
        public string Id { get; set; }
        public DateTime CreateTimes { get; set; }
        public string? CreateBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
    }
}
