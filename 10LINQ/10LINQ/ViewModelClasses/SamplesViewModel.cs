﻿using _10LINQ.EntityClasses;
using _10LINQ.RepositoryClasses;
using System.Text;

namespace _10LINQ.ViewModelClasses
{
    public class SamplesViewModel
    {
        public List<Product> Products { get; set; }
        public List<SalesOrderDetail> Sales { get; set; }
        public bool UseQuerySyntax { get; set; }
        public string ResultText { get; set; }

        public SamplesViewModel()
        {
            Products = ProductRepository.GetAll();
            Sales = SalesOrderRetailRepository.GetAll();
        }

        // Put all products into a collection by looping
        public void GetAllLooping()
        {
            List<Product> list = new List<Product>();
            foreach(Product item in Products)
            {
                list.Add(item);
            }

            ResultText = $"Total products: {list.Count}";
        }

        // Put all products into a collection using LINQ
        public void GetAll()
        {
            List<Product> list = new List<Product>();

            if (UseQuerySyntax)
            {
                // Query Syntax
                // from <some_variable_name> in <some_collection> select <what_you_want_to_select>
                // in this case we want to select each product object, which in our case we named "prod", so we return the whole thing
                list = (from prod in Products select prod).ToList();
            }
            else
            {
                // Method syntax
                list = Products.Select(prod => prod).ToList();
            }

            ResultText = $"Total products: {list.Count}";
        }

        // Select a single column
        public void GetSingleColumn()
        {
            StringBuilder sb = new StringBuilder(1024);
            List<string> list = new List<string>();

            if (UseQuerySyntax)
            {
                // Query Syntax
                list.AddRange(from prod in Products select prod.Name);
            }
            else 
            {
                // Method syntax
                list.AddRange(Products.Select(prod => prod.Name));
            }

            foreach(string item in list)
            {
                sb.AppendLine(item);
            }

            ResultText = $"Total products: {list.Count}" + Environment.NewLine + sb.ToString();
            Products.Clear();
        }

        // Get specific columns
        // Select a few specific properties from products and create new Product object
        public void GetSpecificColumns()
        {
            if (UseQuerySyntax)
            {
                // Query Syntax
                // overwrite the Products variable which is filled with all the Products, with only a selected few of the properties
                Products = (from prod in Products
                            select new Product
                            {
                                ProductID = prod.ProductID,
                                Name = prod.Name,
                                Size = prod.Size,
                            }).ToList();
            }
            else
            {
                // Method syntax
                Products.Select(prod => new Product { ProductID = prod.ProductID, Name = prod.Name, Size = prod.Size });
            }
            ResultText = $"Total products: {Products.Count}";
        }

        // Create an anonymous class with the selected product properties
        public void AnonymousClass()
        {
            StringBuilder sb = new StringBuilder(2048);
            if (UseQuerySyntax)
            {
                // Query syntax
                var products = (from prod in Products
                                          select new
                                          {
                                              Identifier = prod.ProductID,
                                              ProductName = prod.Name,
                                              ProductSize = prod.Size
                                          });

                // Loop through anonymous class
                foreach (var prod in products)
                {
                    sb.AppendLine($"Product ID: {prod.Identifier}");
                    sb.AppendLine($"   Product Name: {prod.ProductName}");
                    sb.AppendLine($"   Product Size: {prod.ProductSize}");
                }
            }
            else
            {
                // Method syntax
                var products = Products.Select(prod => new
                {
                    Identifier = prod.ProductID,
                    ProductName = prod.Name,
                    ProductSize = prod.Size
                });

                // Loop through anonymous class
                foreach (var prod in products)
                {
                    sb.AppendLine($"Product ID: {prod.Identifier}");
                    sb.AppendLine($"   Product Name: {prod.ProductName}");
                    sb.AppendLine($"   Product Size: {prod.ProductSize}");
                }
            }
            ResultText = sb.ToString();
            Products.Clear();
        }

        // Order products by name
        public void OrderBy()
        {
            if (UseQuerySyntax)
            {
                // query syntax
                Products = (from prod in Products orderby prod.Name select prod).ToList();
            }
            else
            {
                // method syntax
                Products = Products.OrderBy(prod => prod.Name).ToList();
            }
            ResultText = $"Total products: {Products.Count}";
        }

        // Order products by name in descending order
        public void OrderByDescending()
        {
            if (UseQuerySyntax)
            {
                // query syntax
                Products = (from prod in Products orderby prod.Name descending select prod).ToList();
            }
            else
            {
                // method syntax
                Products = Products.OrderByDescending(prod => prod.Name).ToList();
            }
            ResultText = $"Total products: {Products.Count}";
        }

        // Order products by Color descending, then Name
        public void OrderByTwoFields()
        {
            if (UseQuerySyntax)
            {
                // query syntax
                // don't need to use the ascending keyword since that's the default order, it's just for clarity
                Products = (from prod in Products orderby prod.Color descending, prod.Name ascending select prod).ToList();
            }
            else
            {
                // method syntax
                Products = Products.OrderByDescending(prod => prod.Color).ThenBy(prod => prod.Name).ToList();
            }
            ResultText = $"Total products: {Products.Count}";
        }

        // Filter products using where. If the data is not found, return empty list
        public void WhereExpression()
        {
            string search = "L";

            if (UseQuerySyntax)
            {
                // query syntax
                Products = (from prod in Products where prod.Name.StartsWith(search) select prod).ToList();
            }
            else
            {
                // method syntax
                Products = Products.Where(prod => prod.Name.StartsWith(search)).ToList();
            }
            ResultText = $"Total products: {Products.Count}";
        }

        // Filter products using where with two fields
        public void WhereTwoFields()
        {
            string search = "L";
            decimal cost = 100;

            if (UseQuerySyntax)
            {
                Products = (from prod in Products where prod.Name.StartsWith(search) && prod.StandardCost > cost select prod).ToList();
            }
            else
            {
                Products = Products.Where(prod => prod.Name.StartsWith(search) && prod.StandardCost > cost).ToList();
            }
            ResultText = $"Total products: {Products.Count}";
        }

        // Filter products using a custom extension method
        public void WhereExtensionMethod()
        {
            string search = "Red";

            // The results of (from prod in Products select prod) is an IEnumerable<Product> which is why ByColor can be applied to it
            if (UseQuerySyntax)
            {
                Products = (from prod in Products select prod).ByColor(search).ToList();
            }
            else
            {
                Products = Products.Select(prod => prod).ByColor(search).ToList();
            }

            ResultText = $"Total products: {Products.Count}";
        }

        // Locate a specific product using First(). First() searches forward in a list until it finds the first element that matches criteria
        // First() throws an exception if the result does not produce any values
        public void First()
        {
            string search = "Red";
            Product value;

            try
            {
                if (UseQuerySyntax)
                {
                    // Query Syntax
                    value = (from prod in Products select prod).First(prod => prod.Color == search);
                }
                else
                {
                    // Method Syntax
                    value = Products.First(prod => prod.Color == search);
                }

                ResultText = $"Found: {value}";
            }
            catch
            {
                ResultText = "Not found";
            }

            Products.Clear();
        }

        // Locate a specific product using FirstOrDefault(). It searches forward in a list until it finds the first element that matches criteria
        // First() doesn't throw an exception if the result does not produce any values, instead it returns null
        public void FirstOrDefault()
        {
            string search = "Red";
            Product value;

            if (UseQuerySyntax)
            {
                value = (from prod in Products select prod).FirstOrDefault(prod => prod.Color == search);
            }
            else
            {
                //value = Products.Select(prod => prod).FirstOrDefault(prod => prod.Color == search);
                // same thing as
                value = Products.FirstOrDefault(prod => prod.Color == search);
            }

            if (value == null)
            {
                ResultText = "Not found";
            } else
            {
                ResultText = $"Found: {value}";
            }

            Products.Clear();
        }

        // Locate a specific product using Last(). Last() searches backwards in a list until it finds the first element that matches criteria
        // Last() throws an exception if the result does not produce any values
        public void Last()
        {
            string search = "Red";
            Product value;

            try
            {
                if (UseQuerySyntax)
                {
                    // Query Syntax
                    value = (from prod in Products select prod).Last(prod => prod.Color == search);
                }
                else
                {
                    // Method Syntax
                    value = Products.Last(prod => prod.Color == search);
                }

                ResultText = $"Found: {value}";
            }
            catch
            {
                ResultText = "Not found";
            }

            Products.Clear();
        }

        // LastOrDefault searches from the back of the list and returns the first element found or null
        public void LastOrDefault()
        {
            string search = "Red";
            Product value;

            if (UseQuerySyntax)
            {
                value = (from prod in Products select prod).LastOrDefault(prod => prod.Color == search);
            }
            else
            {
                //value = Products.Select(prod => prod).FirstOrDefault(prod => prod.Color == search);
                // same thing as
                value = Products.LastOrDefault(prod => prod.Color == search);
            }

            if (value == null)
            {
                ResultText = "Not found";
            }
            else
            {
                ResultText = $"Found: {value}";
            }

            Products.Clear();
        }

        // The Single method returns the only element that matches the criteria or an exception if no element or multiple elements match
        // We use Single() when we search for something like a primary key, so we expect only one to be in that collection since it's unique
        public void Single()
        {
            int search = 706;
            Product value;

            try
            {
                if (UseQuerySyntax)
                {
                    value = (from prod in Products select prod).Single(prod => prod.ProductID == search);
                }
                else
                {
                    value = Products.Single(prod => prod.ProductID == search);
                }

                ResultText = $"Found: {value}";
            }
            catch
            {
                ResultText = "Not found, or multiple elements found";
            }

            Products.Clear();
        }

        // The SingleOrDefault method returns the only element that matches the criteria, null if no elements matched, or an exception if multiple matched
        // The exception thrown if multiple elements are found is InvalidOperationException
        public void SingleOrDefault()
        {
            int search = 706;
            Product value;

            try
            {
                if (UseQuerySyntax)
                {
                    value = (from prod in Products select prod).SingleOrDefault(prod => prod.ProductID == search);
                }
                else
                {
                    //value = Products.Select(prod => prod).FirstOrDefault(prod => prod.Color == search);
                    // same thing as
                    value = Products.SingleOrDefault(prod => prod.ProductID == search);
                }

                if (value == null)
                {
                    ResultText = "Not found";
                }
                else
                {
                    ResultText = $"Found: {value}";
                }
            }
            catch
            {
                ResultText = "Multiple elements found";
            }

            Products.Clear();
        }

        // ForEach allows you to iterate over a collection to perform assignments within each object.
        // In this sample, assign the Length of the Name property to a property called NameLength
        // When using the Query syntax, assign the result to a temporary variable.
        public void ForEach()
        {
            if (UseQuerySyntax)
            {
                Products = (from prod in Products let temp = prod.NameLength = prod.Name.Length select prod).ToList();
            }
            else
            {
                Products.ForEach(prod => prod.NameLength = prod.Name.Length);
                // same thing as
                //Products.Select(prod => prod).ToList().ForEach(prod => prod.NameLength = prod.Name.Length);
            }
            ResultText = $"Total products: {Products.Count}";
        }

        // In the SalesForSpecificProduct method we pass in a product and look for all SALES that match that PRODUCT's id
        // then we sum the LineTotal property of all the sales that match
        public void ForEachCallingMethod()
        {
            if (UseQuerySyntax)
            {
                Products = (from prod in Products let tmp = prod.TotalSales = SalesForSpecificProduct(prod) select prod).ToList();
            }
            else
            {
                Products.ForEach(prod => prod.TotalSales = SalesForSpecificProduct(prod));
            }
            ResultText = $"Total products: {Products.Count}";
        }
        public decimal SalesForSpecificProduct(Product prod)
        {
            return Sales.Where(sale => sale.ProductID == prod.ProductID).Sum(sale => sale.LineTotal);
        }

        // Use Take() to select a specified number of items from the beginning of a collection
        public void Take()
        {
            if (UseQuerySyntax)
            {
                Products = (from prod in Products orderby prod.Name select prod).Take(5).ToList();
            }
            else
            {
                Products = Products.OrderBy(prod => prod.Name).Take(5).ToList();
            }
            ResultText = $"Total products: {Products.Count}";
        }

        // TakeWhile() - select elements until a condition is no longer true
        public void TakeWhile()
        {
            if (UseQuerySyntax)
            {
                Products = (from prod in Products orderby prod.Name select prod).TakeWhile(prod => prod.Name.StartsWith("A")).ToList();
            }
            else
            {
                Products = Products.OrderBy(prod => prod.Name).TakeWhile(prod => prod.Name.StartsWith("A")).ToList();
            }
            ResultText = $"Total products: {Products.Count}";
        }

        // Skip() - skip the first x items in the collection
        public void Skip()
        {
            if (UseQuerySyntax)
            {
                Products = (from prod in Products orderby prod.Name select prod).Skip(20).ToList();
            }
            else
            {
                Products = Products.OrderBy(prod => prod.Name).Skip(20).ToList();
            }

            ResultText = $"Total products: {Products.Count}";
        }

        // SkipWhile() - skip while the condition is true, and then return everything after that follows
        // - documentation definition - Bypasses elements in a sequence as long as a specified condition is true and then returns the remaining elements.
        public void SkipWhile()
        {
            if (UseQuerySyntax)
            {
                Products = (from prod in Products orderby prod.Name select prod).SkipWhile(prod => prod.Name.StartsWith("A")).ToList();
            }
            else
            {
                Products = Products.OrderBy(prod => prod.Name).SkipWhile(prod => prod.Name.StartsWith("A")).ToList();
            }

            ResultText = $"Total products: {Products.Count}";
        }

        // Distinct() selects distinct elements only
        public void Distinct()
        {
            List<string> colors;

            if (UseQuerySyntax)
            {
                colors = (from prod in Products select prod.Color).Distinct().ToList();
            }
            else
            {
                colors = Products.Select(prod => prod.Color).Distinct().ToList();
            }

            foreach(string color in colors)
            {
                Console.WriteLine($"Color: {color}");
            }

            Console.WriteLine($"Total colors {colors.Count}");

            Products.Clear();
        }

        // All() searches all items in collection and returns true if all items match condition else false
        // Use All() to see if all items in a collection meet a specified condition
        public void All()
        {
            string search = " ";
            bool value;

            if (UseQuerySyntax)
            {
                value = (from prod in Products select prod).All(prod => prod.Name.Contains(search));
            }
            else
            {
                value = Products.All(prod => prod.Name.Contains(search));
            }

            ResultText = $"Do all Name properties contain a '{search}'? {value}";

            Products.Clear();
        }

        // Any() searches all items in collection and returns true if any items match condition else false
        // Use Any() to see if at least one item in a collection meets a specified condition
        public void Any()
        {
            string search = "z";
            bool value;

            if (UseQuerySyntax)
            {
                value = (from prod in Products select prod).Any(prod => prod.Name.Contains(search));
            }
            else
            {
                value = Products.Any(prod => prod.Name.Contains(search));
            }

            ResultText = $"Does any Name property contain a '{search}'? {value}";

            Products.Clear();
        }


        // Use the LINQ Contains operator to see if a collection contains a specific value
        public void LINQContains()
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
            bool value;

            if (UseQuerySyntax)
            {
                value = (from val in numbers select val).Contains(3);
            }
            else
            {
                value = numbers.Contains(3);
            }

            ResultText = $"Is the number 3 in the collection? {value}";

            Products.Clear();
        }

        //  Use the LINQ Contains operator to see if a collection contains a specific object using an EqualityComparer class to perform the comparison
        public void LINQContainsUsingComparer()
        {
            int search = 744;
            bool value;
            ProductIdComparer comparer = new ProductIdComparer();
            Product prodtoFind = new Product { ProductID = search };

            if (UseQuerySyntax)
            {
                value = (from prod in Products select prod).Contains(prodtoFind, comparer);
            }
            else
            {
                value = Products.Contains(prodtoFind, comparer);
            }
            
            ResultText = $"Product ID: {search} is in Products Collection = {value}";

            Products.Clear();
        }

        // SequenceEqual() returns true if all the items match the items in the other collection, otherwise false
        // SequenceEqual() can compare primitives and objects but it needs to implement EqualityComparer for objects
        // otherwise it compares their references (addresses)
        // SequenceEqual() compares two different collections to see if they are equal
        // When using simple data types such as int, string, a direct comparison between values is performed
        public void SequenceEqualIntegers()
        {
            bool value;
            List<int> list1 = new List<int> { 1, 2, 3, 4, 5 };
            List<int> list2 = new List<int> { 1, 2, 3, 4, 5 };

            if (UseQuerySyntax)
            {
                value = (from num in list1 select num).SequenceEqual(list2);
            }
            else
            {
                value = list1.SequenceEqual(list2);
            }

            if (value)
            {
                ResultText = "Lists are equal";
            }
            else
            {
                ResultText = "Lists are not equal";
            }

            Products.Clear();
        }

        // Use SequenceEqual to compare Object types (Product types - products)
        // returns false because even though properties are the same, and the values, list1 and list2 are references to different objects on the heap
        public void SequenceEqualObjects()
        {
            bool value;

            List<Product> list1 = new List<Product> 
            { 
                new Product { ProductID = 1, Name = "Product 1"},
                new Product { ProductID = 2, Name = "Product 2" } 
            };

            List<Product> list2 = new()
            {
                new Product { ProductID = 1, Name = "Product 1" },
                new Product { ProductID = 2, Name = "Product 2" }
            };

            if (UseQuerySyntax)
            {
                value = (from item in list1 select item).SequenceEqual(list2);
            }
            else
            {
                value = list1.SequenceEqual(list2);
            }

            if (value)
            {
                ResultText = "Lists are equal";
            }
            else
            {
                ResultText = "Lists are not equal";
            }

            Products.Clear();
        }

        // Use an EqualityComparer class to determine if the objects are the same based on the values in properties.
        public void SequenceEqualUsingComparer()
        {
            bool value;
            ProductComparer pc = new ProductComparer(); 
            List<Product> list1 = ProductRepository.GetAll();
            List<Product> list2 = ProductRepository.GetAll();

            // If we remove an item, or we change the value of a property of any of the Product objects, they are not equal anymore
            //list1.RemoveAt(0);

            if (UseQuerySyntax)
            {
                // Query Syntax
                value = (from prod in list1 select prod).SequenceEqual(list2, pc);
            }
            else
            {
                // Method Syntax
                value = list1.SequenceEqual(list2, pc);
            }

            if (value)
            {
                ResultText = "Lists are Equal";
            }
            else
            {
                ResultText = "Lists are NOT Equal";
            }

            // Clear List
            Products.Clear();
        }
    }
}
 