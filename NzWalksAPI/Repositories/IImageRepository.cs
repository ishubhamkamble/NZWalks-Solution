﻿using NzWalksAPI.Models.Domain;

namespace NzWalksAPI.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
