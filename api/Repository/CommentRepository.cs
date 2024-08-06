using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;


        public CommentRepository(ApplicationDBContext context)
        {
            this._context = context;
        }




        public async Task<Comment> CreateAsync(Comment CommentModel)
        {
            await _context.Comments.AddAsync(CommentModel);
            await _context.SaveChangesAsync();
            return CommentModel;
        }

       

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<Comment> UpdateAsync(int id, Comment commentModel)
        {
            var existingComment = await _context.Comments.FindAsync(id);

            if(existingComment == null)
            {
                return null;
            }

            existingComment.Title = commentModel.Title;
            existingComment.Content = commentModel.Content;

            await _context.SaveChangesAsync();

            return existingComment;
        }


         public async Task<Comment> DeleteAsync(int id)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id); 
                       
            if(commentModel == null )
            {
                return null;
            }

             _context.Remove(commentModel);
             await _context.SaveChangesAsync();
             return commentModel;
        }

      
    }
}