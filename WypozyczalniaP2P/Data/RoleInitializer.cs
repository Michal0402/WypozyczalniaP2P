using Microsoft.AspNetCore.Identity;
using WypozyczalniaP2P.Models;

namespace WypozyczalniaP2P.Data
{
    public class RoleInitializer
    {
        
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roleNames = { "Administrator", "Pracownik", "Klient" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Konto domyślnego admina
            var adminEmail = "mAdmin@gmail.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new Administrator
                {
                    UserName = "mAdmin",
                    Email = adminEmail,
                    Imie = "Michal",
                    Nazwisko = "Wyrembek",
                    PhoneNumber = "111111111",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Michal0402@");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Administrator");
                    Console.WriteLine("Admin dodany pomyślnie.");
                }
                
            }

            // Konto domyślnego pracownika
            var pracownikEmail = "mPracownik@gmail.com";
            if (await userManager.FindByEmailAsync(pracownikEmail) == null)
            {
                var pracownik = new Pracownik
                {
                    UserName = "mPracownik",
                    Email = pracownikEmail,
                    Imie = "Michal",
                    Nazwisko = "Wyrembek",
                    PhoneNumber = "222222222",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(pracownik, "Michal0402@");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(pracownik, "Pracownik");
                }
            }

            // Konto domyślnego klienta

            var klientEmail = "mKlient@gmail.com";
            if (await userManager.FindByEmailAsync(klientEmail) == null)
            {
                var klient = new Klient
                {
                    UserName = "mKlient",
                    Email = klientEmail,
                    Imie = "Michal",
                    Nazwisko = "Wyrembek",
                    PhoneNumber = "333333333",
                    NumerPrawaJazdy = "12345678910111213",
                    Adres = "ul. Testowa 1",
                    Miejscowosc = "Testowo",
                    KodPocztowy = "12-345",
                    DataUrodzenia = new DateTime(1999, 4, 2),
                    EmailConfirmed = true

                };

                var result = await userManager.CreateAsync(klient, "Michal0402@");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(klient, "Klient");
                }
            }

            klientEmail = "WynajmujacyKlient@gmail.com";
            if (await userManager.FindByEmailAsync(klientEmail) == null)
            {
                var klient = new Klient
                {
                    UserName = "WynajmujjacyKlient",
                    Email = klientEmail,
                    Imie = "Marek",
                    Nazwisko = "Nowak",
                    PhoneNumber = "444444444",
                    NumerPrawaJazdy = "11111111111111111",
                    Adres = "ul. Stycznia 1",
                    Miejscowosc = "Styczniowo",
                    KodPocztowy = "12-346",
                    DataUrodzenia = new DateTime(2003, 4, 2),
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(klient, "Michal0402@");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(klient, "Klient");
                }
            }
        }
        
    }
}
