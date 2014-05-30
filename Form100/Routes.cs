using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;
using Orchard.WebApi.Routes;

namespace CSM.Form100
{
    public class Routes : IRouteProvider
    {
        private static readonly string Area = "CSM.Form100";

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                new RouteDescriptor() {
                    Route = new Route(
                        url: "Form100/Reviews/{id}",
                        defaults: new RouteValueDictionary() {
                            { "area", Area },
                            { "controller", "Reviews" },
                            { "action", "Index" },
                            { "id", UrlParameter.Optional }
                        },
                        constraints: new RouteValueDictionary(),
                        dataTokens: new RouteValueDictionary() {
                            { "area", Area }
                        },
                        routeHandler: new MvcRouteHandler()
                    )
                },
                new RouteDescriptor() {
                    Route = new Route(
                        url: "Form100/Reviews/{id}/{status}",
                        defaults: new RouteValueDictionary() {
                            { "area", Area },
                            { "controller", "Reviews" },
                            { "action", "Update" },
                        },
                        constraints: new RouteValueDictionary() {
                            { "id", @"\d+" },
                            { "status", String.Format("({0})", String.Join("|", Enum.GetNames(typeof(WorkflowStatus)))) }
                        },
                        dataTokens: new RouteValueDictionary() {
                            { "area", Area }
                        },
                        routeHandler: new MvcRouteHandler()
                    )
                },
                new RouteDescriptor() {
                    Route = new Route(
                        url: "Form100/Reviews/{action}/{id}",
                        defaults: new RouteValueDictionary() {
                            { "area", Area },
                            { "controller", "Reviews" },
                        },
                        constraints: new RouteValueDictionary() {
                            { "action", @"[A-Za-z]+" },
                            { "id", @"\d+" }
                        },
                        dataTokens: new RouteValueDictionary() {
                            { "area", Area }
                        },
                        routeHandler: new MvcRouteHandler()
                    )
                }
            };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
            {
                routes.Add(routeDescriptor);
            }
        }
    }
}