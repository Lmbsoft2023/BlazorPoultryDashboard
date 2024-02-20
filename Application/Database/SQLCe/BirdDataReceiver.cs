using BlazorPoultryDashboard.Models;
using Microsoft.EntityFrameworkCore;
using static MudBlazor.CategoryTypes;

namespace BlazorPoultryDashboard.Application.Database.SQLCe
{
    public class BirdDataReceiver
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public BirdDataReceiver(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<IEnumerable<Bird>> GetBirdsAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BirdDbContext>();
                var birds = await context.Birds.ToListAsync();
                //Return Enumerable.Empty instead of null.
                return birds.Count() >= 1 ? birds : Enumerable.Empty<Bird>()!;
            }
        }
    }
}
