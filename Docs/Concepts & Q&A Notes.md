# Employee Management System (EMS)
## Concepts & Q&A Notes

A running reference of concepts learned and questions answered during development.

---

## MODULE 1 — DATABASE & ENTITY FRAMEWORK

### Q1: What is `virtual` on a navigation property?
```csharp
public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
```
`virtual` allows Entity Framework to override this property at runtime with its own "proxy" version that can automatically load data from the database when you access it — this is called **lazy loading**. Without `virtual`, EF loads nothing for related data unless you explicitly ask for it with `.Include()`.

`ICollection<Employee>` is an interface meaning "a collection of Employee objects." We use the interface rather than `List` directly because EF can swap in its own internal collection type behind the scenes. We assign `= new List<Employee>()` so it's never null.

Think of it like: `ICollection` is the job description, `List` is the worker doing the job.

---

### Q2: What does `get; set;` mean?
`{ get; set; }` is shorthand for a C# property. The compiler automatically generates a hidden private variable (called a backing field) to store the value.

```csharp
// What you write:
public string Name { get; set; }

// What the compiler generates behind the scenes:
private string _name;
public string Name
{
    get { return _name; }    // reading:  var x = employee.Name
    set { _name = value; }   // writing:  employee.Name = "Ali"
}
```
You can restrict access:
- `{ get; }` — read-only property
- `{ get; private set; }` — readable anywhere, writable only inside the class

---

### Q3: What is `modelBuilder`?
`modelBuilder` is an object Entity Framework passes into `OnModelCreating()`. It's EF's configuration panel — you use it to define things that can't be expressed with data annotations alone:

```csharp
// Unique constraint — no two departments can have the same name
entity.HasIndex(d => d.DepartmentName).IsUnique();

// Relationship — one Department has many Employees
entity.HasOne(e => e.Department)
      .WithMany(d => d.Employees)
      .HasForeignKey(e => e.DepartmentId)
      .OnDelete(DeleteBehavior.Restrict);
```

---

### Q4: What is `base`?
`base` refers to the parent class. When your class inherits from another (the `: DbContext` part), calling `base(options)` in the constructor runs the parent class's constructor first, so EF's own internal setup happens before yours.

```csharp
public ApplicationDbContext(DbContextOptions options) : base(options)
// ↑ "Parent class (DbContext), set yourself up first with these options"
```

Same idea in `OnModelCreating` — `base.OnModelCreating(modelBuilder)` lets EF do its default wiring before your custom rules run on top.

---

### Q5: What is EF (Entity Framework)?
EF is a library that lets you interact with your database using C# code instead of writing raw SQL. You write C# model classes, EF reads them and generates the SQL automatically.

```csharp
// Instead of writing SQL:
// SELECT * FROM Employees WHERE DepartmentId = 1

// You write C#:
var employees = _context.Employees
    .Where(e => e.DepartmentId == 1)
    .ToList();
```

EF also handles migrations — it reads your model classes and auto-generates the SQL `CREATE TABLE` statements when you run `dotnet ef database update`.

---

---

## MODULE 2 — DEPARTMENT MANAGEMENT

### Q1: `readonly` but assigned in the constructor?
```csharp
private readonly ApplicationDbContext _context;

public DepartmentController(ApplicationDbContext context)
{
    _context = context;   // ← how is this allowed?
}
```
`readonly` does NOT mean "never writable." It means "can only be assigned once, and only during object construction." The constructor is the one permitted place to assign it. After the constructor finishes, `_context` is permanently locked — you can't reassign it anywhere else in the class. This is intentional: you never want the database connection accidentally swapped out mid-request.

---

### Q2: How does `@section Scripts` handle validation?
It's a two-part automatic system:

**Part 1 — Server side (when Razor renders the page):**
Your model has data annotations like `[Required]` and `[StringLength]`. The `asp-for` and `asp-validation-for` tag helpers read these annotations and generate HTML attributes on the input elements:
```html
<input data-val="true"
       data-val-required="Department name is required"
       data-val-length-max="100"
       name="DepartmentName" ... />
<span data-valmsg-for="DepartmentName" ...></span>
```

**Part 2 — Client side (in the browser):**
`_ValidationScriptsPartial` loads two jQuery libraries:
- `jquery.validate.js` — the validation engine
- `jquery.validate.unobtrusive.js` — reads the `data-val-*` attributes and wires up rules automatically

The libraries scan the page, find all `data-val-*` attributes, and attach validation rules to each field. You write zero custom JavaScript — it's all driven by the HTML attributes generated from your C# annotations.

```
[Required] on C# model
    → asp-validation-for generates data-val-required="..." on the HTML input
        → jQuery validation reads that attribute
            → Shows error message before form even submits
```

---

### Q3: What is `ModelState`? How does it show errors?
`ModelState` is a dictionary ASP.NET automatically builds when a form is submitted. It holds every field's submitted value and any validation errors.

**How it gets populated:**
1. Form submits → ASP.NET checks all data annotations on the model
2. Any failures are added to ModelState automatically
3. You can manually add extra errors too:
```csharp
ModelState.AddModelError("DepartmentName", "A department with this name already exists.");
```

**How errors reach the view:**
`ModelState.IsValid` returns `true` only when zero errors exist. If it's `false`, we return the same view with the model — and `asp-validation-for` tag helpers read from ModelState to display each error next to its field:
```html
<span asp-validation-for="DepartmentName" class="text-danger"></span>
```
This span is empty on first load, but filled with the error message after a failed submission.

---

### Q4: What is `RedirectToAction(nameof(Index))`?
`RedirectToAction` sends an HTTP 302 response to the browser, instructing it to navigate to a different URL — in this case `/Department/Index`.

`nameof(Index)` is a C# compile-time operator that returns the string `"Index"`. It's safer than writing `"Index"` as a plain string because if you ever rename the method, the compiler throws an error instead of silently breaking.

```csharp
// These are equivalent, but nameof is safer:
return RedirectToAction("Index");
return RedirectToAction(nameof(Index));
```

---

### Q5: How does `asp-action` + `asp-route-id` build the URL?
These are **Razor Tag Helpers** — they run on the server before the HTML reaches the browser.

```html
<a asp-action="Edit" asp-route-id="@dept.DepartmentId">Edit</a>
```
The Tag Helper reads your route configuration from `Program.cs` (`{controller}/{action}/{id?}`) and generates:
```html
<a href="/Department/Edit/5">Edit</a>
```
`asp-route-id` maps to the `{id}` segment in the route pattern. You never write URLs manually — the Tag Helper handles it, so if your routes ever change, the links update automatically.

---

### Q6 & Q10: What is `@Html.AntiForgeryToken()`?
It renders a hidden `<input>` inside your form containing a unique encrypted token tied to your current session:
```html
<input type="hidden" name="__RequestVerificationToken" value="CfDJ8..." />
```
When the form is submitted, `[ValidateAntiForgeryToken]` on the controller checks that this token is present and matches the expected value. If it's missing or wrong, the request is rejected with a 400 error.

Note: When you use `<form asp-action="...">` (the tag helper version), the anti-forgery token is injected automatically. `@Html.AntiForgeryToken()` is the older manual way — both do the same thing.

---

### Q7: What is a Cross-Site Request Forgery (CSRF) attack?
Imagine you're logged into your bank. You then visit a malicious website. That site has a hidden form that points to your bank's "transfer money" URL. When the page loads, JavaScript auto-submits the form. Your browser sends the request along with your real session cookie — and the bank executes the transfer.

The anti-forgery token stops this because the malicious site has no way to read or guess your unique token (browsers block cross-origin reads). The bank's server rejects any request without the correct token.

```
Malicious site → POST /Department/Delete/5 (no token)
    → [ValidateAntiForgeryToken] rejects it ✅

Your own form → POST /Department/Delete/5 (with correct token)
    → [ValidateAntiForgeryToken] accepts it ✅
```

---

### Q8: How is the "Active" checkbox pre-checked?
Two things work together:

**Step 1 — Default value in the model:**
```csharp
// Department.cs
public bool ActiveInactive { get; set; } = true;
```

**Step 2 — Passing a new object from the GET action:**
```csharp
public IActionResult Create()
{
    return View(new Department());  // ActiveInactive is already true
}
```
The `asp-for="ActiveInactive"` tag helper reads the value from the model object. Since it's `true`, it renders:
```html
<input type="checkbox" checked ... />
```
Without passing `new Department()`, the view has no model to read from — the checkbox defaults to unchecked, and `false` gets saved to the database.

---

### Q9: What does `@` mean in `@TempData["SuccessMessage"]`?
In `.cshtml` Razor files, `@` switches from HTML mode into C# mode. The C# expression after `@` is evaluated and its result is written directly into the HTML output.

```html
@* These are all valid uses of @ *@
@TempData["SuccessMessage"]        → outputs the string value
@dept.DepartmentName               → outputs a property value
@dept.CreatedDate.ToString("MMM dd, yyyy")  → calls a method and outputs result
@if (dept.ActiveInactive) { ... }  → C# control flow in HTML
@foreach (var dept in Model) { }   → C# loop in HTML
```

`TempData` specifically is a dictionary that persists data for exactly one redirect. You set it in the controller after saving, and it's available once in the very next request (the redirected page). After that it's gone automatically.

---

## QUICK REFERENCE — KEY TERMS

| Term | What it is |
|------|-----------|
| `EF / Entity Framework` | Library to interact with DB using C# instead of SQL |
| `DbContext` | The bridge between C# and the database |
| `Migration` | EF-generated SQL script to create/update tables |
| `ModelState` | Dictionary of form field values and validation errors |
| `TempData` | One-request data store — survives exactly one redirect |
| `async/await` | Wait for DB without freezing the app |
| `IActionResult` | Return type that can be View, Redirect, NotFound, etc. |
| `[HttpPost]` | Action only runs on POST requests (form submissions) |
| `readonly` | Variable can only be assigned once, in the constructor |
| `nameof()` | Compile-time operator that returns a name as a string |
| `Tag Helpers` | Razor attributes (asp-for, asp-action) that generate HTML |
| `Anti-Forgery Token` | Hidden security token that prevents CSRF attacks |
| `Soft Delete` | Mark record as inactive instead of removing from DB |
| `Hard Delete` | Permanently remove a record from the database |
| `Lazy Loading` | EF only loads related data when you access the property |

---

**Last Updated:** February 18, 2026