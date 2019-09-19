using ChatRoom.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChatRoom.API
{
    public static class DataSeeder
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
           .GetRequiredService<IServiceScopeFactory>()
           .CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ChatRoomContext>();

                context.Database.Migrate();
            }
        }
    }
}
