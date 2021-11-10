using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OBD2.Context;
using OBD2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OBD2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public ValuesController(AppDbContext appDbContext) { _appDbContext = appDbContext; }

        //Rota de Get para todos os Codigos
        [HttpGet]
        public async Task<IActionResult> GetCodes()
        {
            //retorna objeto Json
            return Ok
                (new
                {
                    Sucesso = true,
                    Data = System.DateTime.Now,
                    Dados = await _appDbContext.Codes.ToListAsync()
                });
        }

        //Rota para Get busca por ID
        [HttpGet("{Id_Code}")]
        public async Task<ActionResult<CodeOBD2>> GetCode(int Id_Code)
        {
            var odbcode = await _appDbContext.Codes.FindAsync(Id_Code);
            if (odbcode == null)
            { return NotFound(); }
            return Ok(new
                {
                    Sucesso = true,
                    Data = System.DateTime.Now,
                    Dados = odbcode
                });
        }

        //Rota para post cadastro de novos codigos
        [HttpPost]
        public async Task<IActionResult> InsertNewCode(CodeOBD2 code)
        {
            _appDbContext.Codes.Add(code);
            await _appDbContext.SaveChangesAsync();
            //retorna objeto Json
            return Ok
                (new
                {
                    Sucesso = true,
                    Mensagem = "Cadastrado com Sucesso!",
                    Data = System.DateTime.Now,
                    Dados = code
                });
        }

        //Rota Put para alterar os dados/atualizar
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CodeOBD2 code)
        {
            if (id != code.Id)
            {
                return BadRequest();
            }
            _appDbContext.Entry(code).State = EntityState.Modified;
            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!odbexist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        //função privada de verificar existencia
        private bool odbexist(int id)
        {
            return _appDbContext.Codes.Any(e => e.Id == id);
        }


        // Rota de Delete apaga por parametro Code
        [HttpDelete("{Id_Code}")]
        public async Task<IActionResult> Delete(int Id_Code)
        {
            var odbcode = await _appDbContext.Codes.FindAsync(Id_Code);
            if (odbcode == null)
            { return NotFound(); }

            _appDbContext.Codes.Remove(odbcode);
            await _appDbContext.SaveChangesAsync();
            return Ok
                (new
                {
                    Sucesso = true,
                    Mensagem = "Deletado com Sucesso!",
                    Data = System.DateTime.Now    
                });

        }
    }
}
