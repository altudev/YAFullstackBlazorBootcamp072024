using System.Text.Json;
using ChatGPTClone.Domain.Entities;
using ChatGPTClone.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatGPTClone.Infrastructure.Persistence.Configurations;

public class ChatSessionConfiguration : IEntityTypeConfiguration<ChatSession>
{
    public void Configure(EntityTypeBuilder<ChatSession> builder)
    {
        // ID
        builder.HasKey(p => p.Id);

        // Title
        builder.Property(p => p.Title)
            .HasMaxLength(100)
            .IsRequired();

        // Model
        builder.Property(p => p.Model)
            .IsRequired()
            .HasConversion<int>();

        // Threads (JSONB configuration)
        builder.Property(p => p.Threads)
            .HasColumnType("jsonb")
            .IsRequired();

        // Configure JSON serialization options
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Threads => threads in JSON // ChatMessage => chatMessage
            WriteIndented = true
        };

        // Configure value conversion and comparison for Threads
        builder.Property(p => p.Threads)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<ChatThread>>(v, jsonOptions) ?? new List<ChatThread>(),
                new ValueComparer<List<ChatThread>>(
                    (c1, c2) => JsonSerializer.Serialize(c1, jsonOptions) == JsonSerializer.Serialize(c2, jsonOptions),
                    c => c == null ? 0 : JsonSerializer.Serialize(c, jsonOptions).GetHashCode(),
                    c => JsonSerializer.Deserialize<List<ChatThread>>(JsonSerializer.Serialize(c, jsonOptions), jsonOptions)
                )
            );

        // Index on JSONB column for better performance
        builder.HasIndex(p => p.Threads)
            .HasMethod("gin");

        //// Configure JSONB operations
        //builder.HasQueryFilter(p => EF.Functions.JsonContains(p.Threads, @"{""id"": ""some-id""}"));

        //// AppUser relationship
        //builder.HasOne(p => p.AppUser)
        //    .WithMany()
        //    .HasForeignKey(p => p.AppUserId)
        //    .OnDelete(DeleteBehavior.Cascade);

        // CreatedOn
        builder.Property(p => p.CreatedOn)
            .IsRequired();

        // CreatedByUserId
        builder.Property(p => p.CreatedByUserId)
            .IsRequired()
            .HasMaxLength(150);

        // ModifiedOn
        builder.Property(p => p.ModifiedOn)
            .IsRequired(false);

        // ModifiedByUserId
        builder.Property(p => p.ModifiedByUserId)
            .IsRequired(false)
            .HasMaxLength(150);

        builder.ToTable("ChatSessions");
    }
}