using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodeReview.AppMvc.Models;
using CodeReview.AppMvc.ViewModels;
using CodeReview.Business.Models.Produtos;
using CodeReview.Business.Models.Produtos.Service;
using CodeReview.Infra.Data.Repository;
using CodeReview.Business.Core.Notificacoes;
using AutoMapper;
using CodeReview.Business.Models.Fornecedores;
using System.IO;
using System.Web.Routing;

namespace CodeReview.AppMvc.Controllers
{
    public class ProdutoController : BaseController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;

        public ProdutoController(IProdutoRepository produtoRepository, IProdutoService produtoService, IMapper mapper, IFornecedorRepository fornecedorRepository,INotificador notificador) : base(notificador)
        {
            _produtoRepository = produtoRepository;
            _produtoService= produtoService;
            _mapper = mapper;
            _fornecedorRepository = fornecedorRepository;
        }

        // GET: Produto
        [HttpGet]
        [Route("lista-de-produtos")]
        public async Task<ActionResult> Index()
        {
            var produtosVM = _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosPorFornecedores());  


            return View(produtosVM);
        }

        // GET: Produto/Details/5
        [Route("dados-do-produto/{id:guid}")]
        [HttpGet]
        public async Task<ActionResult> Details(Guid id)
        {
            var produtoviewmodel = await ObterProduto(id);

            if (produtoviewmodel == null)
            {
                return HttpNotFound();
            }
            return View(produtoviewmodel);
        }

        // GET: Produto/Create
        [Route("novo-produto")]
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var produtoViewModel = await PopularFornecedores(new ProdutoViewModel());
            
            return View(produtoViewModel);
        }

        [Route("novo-produto")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await PopularFornecedores(produtoViewModel);

            if (!ModelState.IsValid) return View(produtoViewModel);

              var imgPrefixo = Guid.NewGuid() + "_";

            if(!UploadImagem(produtoViewModel.ImageUpload, imgPrefixo))
            {
                return View(produtoViewModel);
            }

            produtoViewModel.Image = imgPrefixo + produtoViewModel.ImageUpload.FileName;

                var produto=_mapper.Map<Produto>(produtoViewModel);

                await _produtoService.Adicionar(produto);

            if (!OperacaoValida()) return View(produtoViewModel);

            ViewBag.Sucesso = "Produto Cadastrado com Sucess";

            return RedirectToAction("Index");
            
         
            
        }

        // GET: Produto/Edit/5
        [Route("editar-produto/{id:guid}")]
        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
            {
                return HttpNotFound();
            }
           
            return View(produtoViewModel);
        }

        [Route("editar-produto/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return View(produtoViewModel);

            var ProdutoAtualizacao = await ObterProduto(produtoViewModel.id);
            produtoViewModel.Image = ProdutoAtualizacao.Image;

            if (produtoViewModel.ImageUpload!=null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";

                if (!UploadImagem(produtoViewModel.ImageUpload, imgPrefixo))
                {
                    return View(produtoViewModel);
                }

                ProdutoAtualizacao.Image= imgPrefixo + produtoViewModel.ImageUpload.FileName;
            }

            ProdutoAtualizacao.Nome=produtoViewModel.Nome;
            ProdutoAtualizacao.Descricao=produtoViewModel.Descricao;
            ProdutoAtualizacao.Valor=produtoViewModel.Valor;
            ProdutoAtualizacao.Ativo=produtoViewModel.Ativo;
            ProdutoAtualizacao.Fornecedor=produtoViewModel.Fornecedor;
            ProdutoAtualizacao.FornecedorID=produtoViewModel.FornecedorID;


            await _produtoService.Atualizar(_mapper.Map<Produto>(ProdutoAtualizacao));

            return RedirectToAction("Index");             
           
        }

        // GET: Produto/Delete/5
        [HttpGet]
        [Route("excluir-produto/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {

            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
            {
                return HttpNotFound();
            }
            return View(produtoViewModel);
        }

        // POST: Produto/Delete/5
        [Route("excluir-produto/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);


            if (produtoViewModel == null)
            {
                return HttpNotFound();
            }

            await _produtoService.Remover(id);

            return RedirectToAction("Index");
        }


        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            var ProdutoVM=_mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));
            ProdutoVM.Fornecedores= _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            
            return ProdutoVM;
        }

        public async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produto)
        {
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            return produto;
        }

        private bool UploadImagem(HttpPostedFileBase imag,string imgPrefixo)
        {
            if (imag==null || imag.ContentLength<=0)
            {
                ModelState.AddModelError(string.Empty, "Imagem em formato inválido");
                return false;
            }

            var path = Path.Combine(HttpContext.Server.MapPath("~/Imagens"), imgPrefixo + imag.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return false;
            }

            imag.SaveAs(path);

            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _produtoRepository.Dispose();
                _produtoService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
