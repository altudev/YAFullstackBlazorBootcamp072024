
using ChatGPTClone.Infrastructure.Persistence.Contexts;

namespace ChatGPTClone.WebApi.Controllers;

using ChatGPTClone.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ChatSessionsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public ChatSessionsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: api/ChatSessions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChatSession>>> GetChatSessions()
    {
        return await _dbContext.ChatSessions.ToListAsync();
    }

    // GET: api/ChatSessions/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ChatSession>> GetChatSession(Guid id)
    {
        var chatSession = await _dbContext.ChatSessions.FindAsync(id);

        if (chatSession == null)
        {
            return NotFound();
        }

        return chatSession;
    }

    // POST: api/ChatSessions
    [HttpPost]
    public async Task<ActionResult<ChatSession>> PostChatSession(ChatSession chatSession)
    {
        _dbContext.ChatSessions.Add(chatSession);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetChatSession), new { id = chatSession.Id }, chatSession);
    }

    // PUT: api/ChatSessions/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutChatSession(Guid id, ChatSession chatSession)
    {
        if (id != chatSession.Id)
        {
            return BadRequest();
        }

        _dbContext.Entry(chatSession).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ChatSessionExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/ChatSessions/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChatSession(Guid id)
    {
        var chatSession = await _dbContext.ChatSessions.FindAsync(id);
        if (chatSession == null)
        {
            return NotFound();
        }

        _dbContext.ChatSessions.Remove(chatSession);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool ChatSessionExists(Guid id)
    {
        return _dbContext.ChatSessions.Any(e => e.Id == id);
    }
}