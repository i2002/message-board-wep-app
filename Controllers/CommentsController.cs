using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MessageBoard.Data;
using MessageBoard.Models;
using MessageBoard.Controllers;

namespace MessageBoard.Migrations
{
    public class CommentsController : Controller
    {
        private readonly MessageBoardContext _context;

        public CommentsController(MessageBoardContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var messageBoardContext = _context.Comment.Include(c => c.Topic).Include(c => c.User);
            return View(await messageBoardContext.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Topic)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public async Task<IActionResult> Create(int? topicId)
        {
            Topic? topic = null;
            if (topicId == null)
            {
                ViewData["TopicId"] = new SelectList(_context.Topic, "Id", "Title");
            }
            else
            {
                topic = await _context.Topic.FirstOrDefaultAsync(t => t.Id == topicId);
                if (topic == null)
                {
                    return NotFound();
                }
            }
            ViewData["Topic"] = topic;
            ViewData["Users"] = new SelectList(_context.User, "Id", "Name");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,CreatedDate,UpdatedDate,TopicId,UserId")] Comment comment, int? replyTopicId)
        {
            if (replyTopicId != null)
            {
                comment.TopicId = (int)replyTopicId;
            } 
        
            comment.CreatedDate = DateTime.Now;
            comment.UpdatedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                if (replyTopicId == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Details", "Topics", new {Id = replyTopicId});
                }
            }
            ViewData["TopicId"] = new SelectList(_context.Topic, "Id", "Id", comment.TopicId);
            ViewData["Users"] = new SelectList(_context.User, "Id", "Name");
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id, int? topicId)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["TopicId"] = new SelectList(_context.Topic, "Id", "Id", comment.TopicId);
            ViewData["Users"] = new SelectList(_context.User, "Id", "Name");

            Topic? topic = null;
            if (topicId != null)
            {
                topic = await _context.Topic.FirstOrDefaultAsync(t => t.Id == topicId);
                if (topic == null)
                {
                    return NotFound();
                }
            }
            ViewData["Topic"] = topic;

            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,CreatedDate,UpdatedDate,TopicId,UserId")] Comment comment, bool? returnToTopic)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            comment.UpdatedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                if (returnToTopic == null || returnToTopic == false)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Details", "Topics", new { Id = comment.TopicId });
                }
            }
            ViewData["TopicId"] = new SelectList(_context.Topic, "Id", "Id", comment.TopicId);
            ViewData["Users"] = new SelectList(_context.User, "Id", "Name");
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Topic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comment == null)
            {
                return Problem("Entity set 'MessageBoardContext.Comment'  is null.");
            }
            var comment = await _context.Comment.FindAsync(id);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
          return (_context.Comment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
