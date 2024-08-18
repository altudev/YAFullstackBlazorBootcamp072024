namespace ChatGPTClone.Domain.Common;

public interface IModifiedByEntity
{
    DateTimeOffset? ModifiedOn { get; set; }
    string? ModifiedByUserId { get; set; }
}