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
using CodeReview.Business.Models.Fornecedores;
using CodeReview.Business.Models.Fornecedores.Services;
using CodeReview.Infra.Data.Repository;
using CodeReview.Business.Core.Notificacoes;
using AutoMapper;
using CodeReview.AppMvc.Extensions;

namespace CodeReview.AppMvc.Controllers
{
    [Authorize]
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService     _fornecedorService;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository fornecedorRepository, IFornecedorService fornecedorService, IMapper mapper, INotificador notificador):base(notificador)
        {
            _fornecedorRepository= fornecedorRepository;
            _fornecedorService= fornecedorService;
            _mapper=mapper;
        }

        [AllowAnonymous]
        [Route("Lista-Fornecedor")]
        public async Task<ActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos()));
        }

        [AllowAnonymous]
        [Route("Detalhes-Fornecedor")]
        public async Task<ActionResult> Details(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if (fornecedor == null) return HttpNotFound();

            return View(fornecedor);
        }

        [Route("novo-fornecedor")]
        [ClaimsAuthorize("Fornecedor","Adicionar")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Route("novo-fornecedor")]
        [ClaimsAuthorize("Fornecedor", "Adicionar")]
        [HttpPost]
        public async Task<ActionResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
           await _fornecedorService.Adicionar(fornecedor);

            // TODO:
            // E se não der certo
            if (!OperacaoValida()) return View(fornecedorViewModel);

            ViewBag.Sucesso = "Fornecedor Cadastrado com Sucess";

            return RedirectToAction("Index");
        }


        [Route("editar-fornecedor/{id:guid}")]
        [ClaimsAuthorize("Fornecedor", "Editar")]
        [HttpGet]
        public async Task <ActionResult> Edit(Guid id)
        {
            var FornecedorViewModel = await ObterFornecedorProdutoEndereco(id);

            if (FornecedorViewModel==null)
            {
                return HttpNotFound();
            }
            return View(FornecedorViewModel);
        }

        [Route("editar-fornecedor/{id:guid}")]
        [HttpPost]
        [ClaimsAuthorize("Fornecedor", "Editar")]
        public async Task<ActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            
            if (id != fornecedorViewModel.Id)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid) return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorService.Atualizar(fornecedor);

            // TODO:
            // E se não der certo

            return RedirectToAction("Index");
        }

        [Authorize(Roles ="Admin")]
        [Route("excluir-fornecedor/{id:guid}")]
        [ClaimsAuthorize("Fornecedor", "Excluir")]
        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            var FornecedorViewModel=await ObterFornecedorEndereco(id);

            if (FornecedorViewModel==null)
            {
                return HttpNotFound();
            }
            return View(FornecedorViewModel);
        }

        [Authorize(Roles = "Admin")]
        [Route("excluir-fornecedor/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedor= await ObterFornecedorEndereco(id);
            if (fornecedor == null)
            {
                return HttpNotFound();
            }

           await _fornecedorService.Remover(id);

            // TODO:
            // E se não der certo

            return View("Index");

        }
        [Route("obter-endereco-fornecedor/{id:guid}")]
        public async Task<ActionResult> ObterEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);
            if (fornecedor == null) return HttpNotFound();

            return PartialView("_DetalhesEndereco", fornecedor);
        }

        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        [HttpGet]
        public async Task<ActionResult> AtualizarEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if (fornecedor == null) return HttpNotFound();

            return PartialView("_AtualizarEndereco", new FornecedorViewModel { Endereco = fornecedor.Endereco });
        }

        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        [HttpPost]
        public async Task<ActionResult> AtualizarEndereco(FornecedorViewModel fornecedorViewModel)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("Documento");
            if(!ModelState.IsValid) return PartialView("_AtualizarEndereco",fornecedorViewModel);

            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(fornecedorViewModel.Endereco));

            // TODO:
            // E se não der certo

            var url = Url.Action("ObterEndereco", "Fornecedores", new { id = fornecedorViewModel.Endereco.FornecedorID });
            return Json(new { success = true, url });
        }



        public async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
           return  _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }

        public async Task<FornecedorViewModel> ObterFornecedorProdutoEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
        }
    }
}