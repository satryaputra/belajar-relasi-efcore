using EFCoreRelationship.Data;
using EFCoreRelationship.Dtos;
using EFCoreRelationship.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreRelationship.Controllers;

[Route("character")]
[ApiController]
public class CharacterController : ControllerBase
{
    private readonly DataContext _db;
    
    public CharacterController(DataContext context)
    {
        _db = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetCharactersByUserId(int userId)
    {
        var characters = await _db.Characters
            .Where(c => c.UserId == userId)
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .ToListAsync();
        return Ok(characters); 
    }
    
    [HttpPost]
    public async Task<IActionResult> AddCharacter(AddCharacterDto request)
    {
        var user = await _db.Users.FindAsync(request.UserId);
        if (user == null) return NotFound();

        var newCharacter = new Character
        {
            Name = request.Name,
            RpgClass = request.RpgClass,
            UserId = request.UserId
        };

        _db.Characters.Add(newCharacter);
        await _db.SaveChangesAsync();

        return await GetCharactersByUserId(request.UserId);
    }
    
    [HttpPost("weapon")]
    public async Task<ActionResult<Character>> AddWeapon(AddWeaponDto request)
    {
        var character = await _db.Characters.FindAsync(request.CharacterId);
        if (character == null) return NotFound();

        var newWeapon = new Weapon
        {
            Name = request.Name,
            Damage = request.Damage,
            CharacterId = request.CharacterId
        };

        _db.Weapons.Add(newWeapon);
        await _db.SaveChangesAsync();

        return character;
    }
    
    [HttpPost("skill")]
    public async Task<ActionResult<Character>> AddSkill(AddCharacterSkillDto request)
    {
        var character = await _db.Characters
            .Where(c => c.Id == request.CharacterId)
            .Include(c => c.Skills)
            .FirstOrDefaultAsync();
        if (character == null) return NotFound();
        
        var skill = await _db.Skills.FindAsync(request.SkillId);
        if (skill == null) return NotFound();
        
        character.Skills.Add(skill);
        await _db.SaveChangesAsync();

        return character;
    }
}