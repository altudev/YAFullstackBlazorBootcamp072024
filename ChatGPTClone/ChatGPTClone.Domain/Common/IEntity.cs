namespace ChatGPTClone.Domain.Common;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}