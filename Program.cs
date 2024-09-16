
using FECoreDemo2.NW;
using Microsoft.EntityFrameworkCore;

// instantiate the entry point of the database
NorthwindContext db = new(); // command + dot to add using directive
// GetAllCategories();
// GetAllCategoriesByFirstCharacterLambda("C");
// GetCategoriesInDescOrder();
// GetCategoriesInDescOrderSqlLike();
// GetProducts();
// GetProductsThreeColumns();
GetProductsWithCategoryName();

void GetAllCategories()
{
    var categories = db.Categories; // LINQ: Language independent query

    foreach (var c in categories)
    {
        Console.WriteLine($"{c.CategoryId}\t{c.CategoryName}\t{c.Description}");
    }
    Console.WriteLine("==============================");
}

void GetAllCategoriesByFirstCharacterLambda(string starts)
{
    var qry = db.Categories
      .Where(c => c.CategoryName.StartsWith(starts));

    foreach (var item in qry)
    {
        Console.WriteLine($"{item.CategoryId}\t{item.CategoryName}\t{item.Description}");
    }
}

void GetCategoriesInDescOrder()
{
    var qry = db.Categories
      .OrderByDescending(c => c.CategoryName);

    foreach (var item in qry)
    {
        Console.WriteLine($"{item.CategoryId}\t{item.CategoryName}\t{item.Description}");
    }
}

void GetCategoriesInDescOrderSqlLike()
{
    // upside down query sql syntax
    var categories = from c in db.Categories
                     orderby c.CategoryName descending
                     select c;

    foreach (var c in categories)
    {
        Console.WriteLine($"{c.CategoryId}\t{c.CategoryName}\t{c.Description}");
    }
}

void GetProducts()
{
    var products = db.Products;

    Console.WriteLine(products.ToQueryString()); // treating data as a class, this will show query sent to database

    foreach (var p in products)
    {
        Console.WriteLine($"{p.ProductId}\t{p.ProductName}\t{p.UnitPrice}");
    }
}

// To get three columns instead of all columns
void GetProductsThreeColumns()
{
    var products = db.Products;

    var qry = products.Select(p => new { p.ProductId, p.ProductName, p.UnitPrice });

    foreach (var p in qry)
    {
        Console.WriteLine($"{p.ProductId}\t{p.ProductName}\t{p.UnitPrice}");
    }
}

void GetProductsWithCategoryName()
{
    var products = db.Products.Select(p => new
    {
        p.ProductId,
        p.ProductName,
        p.UnitPrice,
        p.Category!.CategoryName // navigate into the category table, ! means it is not null
    });
    Console.WriteLine(products.ToQueryString());  // you can see join is used, but we don't need to write join

    foreach (var p in products)
    {
        Console.WriteLine($"{p.ProductId}\t{p.ProductName}\t{p.UnitPrice}\t{p.CategoryName}");
    }
}

void InsertCategorySP(string name, string desc) {
  var pName = new SqlParameter("@CategoryName", name);
  var pDesc = new SqlParameter("@Description", desc);

  var result = db.Database.ExecuteSqlRaw("dbo.CategoryInsert @CategoryName, @Description", pName, pDesc);
}

