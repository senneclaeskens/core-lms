﻿using CoreLMS.Core.DataTransferObjects;
using CoreLMS.Core.Entities;
using CoreLMS.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLMS.Application.Services
{
    public partial class AuthorService : IAuthorService
    {   
        private readonly IAppDbContext db;
        private readonly ILogger<AuthorService> logger;

        public AuthorService(IAppDbContext db, ILogger<AuthorService> logger)
        {   
            this.db = db;
            this.logger = logger;
        }

        public async Task<Author> AddAuthorAsync(CreateAuthorDto authorDto)
        {   
            var author = new Author
            {
                FirstName = authorDto.FirstName,
                MiddleName = authorDto.MiddleName,
                LastName = authorDto.LastName,
                Suffix = authorDto.Suffix,
                ContactEmail = authorDto.ContactEmail,
                ContactPhoneNumber = authorDto.ContactPhoneNumber,
                Description = authorDto.Description,
                WebsiteURL = authorDto.WebsiteURL
            };

            try
            {
                this.ValidateAuthorOnCreate(author);
            } 
            catch (Exception ex)
            {
                logger.LogError(ex, "Attempted to add invalid author.");
                throw;
            }

            return await this.db.CreateAuthorAsync(author);
        }

        public Task<Author> DeleteAuthorAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Author> GetAuthorAsync(int id)
        {
            var author = await db.SelectAuthorByIdAsync(id);

            // TODO Test logging / exception logging for efficiency //
            if (author == null)
            {
                logger.LogWarning($"Course {id} not found.");
                throw new ApplicationException($"Course {id} not found.");
            }

            return author;
        }

        public async Task<List<Author>> GetAuthorsAsync() => await this.db.SelectAuthorsAsync();

        public Task<Author> UpdateAuthorAsync(UpdateAuthorDto author)
        {
            throw new NotImplementedException();
        }
    }
}
