﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.Models;
using Movies.ViewModels;

namespace Movies.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _manager;

        public PurchasesController(ApplicationDbContext context, UserManager<AppUser> manager)
        {
            _context = context;
            _manager = manager;
        }

        // GET: Purchases
        public IActionResult Index()
        {
            var purchases = _context.Purchases
                .Include(p => p.Tickets)
                .Include("Tickets.Session.Movie")
                .Include("Tickets.Session.Hall")
                .Include("Tickets.User")
                .Select(p => new PurchaseViewModel(p))
                .ToList();
            return View(purchases);
        }
        [Authorize]
        public IActionResult MyPurchases()
        {
            var purchases = _context.Purchases
                .Include(p => p.Tickets)
                .Include("Tickets.Session.Movie")
                .Include("Tickets.Session.Hall")
                .Include("Tickets.User")
                .Where(t => t.UserId == _manager.GetUserId(User))
                .Select(p => new PurchaseViewModel(p))
                .ToList();
            return View(purchases);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                var purchase = await _context.Purchases
                .Include(p => p.Tickets)
                .Include("Tickets.Session.Movie")
                .Include("Tickets.Session.Hall")
                .Include("Tickets.User")
                .FirstOrDefaultAsync(m => m.PurchaseId == id);
                return View(new PurchaseViewModel(purchase!));
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admins")]
        public IActionResult Create()
        {
            ViewBag.UserId = _manager.GetUserId(User);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admins")]
        public IActionResult Create(PurchaseViewModel model)
        {
            var purchase = new Purchase(model);
            purchase.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                List<Ticket> tickets = _context.Tickets
                .Where(t => t.UserId == _manager.GetUserId(User))
                .Where(t => t.PurchaseId == null)
                .ToList();
                _context.Purchases.Add(purchase);
                _context.SaveChanges();
                foreach (var t in tickets)
                {
                    t.PurchaseId = purchase.PurchaseId;
                    _context.Update(t);
                    _context.SaveChanges();
                }
                return RedirectToAction("BuyResult", new {Person = User.Identity!.Name});
            }
            return View(model);
        }

        public IActionResult BuyResult(string Person)
        {
            if (Person != null)
            {
                ViewBag.Name = Person;
                return View();
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var purchase = await _context.Purchases
                .FindAsync(id);
                ViewBag.UserId = _manager.GetUserId(User);
                return View(new PurchaseViewModel(purchase!));
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admins")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PurchaseViewModel model)
        {
            var purchase = new Purchase(model);
            if (ModelState.IsValid)
            {
                if (id == purchase.PurchaseId)
                {
                    _context.Update(purchase);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            return View(model);
        }

        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var purchase = await _context.Purchases
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.PurchaseId == id);
                return View(new PurchaseViewModel(purchase!));
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase != null)
            {
                var ticketsToRemove = _context.Tickets
                    .Where(t => t.PurchaseId == purchase.PurchaseId).ToList();
                _context.Tickets.RemoveRange(ticketsToRemove);
                _context.Purchases.Remove(purchase);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
