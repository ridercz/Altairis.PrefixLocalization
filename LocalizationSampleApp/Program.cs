using Altairis.PrefixLocalization;

/* Register services to the IoC/DI container *********************************/
var builder = WebApplication.CreateBuilder(args);

// Register Razor Pages
builder.Services.AddRazorPages();

// Register prefix localization library
builder.Services.AddPrefixLocalization(options => {
    // Define supported locales
    options.LocaleMappings.Add(new PrefixToCultureMapping("cesky", "cs-CZ", "cs"));
    options.LocaleMappings.Add(new PrefixToCultureMapping("english", "en-US", "en"));
    options.LocaleMappings.Add(new PrefixToCultureMapping("deutsch", "de-DE", "de"));
    // Add ignored paths
    options.IgnorePaths.Add("^/content/.+");
});

/* Configure the application **********************************************/
var app = builder.Build();

// Always use prefix localization as a first middleware (with possible exception below)
app.UsePrefixLocalization();

// You may use static middleware as a first one, if you want all static files excluded from localization
app.UseStaticFiles();

// This is not exactly needed, but it helps to see error codes
app.UseStatusCodePagesWithReExecute("/english/Errors/{0}");

// Map razor pages
app.MapRazorPages();

/* Run the application ***************************************************/
await app.RunAsync();