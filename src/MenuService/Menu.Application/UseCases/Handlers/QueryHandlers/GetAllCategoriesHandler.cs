using MediatR;
using Microsoft.EntityFrameworkCore;
using Menu.Application.UseCases.Queries;
using Menu.Domain;
using Menu.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Application.UseCases.Handlers.QueryHandlers
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, List<Category>>
    {

        private readonly MenuDbContext dbContext;

        public GetAllCategoriesHandler(MenuDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Category>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await dbContext.Categories.ToListAsync(cancellationToken);
                return result;
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
