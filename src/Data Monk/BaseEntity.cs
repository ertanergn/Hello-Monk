namespace Monk.Data
{
    public class BaseEntity<TKey>
    {
        public virtual TKey Id { get; set; }
    }
}
