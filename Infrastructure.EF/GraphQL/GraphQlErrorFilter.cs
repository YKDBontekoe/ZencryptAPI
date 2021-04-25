using HotChocolate;

namespace Infrastructure.EF.GraphQL
{
    public class GraphQlErrorFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            return error.WithMessage(error.Exception != null ? error.Exception.Message : "Something went wrong");
        }
    }
}