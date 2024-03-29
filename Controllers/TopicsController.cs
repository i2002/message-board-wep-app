﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MessageBoard.Data;
using MessageBoard.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;

namespace MessageBoard.Controllers
{
    public class TopicsController : Controller
    {
        private readonly MessageBoardContext _context;

        public TopicsController(MessageBoardContext context)
        {
            _context = context;
        }

        // GET: Topics
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Topic == null)
            {
                return Problem("Entity set 'MessageBoardContext.Topic'  is null.");
            }

            var topics = from t in _context.Topic.Include(t => t.User) select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                topics = topics.Where(t => t.Title!.Contains(searchString));
            }

            return View(await topics.ToListAsync());
        }

        // GET: Topics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Topic == null)
            {
                return NotFound();
            }

            var topic = await _context.Topic
                .Include(t => t.Comments)
                .ThenInclude(c => c.User)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            /*_context.Entry(topic).Collection(t => t.Comments).Load();*/
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // GET: Topics/Create
        public IActionResult Create()
        {
            ViewData["Users"] = new SelectList(_context.User, "Id", "Name");
            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,Category,UserId")] Topic topic)
        {
            topic.CreatedDate = DateTime.Now;
            topic.UpdatedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(topic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Users"] = new SelectList(_context.User, "Id", "Name");
            return View(topic);
        }

        // GET: Topics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Topic == null)
            {
                return NotFound();
            }

            var topic = await _context.Topic.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            ViewData["Users"] = new SelectList(_context.User, "Id", "Name", topic.UserId);
            return View(topic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,CreatedDate,UpdatedDate,Content,Category,UserId")] Topic topic)
        {
            if (id != topic.Id)
            {
                return NotFound();
            }

            topic.UpdatedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(topic.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Users"] = new SelectList(_context.User, "Id", "Name");
            return View(topic);
        }

        // GET: Topics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Topic == null)
            {
                return NotFound();
            }

            var topic = await _context.Topic
                .FirstOrDefaultAsync(m => m.Id == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Topic == null)
            {
                return Problem("Entity set 'MessageBoardContext.Topic'  is null.");
            }
            var topic = await _context.Topic.FindAsync(id);
            if (topic != null)
            {
                _context.Topic.Remove(topic);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TopicExists(int id)
        {
          return (_context.Topic?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
