﻿using LiWiMus.Core.Models;

namespace LiWiMus.Core.Interfaces;

public interface IMailRequestService
{
    public Task<MailRequest> CreateConfirmEmailAsync(string userName, string email, string confirmUrl);
}