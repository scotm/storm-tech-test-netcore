# Solution Notes

## Answer 1

Straightforward enough. Installed the .NET tools for MacOS, cloned the repository, restored the project, ran the provided tools to create the database.

Ended up using the command line, mostly. `dotnet watch --project Todo` gives me hot reloading and a fast development experience.

## Answer 2

I ended up converting the Items Collection into an Array, then used the `Array.Sort` method.

```csharp
    // I'm sure there's a better, more idiomatic method of doing this. I'm not a C# dev. Yet.
    var items = Model.Items.ToArray();

    // Use Array sort method to sort items by Importance
    Array.Sort(items, (x, y) => x.Importance.CompareTo(y.Importance));
```

I'm sure there's a better way of doing this using the Entity framework. It might work within the SingleTodoList method in `ApplicationDbContextConvenience.cs` with the OrderBy chained method.

Example:

```csharp
OrderBy(x => x.Col1).ThenBy(x => x.Col2)
```

But the problem specified to make the changes to `Views/TodoList/Detail.cshtml`. So I've left that as is for now.

## Answer 3

Ran the unit tests, noted the failing method, then followed the setup method up to the faulty `TodoItemEditFields` constructor. I then added the correct `Importance` field as passed into the constructor and the unit test passed.

## Answer 4

Initially just changed the text in the View `label` on `TodoItem/Create.cshtml` and `TodoItem/Edit.cshtml` - then guessed there was a better DRY way, like Django has in its model definitions. A quick google tells me that this is called a "Data Annotation" - it looks like a Python decorator.

```csharp
    [Display(Name = "Who is going to do this?")]
    public string ResponsiblePartyId { get; set; }
```

Did the same thing with the IsDone field. Looks nicer.

Not sure how to test how or if these changes end up on the rendered View. Leaving tests alone for now.

## Answer 5

"On the details page, add an option to hide items that are marked as done."

Wasn't clear what 'an option' was. I added a button to the bottom of the todo list with an onclick handler, that sets the class of the done item list elements to 'hidden'. A little bit of Bootstrap 3 magic.

## Answer 6

Okay. A little bit of reading on the dbContext and the Entity Framework. I added another static method in the `ApplicationDbContextConvenience.cs` file and copied the provided code from RelevantTodoLists.

I then changed the .Where predicate using an inclusive or - the Items.Any() method should also include other ToDo lists that had items with the ResponsibleParty's userId.

```csharp
  public static IQueryable<TodoList> ReplacementRelevantToDoLists(this ApplicationDbContext dbContext, string userId)
  {
      return dbContext.TodoLists.Include(tl => tl.Owner)
          .Include(tl => tl.Items)
          .Where(tl => tl.Owner.Id == userId || tl.Items.Any(ti => ti.ResponsibleParty.Id == userId));
  }
```

I reckon this could be quite powerful on the backend - with a bit of time to appreciate what Entity Framework can do.

## Answer 7

Some reading up on the Entity Framework and migrations.

- Before starting I created an initial migration, with `dotnet ef migrations add InitialMigration`.
- Added a Rank integer field to the TodoItem Entity class with getter/setter methods. Ran the `dotnet ef migrations add AddRank` command. Then ran the `dotnet ef database update` command.

Ended up doing some pair programming with the compiler, views and tests to add Rank to the places it was needed. Wasn't sure if I was getting this right - it's still a bit baffling. But it worked, eventually.

I've added a Button to `Views/TodoList/Detail.cshtml` and some scaffolding, which adds a query param. When the button is clicked, the TodoListItems are sorted in descending order by Rank.

Added a test case to ensure that WhenTodoItemIsConvertedToEditFields checks Rank, too.

## Answer 8

I don't think we should do this as part of page load. If Gravatar is down, then we cannot retrieve the user's profile, and the entire page fails. Failed images are acceptable, the page itself failing is not.

Therefore, this enhancement is best done on the client side - deliver the required user identifers/graphic links on pageload, and replace the email address/username on DOMContentLoaded.

- Use the browser's fetch api to send a request containing an MD5 hash of the email address to the Gravatar API.
- On promise fulfilment, deserialise the JSON into an object, and replace the email addresses with the appropriate name property.

If the promise fails, catch it and let it pass - you've still got the normal identifier.

But. This didn't work. When asking gravatar.com directly - we got the following error message.

```
Access to fetch at 'https://www.gravatar.com/5f6f3999b09d89f9477fc5559927c6e1.json' from origin 'https://localhost:5001' has been blocked by CORS policy: No 'Access-Control-Allow-Origin' header is present on the requested resource. If an opaque response serves your needs, set the request's mode to 'no-cors' to fetch the resource with CORS disabled.
```

-->

CORS means we probably need to implement this as a server-API endpoint. So, I've written a GravatarController.cs file, which performs a fetch on our behalf and returns a JSON object with the displayName and the Gravatar URL.

_NOTE_: This solution is pretty brittle. If the app is refactored to not use Email as an identifier, then this needs to be changed. I've left it as is for now.
