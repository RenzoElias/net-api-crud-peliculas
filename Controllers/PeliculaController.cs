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
    public class PeliculaController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public PeliculaController (ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(200, Type= typeof(List<Pelicula>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPeliculas()
        {
            var lista = await _db.Peliculas.OrderBy(p => p.NombrePelicula).Include(p => p.Categoria).ToListAsync();

            return Ok(lista);
        }

        [HttpGet("{id:int}", Name= "GetPelicula")]
        [ProducesResponseType(200, Type = typeof(Pelicula))] // GET - Correct
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> GetPelicula(int id)
        {
            var obj = await _db.Peliculas.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);

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
        public async Task<IActionResult> CrearPelicula([FromBody] Pelicula pelicula)
        {
            if(pelicula == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _db.AddAsync(pelicula);
            await _db.SaveChangesAsync();

            return Ok();

        }
    }
}
