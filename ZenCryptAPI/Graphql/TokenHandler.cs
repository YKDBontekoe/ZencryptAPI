using System;
using System.Linq;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace ZenCryptAPI.Graphql
{
    public class TokenHandler
    {
        public static string GetToken(IHttpContextAccessor contextAccessor)
        {
            try
            {
                return contextAccessor.HttpContext.Request.Headers.SingleOrDefault(c => c.Key.Equals("Authorization"))
                    .Value.ToString().Split(" ")[1];
            }
            catch (Exception e)
            {
                throw new InvalidTokenException();
            }
        }
    }
}