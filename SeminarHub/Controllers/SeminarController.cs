using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeminarHub.Data;
using SeminarHub.Data.DataConstants;
using SeminarHub.Models;
using System.Globalization;
using System.Security.Claims;

namespace SeminarHub.Controllers
{
    [Authorize]
    public class SeminarController : Controller
    {
        private readonly SeminarHubDbContext data;

        public SeminarController(SeminarHubDbContext data)
            => this.data = data;

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var seminars = await data
                .Seminars
                .Select(s => new AllSeminarsViewModel
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    Lecturer = s.Lecturer,
                    DateAndTime = s.DateAndTime.ToString(SeminarConstants.SeminarDateFormat),
                    Category = s.Category.Name,
                    Organizer = s.Organizer.UserName
                })
                .ToListAsync();

            return View(seminars);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddSeminarViewModel
            {
                Categories = await GetCategories()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddSeminarViewModel seminar)
        {
            int duration ;

            if (!int.TryParse(seminar.Duration, out duration) || duration < SeminarConstants.DurationMinValue || duration > SeminarConstants.DurationMaxValue)
            {
               
                ModelState
                    .AddModelError(nameof(seminar.Duration), $"Duration must be between {SeminarConstants.DurationMinValue} and {SeminarConstants.DurationMaxValue} minutes!");
                seminar.Categories = await GetCategories();
                return View(seminar);
            }

            DateTime date;

            if (!DateTime.TryParseExact(
               seminar.DateAndTime,
               SeminarConstants.SeminarDateFormat,
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out date))
            {
                ModelState
                    .AddModelError(nameof(seminar.DateAndTime), $"Invalid date! Format must be: {SeminarConstants.SeminarDateFormat}");
                return View(seminar);
            }

            if (!ModelState.IsValid)
            {
                seminar.Categories = await GetCategories();
                return View(seminar);
            }

            var seminarData = new Seminar
            {
                Topic = seminar.Topic,
                Lecturer = seminar.Lecturer,
                Details = seminar.Details,
                DateAndTime = date,
                Duration = duration,
                CategoryId = seminar.CategoryId,
                OrganizerId = GetUserId()
            };

            data.Seminars.Add(seminarData);
            await data.SaveChangesAsync();

            return RedirectToAction("All", "Seminar");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var seminar = await data
                .Seminars
                .Where(s => s.Id == id)
                .Select(s => new DetailsSeminarViewModel
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    DateAndTime = s.DateAndTime.ToString(SeminarConstants.SeminarDateFormat),
                    Duration = s.Duration,
                    Lecturer = s.Lecturer,
                    Category = s.Category.Name,
                    Details = s.Details,
                    Organizer = s.Organizer.UserName
                })
                .FirstOrDefaultAsync();

            if (seminar == null)
            {
                return NotFound();
            }

            return View(seminar);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var seminar = await data
                .Seminars
                .Where(s => s.Id == id)
                .Select(s => new DeleteSeminarViewModel
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    DateAndTime = s.DateAndTime.ToString(SeminarConstants.SeminarDateFormat)
                })
                .FirstOrDefaultAsync();

            if (seminar == null)
            {
                return NotFound();
            }

            return View(seminar);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seminar = await data
                .Seminars
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (seminar == null)
            {
                return NotFound();
            }

            data.Seminars.Remove(seminar);
            await data.SaveChangesAsync();

            return RedirectToAction("All", "Seminar");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var seminar = await data
                .Seminars
                .FindAsync(id);

            if (seminar == null)
            {
                return BadRequest();
            }

            if (seminar.OrganizerId != GetUserId())
            {
                return Unauthorized();
            }

            var seminarModel = new EditSeminarViewModel()
            {
                Topic = seminar.Topic,
                Lecturer = seminar.Lecturer,
                Details = seminar.Details,
                DateAndTime = seminar.DateAndTime.ToString(SeminarConstants.SeminarDateFormat),
                Duration = seminar.Duration.ToString(),
                CategoryId = seminar.CategoryId,
            };

            seminarModel.Categories = await GetCategories();

            return View(seminarModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSeminarViewModel model, int id)
        {
            var seminar = await data
                .Seminars
                .FindAsync(id);

            int duration;

            if (!int.TryParse(model.Duration, out duration) || duration < SeminarConstants.DurationMinValue || duration > SeminarConstants.DurationMaxValue)
            {
                ModelState
                    .AddModelError(nameof(seminar.Duration), $"Duration must be between {SeminarConstants.DurationMinValue} and {SeminarConstants.DurationMaxValue} minutes!");

                model.Categories = await GetCategories();

                return View(model);
            }
            DateTime date;

            if (!DateTime.TryParseExact(
                 model.DateAndTime,
                 SeminarConstants.SeminarDateFormat,
                 CultureInfo.InvariantCulture,
                 DateTimeStyles.None,
                 out date))
            {
                ModelState
                    .AddModelError(nameof(model.DateAndTime), $"Invalid date! Format must be: {SeminarConstants.SeminarDateFormat}");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategories();
                return View(seminar);
            }

            var seminarData = await data
                .Seminars
                .Where(s => s.Id == model.Id)
                .FirstOrDefaultAsync();

            if (seminarData == null)
            {
                return NotFound();
            }

            seminarData.Topic = model.Topic;
            seminarData.Lecturer = model.Lecturer;
            seminarData.Details = model.Details;
            seminarData.DateAndTime = date;
            seminarData.Duration = duration;
            seminarData.CategoryId = model.CategoryId;

            await data.SaveChangesAsync();

            return RedirectToAction("All", "Seminar");
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {

            var model = await data.Seminars.Where(s => s.Id == id)
                .Include(sp => sp.SeminarsParticipants)
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (!model.SeminarsParticipants.Any(sp => sp.ParticipantId == userId))
            {
                model.SeminarsParticipants.Add(new SeminarParticipant
                {
                    ParticipantId = userId,
                    SeminarId = id
                });

                await data.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Joined));
        }
        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            var userId = GetUserId();

            var seminars = await data
                .SeminarsParticipants
                .Where(sp => sp.ParticipantId == userId)
                .Select(sp => new JoinedSeminarViewModel
                {
                    Id = sp.Seminar.Id,
                    Topic = sp.Seminar.Topic,
                    Lecturer = sp.Seminar.Lecturer,
                    DateAndTime = sp.Seminar.DateAndTime.ToString(SeminarConstants.SeminarDateFormat),
                    Organizer = sp.Seminar.Organizer.UserName
                })
                .ToListAsync();

            return View(seminars);
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            var userId = GetUserId();

            var seminar = await data
                .SeminarsParticipants
                .Where(sp => sp.SeminarId == id && sp.ParticipantId == userId)
                .FirstOrDefaultAsync();

            if (seminar == null)
            {
                return BadRequest();
            }

            data.SeminarsParticipants.Remove(seminar);
            await data.SaveChangesAsync();

            return RedirectToAction("Joined", "Seminar");
        }
        private async Task<IEnumerable<CategoryViewModel>> GetCategories()
        {
            return await data
                .Categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

    }
}
