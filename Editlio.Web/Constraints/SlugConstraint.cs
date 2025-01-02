namespace Editlio.Web.Constraints
{
    public class SlugConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values[routeKey] is string slug)
            {
                
                return !slug.Contains('/');
            }
            return false;
        }
    }
}
