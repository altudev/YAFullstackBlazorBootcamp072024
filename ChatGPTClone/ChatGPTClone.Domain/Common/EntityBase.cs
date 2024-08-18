namespace ChatGPTClone.Domain.Common;

public abstract class EntityBase:IEntity,ICreatedByEntity,IModifiedByEntity
{
    public virtual Guid Id { get; set; }

    public virtual DateTimeOffset CreatedOn { get; set; }
    public virtual string CreatedByUserId { get; set; }

    public virtual DateTimeOffset? ModifiedOn { get; set; }
    public virtual string? ModifiedByUserId { get; set; }
}