﻿using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;
using System.Collections.Generic;

namespace SistemasdeTarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartaoController : ControllerBase
    {
        private readonly ICartaoRepository _cartaoRepository;

        public CartaoController(ICartaoRepository cartaoRepository)
        {
            _cartaoRepository = cartaoRepository;
        }

        [HttpGet("encarregados")]
        public ActionResult<IEnumerable<Cartao>> GetEncarregados(string codigo)
        {
            var encarregados = _cartaoRepository.GetEncarregados(codigo);
            return Ok(encarregados);
        }

        [HttpGet("estudantes")]
        public ActionResult<IEnumerable<Cartao>> GetEstudantes(string codigo)
        {
            var estudantes = _cartaoRepository.GetEstudantes(codigo);
            return Ok(estudantes);
        }
    }
}