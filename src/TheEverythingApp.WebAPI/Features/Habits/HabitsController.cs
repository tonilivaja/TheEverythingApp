using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheEverythingApp.Application.Features.Habits;

namespace TheEverythingApp.WebAPI.Features.Habits;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class HabitsController : ControllerBase
{
    private readonly IHabitService _habitService;

    public HabitsController(IHabitService habitService)
    {
        _habitService = habitService;
    }

    // ── CRUD ─────────────────────────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = false)
    {
        var habits = await _habitService.GetAllAsync(includeInactive);
        return Ok(habits);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var habit = await _habitService.GetByIdAsync(id);
        return habit is null ? NotFound() : Ok(habit);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateHabitRequest request)
    {
        var habit = await _habitService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = habit.Id }, habit);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateHabitRequest request)
    {
        var habit = await _habitService.UpdateAsync(id, request);
        return habit is null ? NotFound() : Ok(habit);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _habitService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    // ── ENTRIES ───────────────────────────────────────────────────────────────

    [HttpPost("{id:int}/entries")]
    public async Task<IActionResult> LogEntry(int id, LogEntryRequest request)
    {
        try
        {
            var entry = await _habitService.LogEntryAsync(id, request);
            return CreatedAtAction(nameof(GetEntries), new { id }, entry);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}/entries/{entryId:int}")]
    public async Task<IActionResult> DeleteEntry(int id, int entryId)
    {
        var deleted = await _habitService.DeleteEntryAsync(id, entryId);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("{id:int}/entries")]
    public async Task<IActionResult> GetEntries(
        int id,
        [FromQuery] DateOnly? from = null,
        [FromQuery] DateOnly? to = null)
    {
        var entries = await _habitService.GetEntriesAsync(id, from, to);
        return Ok(entries);
    }

    // ── STATISTICS ────────────────────────────────────────────────────────────

    [HttpGet("{id:int}/stats")]
    public async Task<IActionResult> GetStats(int id)
    {
        var stats = await _habitService.GetStatsAsync(id);
        return stats is null ? NotFound() : Ok(stats);
    }
}
