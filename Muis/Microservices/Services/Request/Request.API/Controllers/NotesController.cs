using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;
using Microsoft.AspNetCore.Mvc;
using Request.API.Services;
using Request.Model;

namespace Request.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class NotesController : Controller
  {
    public INotesService _notesService;

    public NotesController (INotesService notesService)
    {
      _notesService = notesService;
    }

    [HttpGet]
    public async Task<IList<Notes>> GetNotes(long requestID)
    {
      return await _notesService.ListOfNotes(requestID);
    }

    [HttpPost]
    public async Task<Notes> AddNotes ([FromBody] Notes notes, Guid userID, string username)
    {
      return await _notesService.AddNotes(notes, new Officer(userID, username));
    }
  }
}
