﻿using PicturesAPI.Entities;
using PicturesAPI.Entities.Joins;
using PicturesAPI.Repos.Interfaces;

namespace PicturesAPI.Repos;
public class TagRepo : ITagRepo
{
    private readonly PictureDbContext _dbContext;

    public TagRepo(PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Tag> GetAll()
    {
        return _dbContext.Tags.ToList();
    }

    public List<Tag> GetByPhrase(string phrase)
    {
        return _dbContext.Tags
            .Where(t => t.Value.Contains(phrase))
            .Take(5)
            .ToList();
    }

    public Tag GetByValue(string value)
    {
        return _dbContext.Tags.First(t => t.Value == value);
    }

    public Tag InsertAndSave(Tag tag)
    {
        if (_dbContext.Tags.Any(t => t.Value == tag.Value))
        {
            return _dbContext.Tags.FirstOrDefault(t => t.Value == tag.Value);
        }
        _dbContext.Tags.Add(tag);
        _dbContext.SaveChanges();
        return tag;
    }

    public void InsertPictureTagJoin(Picture picture, Tag tag)
    {
        _dbContext.PictureTagJoins.Add(new PictureTagJoin()
        {
            Picture = picture,
            Tag = tag
        });
    }

    public void Delete(Tag tag)
    {
        _dbContext.Tags.Remove(tag);
    }

    public void Update(Tag tag)
    {
        _dbContext.Tags.Update(tag);
    }

    public List<Tag> GetTagsByPictureId(int pictureId)
    {
        return _dbContext.PictureTagJoins
            .Where(p => p.PictureId == pictureId)
            .Select(p => p.Tag)
            .ToList();
    }

    public List<Tag> GetTagsByAccountId(int accountId)
    {
        return _dbContext.AccountLikedTagJoins
            .Where(a => a.AccountId == accountId)
            .Select(t => t.Tag)
            .ToList();
    }

    public void Save()
    {
        _dbContext.SaveChanges();
    }

}