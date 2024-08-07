using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        public CommentController(ICommentRepository commentRepo,IStockRepository stockRepo)
        {
            this._commentRepo = commentRepo;
            this._stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if(!ModelState.IsValid)
            return BadRequest(ModelState);

            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(s => s.ToCommentDto());
            return Ok(commentDto);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            
            if(!ModelState.IsValid)
            return BadRequest(ModelState);
            
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create ([FromRoute] int stockId , CreateCommentDto CommentDto)
        {
            
            if(!ModelState.IsValid)
            return BadRequest(ModelState);
            
            if(!await _stockRepo.StockExists(stockId))
            {
                return BadRequest("Stock does not exist!");
            }

            
            var commentModel = CommentDto.ToCommentFromCreate(stockId);
            await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new {id = commentModel}, commentModel.ToCommentDto());
        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update ([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto)
        {
            var comment = await this._commentRepo.UpdateAsync(id,updateDto.ToCommentFromUpdate(id));

            if(comment == null )
            {
                return NotFound();
            }

            return Ok(comment);

        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            
            if(!ModelState.IsValid)
            return BadRequest(ModelState);
            
            var commentModel = await  _commentRepo.DeleteAsync(id);

            if(commentModel == null)
            {
                return NotFound("comment does not exist");
            }

            return Ok(commentModel) ;
        }

        
    }
}