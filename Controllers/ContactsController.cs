using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApplicationDevelopmentCourseProject.Data;
using ApplicationDevelopmentCourseProject.Models;
using System.Net.Mail;
using System.Net;
using System.Threading;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace ApplicationDevelopmentCourseProject.Controllers
{
    //TODO: HANDLE CRUD VIEW RETURNS

    public class ContactsController : Controller
    {
        private readonly ApplicationDevelopmentCourseProjectContext _context;

        public ContactsController(ApplicationDevelopmentCourseProjectContext context)
        {
            _context = context;
        }

        // GET: Contacts
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Index()
        {
            return View(await _context.Contact.ToListAsync());
        }

        // GET: Contacts/Details/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Telephone,Message")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();

                //prepare email
                var toAddress = "talofirohad@yahoo.com";
                var fromAddress = contact.Email;
                var subject = "Test enquiry from " + contact.Name;
                var message = new StringBuilder();
                message.Append("Name: " + contact.Name + "\n");
                message.Append("Email: " + contact.Email + "\n");
                message.Append("Telephone: " + contact.Telephone + "\n\n");
                message.Append(contact.Message);

                //start email Thread
                var tEmail = new Thread(() =>
                SendEmail(toAddress, fromAddress, subject, message.ToString()));
                tEmail.Start();

                return RedirectToAction(nameof(Index));
            }

            else
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            }

            return View(contact);
        }

        // GET: Contacts/Edit/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Telephone,Message")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
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
            return View(contact);
        }

        // GET: Contacts/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contact.FindAsync(id);
            _context.Contact.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]

        private bool ContactExists(int id)
        {
            return _context.Contact.Any(e => e.Id == id);
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> ContactEmail([FromForm] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();

                //prepare email
                var toAddress = contact.Email;
                var fromAddress = "talofirohad@yahoo.com";
                var subject = contact.Name;
                var message = new StringBuilder();
                message.Append("Name: " + contact.Name + "\n");
                message.Append("Email: " + contact.Email + "\n");
                message.Append("Telephone: " + contact.Telephone + "\n\n");
                message.Append(contact.Message);

                //start email Thread
                var tEmail = new Thread(() =>
                SendEmail(toAddress, fromAddress, subject, message.ToString()));
                tEmail.Start();

                return RedirectToAction("Index", "Home");
            }

            else
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            }

            return RedirectToAction("Index", "Home"); ;
        }

        public void SendEmail(string toAddress, string fromAddress,
                      string subject, string message)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(fromAddress);
                mail.To.Add(toAddress);
                mail.Subject = subject;
                mail.Body = message;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("talofirohad@gmail.com", "Aa123456789!!!");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
