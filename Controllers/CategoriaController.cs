﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculaAPI.Data;
using PeliculaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CategoriaController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Categoria>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCategorias()
        {
            var lista = await _db.Categorias.OrderBy(c => c.Nombre).ToListAsync();

            return Ok(lista);
        }

        [HttpGet("{id:int}", Name = "GetCategoria")]
        [ProducesResponseType(200, Type = typeof(Categoria))] // GET - Correct
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> GetCategoria(int id)
        {
            var obj = await _db.Categorias.FirstOrDefaultAsync(c => c.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            return Ok(obj);
        }

        [HttpPost]
        [ProducesResponseType(201)] // POST - Correct
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(500)] // Internal Error
        public async Task<IActionResult> CrearCategoria([FromBody] Categoria categoria)
        {

            if (categoria == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _db.AddAsync(categoria);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetCategoria", new { id = categoria.Id }, categoria);

        }
    }
}
