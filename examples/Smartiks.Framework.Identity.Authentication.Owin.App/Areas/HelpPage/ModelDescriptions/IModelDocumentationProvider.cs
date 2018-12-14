using System;
using System.Reflection;

namespace Smartiks.Framework.Identity.Authentication.Owin.App.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}