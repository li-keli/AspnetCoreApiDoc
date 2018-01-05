using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace AspnetCoreApiDoc.Proto.Doc
{
    public class ApiDocApplicationConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            application.Controllers.ToList().ForEach(t =>
            {
                t.Actions.ToList().ForEach(ac =>
                {
                    foreach (Attribute acAttribute in ac.Attributes)
                    {
                        if (!(acAttribute is ApiDocAttribute)) continue;
                        var attr = acAttribute as ApiDocAttribute;
                        t.ApiExplorer.GroupName = string.IsNullOrWhiteSpace(attr.GroupName) ? t.ControllerName : attr.GroupName;
                        ac.ApiExplorer.IsVisible = attr.IsCreateDoc;
                    }
                });
                foreach (Attribute objAttribute in t.Attributes)
                {
                    if (!(objAttribute is ApiDocAttribute)) continue;
                    var attr = objAttribute as ApiDocAttribute;
                    t.ApiExplorer.GroupName = string.IsNullOrWhiteSpace(attr.GroupName) ? t.ControllerName : attr.GroupName;
                    
                    t.ApiExplorer.IsVisible = attr.IsCreateDoc;
                }
            });
        }
    }
}