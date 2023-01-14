using CodeReview.Business.Core.Notificacoes;
using CodeReview.Business.Models.Fornecedores;
using CodeReview.Business.Models.Fornecedores.Services;
using CodeReview.Business.Models.Produtos;
using CodeReview.Business.Models.Produtos.Service;
using CodeReview.Infra.Data.Context;
using CodeReview.Infra.Data.Repository;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace CodeReview.AppMvc.App_Start
{
    public class DependencyInjectionConfig
    {
        public static void RegisterDIContainer()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainer(Container container)
        {
            container.Register<MeuDbContext>(Lifestyle.Scoped);

            container.Register<IProdutoRepository,ProdutoRepository>(Lifestyle.Scoped);

            container.Register<IProdutoService, ProdutoService>(Lifestyle.Scoped);

            container.Register<IFornecedorRepository, FornecedorRepository>(Lifestyle.Scoped);

            container.Register<IFornecedorService, FornecedorService>(Lifestyle.Scoped);

            container.Register<IEnderecoRepository, EnderecoRepository>(Lifestyle.Scoped);

            container.Register<INotificador, Notificador>(Lifestyle.Scoped);

            container.RegisterSingleton(() => AutoMapperConfig.GetMapperConfiguration().CreateMapper(container.GetInstance));

        }
    }
}