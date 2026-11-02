using CadetTest.Entities;
using CadetTest.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CadetTest.Services
{
    public interface IDataService
    {
        string GetRandomString(int stringLength);
        Task<List<Consent>> GetRangeByIdAsync(int id, int adet);
        Task<Consent?> GetByIdAsync(int id);
        Task<Consent> CreateAsync(Consent consent);
        Task<Consent?> UpdateAsync(int id, Consent updatedConsent);
        Task<bool> DeleteAsync(int id);
    }

    public class DataService : IDataService
    {
        private readonly DataContext _context;

        public DataService(DataContext context)
        {
            _context = context;
        }

        #region Public Methods

        public async Task<List<Consent>> GetRangeByIdAsync(int id, int adet)
        {
            return await _context.Consents
                .AsNoTracking()
                .Where(k => k.Id >= id)
                .OrderBy(k => k.Id)
                .Take(adet)
                .ToListAsync();
        }

        public async Task<Consent?> GetByIdAsync(int id)
        {
            return await _context.Consents
                .AsNoTracking()
                .FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task<Consent> CreateAsync(Consent consent)
        {
            consent.Id = 0;

            _context.Consents.Add(consent);
            await _context.SaveChangesAsync();

            return consent;
        }

        public async Task<Consent?> UpdateAsync(int id, Consent updatedConsent)
        {
            var existing = await _context.Consents.FirstOrDefaultAsync(k => k.Id == id);
            if (existing == null) return null;

            existing.Type = updatedConsent.Type;
            existing.Recipient = updatedConsent.Recipient;
            existing.Status = updatedConsent.Status;
            existing.RecipientType = updatedConsent.RecipientType;

            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Consents.FirstOrDefaultAsync(k => k.Id == id);
            if (existing == null) return false;

            _context.Consents.Remove(existing);
            await _context.SaveChangesAsync();

            return true;
        }

        public string GetRandomString(int stringLength)
        {
            var sb = new StringBuilder();
            int numGuidsToConcat = (((stringLength - 1) / 32) + 1);

            for (int i = 1; i <= numGuidsToConcat; i++)
            {
                sb.Append(Guid.NewGuid().ToString("N"));
            }

            return sb.ToString(0, stringLength);
        }

        #endregion
    }
}