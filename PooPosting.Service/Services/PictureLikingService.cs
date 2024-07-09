﻿using Microsoft.EntityFrameworkCore;
using PooPosting.Data.DbContext;
using PooPosting.Data.DbContext.Entities;
using PooPosting.Data.DbContext.Entities.Joins;
using PooPosting.Data.Exceptions;
using PooPosting.Service.Mappers;
using PooPosting.Service.Models.Dtos.Picture;
using PooPosting.Service.Services.Helpers;
using PooPosting.Service.Services.Interfaces;

namespace PooPosting.Service.Services;

public class PictureLikingService(
        IAccountContextService accountContextService,
        PictureDbContext dbContext
        )
    : IPictureLikingService
{
    public async Task<PictureDto> Like(int pictureId)
    {
        var picture = await dbContext.Pictures
            .Include(p => p.Account)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Account)
            .Include(p => p.PictureTags)
            .ThenInclude(pt => pt.Tag)
            .ThenInclude(t => t.AccountLikedTags)
            .FirstOrDefaultAsync(p => p.Id == pictureId);

        if (picture == null) throw new NotFoundException();
        var accountId = accountContextService.GetAccountId();
        var pictureResult = await LikeAsync(picture, accountId);

        return await dbContext.Pictures
            .Where(p => p.Id == pictureResult.Id)
            .ProjectToDto()
            .FirstAsync();
    }

    public async Task<PictureDto> DisLike(int pictureId)
    {
        var picture = await dbContext.Pictures
            .Include(p => p.Account)
            .Include(p => p.Likes)
            .ThenInclude(l => l.Account)
            .Include(p => p.PictureTags)
            .ThenInclude(pt => pt.Tag)
            .FirstOrDefaultAsync(p => p.Id == pictureId);
            
        if (picture == null) throw new NotFoundException();
        var accountId = accountContextService.GetAccountId();
        var pictureResult = await DislikeAsync(picture, accountId);
        return pictureResult.MapToDto();
    }
    
    private async Task<Picture> LikeAsync(Picture picture, int accountId)
    {
        var like = picture.Likes.FirstOrDefault(l => l.AccountId == accountId);

        if (like == null)
        {
            // User hasn't liked this picture before
            picture.Likes.Add(new Like
            {
                AccountId = accountId,
                PictureId = picture.Id,
                Liked = DateTime.UtcNow
            });

            // Update LikedTags
            foreach (var pictureTag in picture.PictureTags)
            {
                var entryExists = pictureTag.Tag.AccountLikedTags.Any(alt =>
                    alt.AccountId == accountId && alt.TagId == pictureTag.TagId);

                if (entryExists) continue;

                pictureTag.Tag.AccountLikedTags.Add(new AccountLikedTag
                {
                    AccountId = accountId,
                    TagId = pictureTag.TagId,
                });
            }
        }
        else
        {
            picture.Likes.Remove(like);
        }

        // Update PopularityScore
        picture.PopularityScore = PictureScoreCalculator.CalcPoints(picture);

        await dbContext.SaveChangesAsync();
        return picture;
    }


    private async Task<Picture> DislikeAsync(Picture picture, int accountId)
    {
        var like = picture.Likes.SingleOrDefault(l => l.AccountId == accountId);

        if (like == null)
        {
            // User hasn't disliked this picture before
            picture.Likes.Add(new Like
            {
                AccountId = accountId,
                PictureId = picture.Id,
                Liked = DateTime.UtcNow
            });

            // No need to update LikedTags in this case
        }
        else
        {
            picture.Likes.Remove(like);
        }

        // Update PopularityScore
        picture.PopularityScore = PictureScoreCalculator.CalcPoints(picture);

        await dbContext.SaveChangesAsync();
        return picture;
    }
}