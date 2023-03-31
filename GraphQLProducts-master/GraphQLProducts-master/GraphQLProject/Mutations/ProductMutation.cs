using GraphQL;
using GraphQL.Types;
using GraphQLProject.Interfaces;
using GraphQLProject.Models;
using GraphQLProject.Type;

namespace GraphQLProject.Mutations
{
    public class ProductMutation : ObjectGraphType
    {
        public ProductMutation(IProduct productService)
        {


            // ADD PRODUTO
            //==========================================================================================
            Field<ProductType>
                (
                    // createProduct
                    // Copiar de Query mas cuidado com o type no argumento (ProductInputType)
                    // Observar que o Produto passa aser um Type (Name = "product")
                    "createProduct",
                    arguments: new QueryArguments(new QueryArgument<ProductInputType> { Name = "product" }),
                    resolve: context =>
                    {
                        // RECEBE ARGUMENTO 
                        // Ovservar o Método AddProduct com Argument do tipo <Product> e o name "product"
                        return productService.AddProduct(context.GetArgument<Product>("product"));
                    }
                );




            // UPDATE PRODUTO BY ID
            //==========================================================================================
            Field<ProductType>
                (
                    // updateProduct  (COMBINA O ID COM O OBJETO)
                    // Copiar de Query mas cuidado com o type no argumento (ProductInputType)
                    // Observar que o Produto passa aser um Type (Name = "product")
                    "updateProduct",
                    arguments: new QueryArguments
                    (
                        new QueryArgument<IntGraphType> { Name = "id" },
                        new QueryArgument<ProductInputType> { Name = "product" }
                    ),
                    resolve: context =>
                    {
                        // RECEBE ARGUMENTO 
                        // Observar o Método AddProduct com Argument do tipo <Product> e o name "product"
                        var productId = context.GetArgument<int>("id");
                        var productObj = context.GetArgument<Product>("product");  // LINHA 25
                        return productService.UpdateProduct( productId, productObj );
                    }
                );



            // DELETE PRODUTO BY ID (COPIAR  DO UPDATE) - AO FINAL ADICIONAR EM Product Schema
            //==========================================================================================
            // GRAPH QL NAO TEM VOID - USAR StringGraphType
            Field<StringGraphType>("deleteProduct",
                    arguments: new QueryArguments
                    (
                        new QueryArgument<IntGraphType> { Name = "id" }
                    ),
                    resolve: context =>
                    {
                        // RECEBE ARGUMENTO 
                        // Observar o Método AddProduct com Argument do tipo <Product> e o name "product"
                        var productId = context.GetArgument<int>("id");
                        productService.DeleteProduct(productId);
                        return "Produto " + productId + " excluido";
                    }
                );
        }
    }
}
