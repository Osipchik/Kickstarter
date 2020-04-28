using System.Collections.Generic;
using System.Linq;
using Category.API.Models;

namespace Category.API.Data
{
    public class DbInit
    {
        public static void Initialize(ApplicationContext context)
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            if (context.CategoryContext.Any())
            {
                return;
            }

            CreateCategories(context);
            CreateSubCategories(context);
        }
        
        private static void CreateCategories(ApplicationContext context)
        {
            var category = new List<Models.Category>()
            {
                new Models.Category {CategoryName = "Art"},
                new Models.Category {CategoryName = "Comics & illustration"},
                new Models.Category {CategoryName = "Design & Tech"},
                new Models.Category {CategoryName = "Film"},
                new Models.Category {CategoryName = "Food & Craft"},
                new Models.Category {CategoryName = "Games"},
                new Models.Category {CategoryName = "Music"},
                new Models.Category {CategoryName = "Publishing"},
            };
            
            context.CategoryContext.AddRange(category);
            context.SaveChanges();
        }

        private static void CreateSubCategories(ApplicationContext context)
        {
            var sub = new List<SubCategory>();

            var asd = context.CategoryContext.First(i => i.CategoryName == "Art").Id;
            
            // Art +
            sub.Add(new SubCategory
            {
                SubCategoryName = "Ceramics",
                CategoryId = context.CategoryContext.First(i => i.CategoryName == "Art").Id
            });
            sub.Add(new SubCategory
            {
                SubCategoryName = "Conceptual art",
                CategoryId = context.CategoryContext.First(i => i.CategoryName == "Art").Id
            });
            sub.Add(new SubCategory
            {
                SubCategoryName = "Digital art",
                CategoryId = context.CategoryContext.First(i => i.CategoryName == "Art").Id
            });

            //Comics +
            sub.Add(new SubCategory
            {
                SubCategoryName = "Anthologies",
                CategoryId = context.CategoryContext.First(i => i.CategoryName == "Comics & illustration").Id,
            });
            sub.Add(new SubCategory
            {
                SubCategoryName = "Comic Books",
                CategoryId = context.CategoryContext.First(i => i.CategoryName == "Comics & illustration").Id,
            });
            sub.Add(new SubCategory
            {
                SubCategoryName = "Webcomics",
                CategoryId = context.CategoryContext.First(i => i.CategoryName == "Comics & illustration").Id,
            });
            
            // Film
            sub.Add(new SubCategory
            {
                SubCategoryName = "Action",
                CategoryId = context.CategoryContext.First(i => i.CategoryName == "Film").Id,
            });
            sub.Add(new SubCategory
            {
                SubCategoryName = "Animation",
                CategoryId = context.CategoryContext.First(i => i.CategoryName == "Film").Id,
            });
            sub.Add(new SubCategory
            {
                SubCategoryName = "Comedy",
                CategoryId = context.CategoryContext.First(i => i.CategoryName == "Film").Id,
            });
            
            // Games +
            sub.Add(new SubCategory
            {
                SubCategoryName = "Gaming Hardware",
                CategoryId = context.CategoryContext.First(i => i.CategoryName == "Games").Id,
            });
            sub.Add(new SubCategory
            {
                SubCategoryName = "Live Games",
                CategoryId = context.CategoryContext.First(i => i.CategoryName == "Games").Id,
            });
            sub.Add(new SubCategory
            {
                SubCategoryName = "Mobile Games",
                CategoryId = context.CategoryContext.First(i => i.CategoryName == "Games").Id,
            });
            
            // Music
            sub.Add(new SubCategory
            {
                SubCategoryName = "Blues",
                CategoryId = context.CategoryContext.First(i => i.CategoryName == "Music").Id,
            });
            sub.Add(new SubCategory
            {
                SubCategoryName = "Chiptune",
                CategoryId = context.CategoryContext.First(i => i.CategoryName == "Music").Id,
            });
            sub.Add(new SubCategory
            {
                SubCategoryName = "Classical Music",
                CategoryId = context.CategoryContext.First(i => i.CategoryName == "Music").Id,
            });

            
            context.SubCategoryContext.AddRange(sub);
            context.SaveChanges();
        }
    }
}