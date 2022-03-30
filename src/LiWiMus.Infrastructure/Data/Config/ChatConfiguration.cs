﻿using LiWiMus.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiWiMus.Infrastructure.Data.Config;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.Property(c => c.Status)
               .HasConversion<string>();

        builder.HasOne(c => c.User)
               .WithMany(u => u.UserChats);

        builder.HasOne(c => c.Consultant)
               .WithMany(u => u.ConsultantChats);
    }
}