using Dapper;
using DapperApp.Web.Context;
using DapperApp.Web.DTOs.Categories;

namespace DapperApp.Web.Services.Categories
{
    public class CategoryService : ICategoryService
    {

        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateCategoryDto categoryDto)
        {
            string query = "insert into categories (Name) values (@Name)";
            var parameters = new DynamicParameters();
            parameters.Add("@Name", categoryDto.Name);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteAsync(int id)
        {
            string query = "delete from categories where CategoryId= @CategoryId";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<IEnumerable<ResultCategoryDto>> GetAllAsync()
        {
            string query = "select * from categories";

            var connection = _context.CreateConnection();

            return await connection.QueryAsync<ResultCategoryDto>(query);
            
        }

        public async Task<ResultCategoryDto> GetByIdAsync(int id)
        {
            string query = "select * from categories where CategoryId=@CategoryId";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", id);

            var connection = _context.CreateConnection();

    return await connection.QueryFirstOrDefaultAsync<ResultCategoryDto>(query, parameters);
        }

        public async Task UpdateAsync(UpdateCategoryDto categoryDto)
        {
            string query = "update categories set Name=@Name where CategoryId=@CategoryId";
            var parameters = new DynamicParameters();
            parameters.Add("@Name", categoryDto.Name);
            parameters.Add("@CategoryId", categoryDto.CategoryId);

            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }
    }
}
