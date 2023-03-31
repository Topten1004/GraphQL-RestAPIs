using GraphQLProject.Mutations;
using GraphQLProject.Query;

namespace GraphQLProject.Schema  // POSSUI REFERENCIAS PARA QUERIES E MUTATIONS
{
    public class ProductSchema : GraphQL.Types.Schema
    {
        public ProductSchema(ProductQuery productQuery, ProductMutation productMutation)
        {
            Query = productQuery;
            Mutation = productMutation; // AJUSTAR NA Startup - ConfigureServices
        }
    }
}
