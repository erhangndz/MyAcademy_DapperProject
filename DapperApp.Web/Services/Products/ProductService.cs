using Dapper;
using DapperApp.Web.Context;
using DapperApp.Web.DTOs.Categories;
using DapperApp.Web.DTOs.Products;
using System.Data;

namespace DapperApp.Web.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IDbConnection _db;

        public ProductService(AppDbContext context)
        {
            _context = context;
            _db = context.CreateConnection();
        }

        public async Task ChangeStatusAsync(int id)
        {

            var product = await GetByIdAsync(id);

            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", id);
            if (product.IsActive)
            {
                parameters.Add("@IsActive", false);
            }
            else
            {
                parameters.Add("@IsActive", true);
            }
                string query = "update products set IsActive=@IsActive where ProductId=@ProductId";

            await _db.ExecuteAsync(query, parameters);

            
        }

        public async Task CreateAsync(CreateProductDto productDto)
        {
            string query = "insert into products (Name,Price,Stock,ImageUrl,IsActive,CategoryId) values(@Name,@Price,@Stock,@ImageUrl,@IsActive,@CategoryId) ";
            var parameters = new DynamicParameters(productDto);
            await _db.ExecuteAsync(query, parameters);
        }

        public async Task DeleteAsync(int id)
        {
            string query = "delete from products where ProductId=@ProductId";
            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", id);
            await _db.ExecuteAsync(query, parameters);
        }

        public async Task<IEnumerable<ResultProductDto>> GetAllAsync()
        {
            string query = "select * from products p inner join categories c on p.CategoryId=c.CategoryId  ";

            return await _db.QueryAsync<ResultProductDto, ResultCategoryDto, ResultProductDto>(query, (product, category) =>
            {
                product.Category = category;
                return product;
            },
  splitOn: "CategoryId");
        }

        public async Task<UpdateProductDto> GetByIdAsync(int id)
        {
            string query = "select * from products where ProductId=@ProductId";
            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", id);
            return await _db.QueryFirstOrDefaultAsync<UpdateProductDto>(query,parameters);
        }

        public async Task UpdateAsync(UpdateProductDto productDto)
        {
            string query = "update products set Name=@Name,ImageUrl=@ImageUrl,Price=@Price,Stock=@Stock, IsActive=@IsActive,CategoryId=@CategoryId where ProductId=@ProductId ";
            var parameters = new DynamicParameters(productDto);
            await _db.ExecuteAsync(query, parameters);
        }
    }
}
