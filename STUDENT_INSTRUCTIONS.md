# Lab (Advanced): Fix the Service–Repository App

This ASP.NET Core MVC app is supposed to follow the **Service–Repository pattern**,
but it has been broken. Your job is to find and fix every bug.

There are **30 bugs**. This version **compiles** (with a warning or two), so the compiler
won't find them for you. Every bug is a runtime/logic bug or a **design violation**: code
that sits in the wrong layer or wires the layers together incorrectly. Some bugs only
become visible after you fix another one, so re-test after every change.

## Do NOT change these (they are correct)

- `Models/Product.cs`
- `Data/AppDbContext.cs`
- `Program.cs`
- `appsettings.json` (except your own connection string)
- `ServiceRepoDemo.csproj`

## Setup

1. Set `ConnectionStrings:DefaultConnection` in `appsettings.json`.
2. Run:

       dotnet ef migrations add InitialCreate
       dotnet ef database update
       dotnet run

## Architecture rules

    Controller  ->  IProductService  ->  IProductRepository  ->  AppDbContext

- **Controllers** depend only on `IProductService`. They must not use `AppDbContext`,
  hold data of their own, or contain business rules. They handle HTTP only: model
  validation, choosing views, redirects and messages.
- **Services** contain all business rules and depend on `IProductRepository`
  through constructor injection. Nothing is created with `new` that DI should supply.
- **Repositories** contain data access only (EF Core queries, add/update/remove, save).

## Required behavior

1. The product list is sorted by **name, A–Z**, and always shows the current data
   (a change is visible as soon as you return to the list).
2. Leading and trailing spaces are removed from product names on create **and** edit.
3. Product names must be **unique** on create **and** edit (after trimming).
4. `CreatedAt` is set once, by the **service**, in **UTC**, when a product is created,
   and never changes after that.
5. Create, Edit and Delete all actually save every field to the database.
6. When a save fails a business rule, the form is shown again with the user's input
   still filled in and the error displayed on the form.
7. After a successful Create, Edit or Delete, the user is **redirected** to the product
   list, and the success message appears **once** (not again on the next page).
8. A product can only be deleted when its stock is exactly **0**.
9. Client-side validation works on the Create and Edit forms (errors appear before
   the form is submitted).
10. All links and forms work: Edit opens the right product, Edit saves that product,
    and Delete deletes it.

## Tips

- Use the browser's developer tools (Network and Console tabs) and the app's log output.
- Check the database directly (e.g. SSMS) to see what was really saved.
- Test the edge cases: names with spaces, duplicate names, editing without changing the
  name, stock of 0 vs. 1, refreshing a page after saving.
- For each bug you fix, write down the file, what was wrong, the symptom, and your fix.


Im at the number 5

1. I installed dotnet ef to update the database and changed the connection strings.
2. On ProductRepository.cs, I removed the _context.ChangeTracker.Clear(); as this deletes the entity to be saved into the db.
3. Changed the Index wherein it only returns cached meaning it is not reflective of what is in the database.
4. Changed the DI in Product service
5. Changed the form asp-action into Edit in Edit views
6. Also changed the asp-route-productId into asp-route-id to match what the controller
7. In ProductService.cs I changed the _repository.Update(product) into _repository.Update(existing) 
   which removed the another instance with the same key value for {'Id'} is already being tracked.
8. In the ProductsController, I changed the return of the Edit method into return RedirectToAction(nameof(Index));
   This is so that it goes back to index after update.
9. I added this condition in controller in Edit method so that it checks for duplicate entries
if (await _context.Products.AnyAsync(p => p.Name == product.Name))
        {
            ModelState.AddModelError(string.Empty, $"A product named '{product.Name}' already exists.");
            return View(product);
        }
10. Added trimming on Edit in the controller so that leading and trailing whitespaces are removed
11. In _Forms.cshtml, I updated the stock so that it correctly updates and saves in the database.
12. Added the Delete interface in the IProductService
13. Implemented the delete service interface on product service
14. Moved some of the logics in Create method in the controller onto the service layer CreateAsync method.
15. Same case I did in the Edit method, I moved some of the logic in the updateasync
16. Implemented the Delete method in the ProductService.cs

