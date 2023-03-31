using GraphQL;
using GraphQL.Types;
using GraphQLProject.Interfaces;
using GraphQLProject.Type;

namespace GraphQLProject.Query
{
    public class ProductQuery : ObjectGraphType
    {
        public ProductQuery(IProduct productService)
        {
            //LISTA PRODUTOS
            Field<ListGraphType<ProductType>>("products", resolve: context=> 
            { 
                return productService.GetAllProducts(); 
            });

            // PRODUTO BY ID
            Field<ProductType>
                (
                    "product", 
                    arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name= "id" }),
                    resolve: context => 
                    { 
                        // RECEBE ARGUMENTO
                        return productService.GetProductById(context.GetArgument<int>("id")); 
                    }
                );
        }
    }
}
