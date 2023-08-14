﻿using PooPosting.Api.Models.Dtos.Account;

namespace PooPosting.Api.Models.Dtos.Comment;

public class CommentDto
{
    public string Id { get; set; }
    public string PictureId { get; set; }

    public string Text { get; set; }
    public DateTime CommentAdded { get; set; }

    public AccountDto Account { get; set; }
    public bool IsModifiable { get; set; } = false;
    public bool IsAdminModifiable { get; set; } = false;
}