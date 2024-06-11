using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<bool> DoesCharacterExist(int id)
    {
        return await _context.Characters.AnyAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<CharacterInfoDto>> GetCharacterInfo(int id)
    {
        var characters = await _context.Characters
            .Where(c => c.Id == id)
            .Include(c => c.Backpacks)
                .ThenInclude(b => b.Item)
            .Include(c => c.CharacterTitles)
                .ThenInclude(ct => ct.Title)
            .ToListAsync();

        return characters.Select(c => new CharacterInfoDto
        {
            FirstName = c.FirstName,
            LastName = c.LastName,
            CurrentWeight = c.CurrentWeight,
            MaxWeight = c.MaxWeight,
            BackpackItems = c.Backpacks.Select(b => new BackpackItemDto
            {
                ItemName = b.Item.Name,
                ItemWeight = b.Item.Weight,
                Amount = b.Amount
            }).ToList(),
            Titles = c.CharacterTitles.Select(t => new CharacterTitleDto
            {
                Title = t.Title.Name,
                AcquiredAt = t.AcquiredAt
            }).ToList()
        });
    }

    public async Task<bool> DoesItemExist(int id)
    {
        return await _context.Items.AnyAsync(i => i.Id == id);
    }

    public async Task<bool> CanCharacterCarryMore(int id, List<int> itemIds)
    {
        var character = await _context.Characters
            .Where(c => c.Id == id)
            .Select(c => new { c.CurrentWeight, c.MaxWeight })
            .FirstOrDefaultAsync();

        if (character == null)
        {
            return false;
        }

        var itemsWeight = await CalculateItemsWeight(itemIds);

        return itemsWeight + character.CurrentWeight <= character.MaxWeight;
    }

    public async Task AddItemsToCharacter(int characterId, List<int> itemIds)
    {
        var backpacks = itemIds.Select(itemId => new Backpack
        {
            CharacterId = characterId,
            ItemId = itemId
        });

        await _context.Backpacks.AddRangeAsync(backpacks);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCharactersWeight(int id, List<int> itemIds)
    {
        var itemsWeight = await CalculateItemsWeight(itemIds);

        var character = await _context.Characters.FindAsync(id);

        if (character != null)
        {
            character.CurrentWeight += itemsWeight;
            await _context.SaveChangesAsync();
        }
    }

    private async Task<int> CalculateItemsWeight(List<int> itemIds)
    {
        return await _context.Items
            .Where(i => itemIds.Contains(i.Id))
            .SumAsync(i => i.Weight);
    }
}
