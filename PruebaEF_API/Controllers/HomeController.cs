using Microsoft.AspNetCore.Mvc;
using Api.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Api.Data.Repository.Response;
using System.ComponentModel;
using Api.Data.Model;

namespace PruebaEF_API.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly PruebaEfDbContext _dbContext;

        public HomeController(PruebaEfDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet]
        [Route("/get/person")]
        public async Task<List<PersonaDto>> GetPersona()
        {
            try
            {
                var persona = await _dbContext.Personas.Include(i=>i.IdDocumentoNavigation).Include(i=>i.IdEstadoCivilNavigation).ToListAsync();

                return AsignarCampos(persona);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("/get/personbyid")]
        public async Task<List<PersonaDto>> GetPersona(string documento)
        {
            try
            {
                var persona = await _dbContext.Personas.Where(i=>i.Documento.Equals(documento)).Include(i => i.IdDocumentoNavigation).Include(i => i.IdEstadoCivilNavigation).ToListAsync();

                return AsignarCampos(persona);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("/get/document")]
        public int GetDocumento(string documento)
        {
            var id = _dbContext.TipoDocumentos.First(i=>i.Documento.Equals(documento)).IdDocumento;
            return id;
        }

        [HttpGet]
        [Route("/get/AllDocuments")]
        public async Task<List<DocumentoDto>> GetDocumento()
        {
            List<DocumentoDto> dto = [];
            var documento = await _dbContext.TipoDocumentos.ToListAsync();
            foreach(var item in documento)
            {
                dto.Add(new DocumentoDto
                {
                    IdDocumento = item.IdDocumento,
                    Documento = item.Documento
                });
            }
            return dto;
        }

        [HttpGet]
        [Route("/get/AllMartalStatus")]
        public async Task<List<EstadoCivilDto>> GetEstadoCivil()
        {
            List<EstadoCivilDto> civils = [];
            var maritalStatul = await _dbContext.EstadoCivils.ToListAsync();
            foreach (var item in maritalStatul)
            {
                civils.Add(new EstadoCivilDto
                {
                    IdEstadoCivil = item.IdEstadoCivil,
                    EstadoCivil1 = item.EstadoCivil1
                });
            }
            return civils;
        }

        [HttpPost]
        [Route("/post/person")]
        public async Task<Response> PostPersona(PersonaDto personaDto)
        {
            try
            {
                Persona persona = new Persona();
                AsignarPersona(ref persona, personaDto);

                _dbContext.Personas.Add(persona);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = 500,
                    Description = ex.Message,
                    StatusCode = 500
                };
            }
            return new Response
            {
                Status = 200,
                Description = "Exisoso",
                StatusCode = 200
            };

        }

        [HttpPut]
        [Route("/put/person")]
        public async Task<Response> PutPersona(PersonaDto personaDto)
        {
            try
            {
                var item = await _dbContext.Personas.FirstOrDefaultAsync(i => i.Documento == personaDto.Documento);

                if (ValidarDatos(personaDto, ref item))
                {
                    await _dbContext.SaveChangesAsync();
                    return new Response
                    {
                        Status = 200,
                        Description = "Existoso",
                        StatusCode = 200

                    };
                }
                return new Response
                {
                    Status = 400,
                    Description = "Faltan datos necesarios por actualizar",
                    StatusCode = 400

                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = 500,
                    Description = "Error",
                    StatusCode = 500

                };
            }
        }

        [HttpDelete]
        [Route("/delete/person")]
        public async Task<Response> DetetePersona(int tipo, string documento)
        {
            try
            {
                var item = await _dbContext.Personas.FirstOrDefaultAsync(i => i.IdDocumento == tipo && i.Documento == documento);
                if(item != null)
                {
                    _ = _dbContext.Personas.Remove(item);
                    await _dbContext.SaveChangesAsync();
                    return new Response
                    {
                        Status = 200,
                        Description = "Existoso",
                        StatusCode = 200
                    };
                }

                return new Response
                {
                    Status = 200,
                    Description = string.Format("No existe persona con tipo de documento {0} y numero de documento {1}", tipo, documento),
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = 500,
                    Description = ex.Message,
                    StatusCode = 500
                };
            }
        }

        private bool ValidarDatos(PersonaDto personaDto, ref Persona item)
        {
            if (!string.IsNullOrEmpty(personaDto.Nombres))
                item.Nombres = personaDto.Nombres;
            if (!string.IsNullOrEmpty(personaDto.Apellidos))
                item.Apellido = personaDto.Apellidos;
            if (!string.IsNullOrEmpty(personaDto.ValorGanar.ToString()) && personaDto.ValorGanar != 0)
                item.ValorGanar = personaDto.ValorGanar;
            if (!string.IsNullOrEmpty(personaDto.EstadoCivil?.ToString()))
                item.IdEstadoCivil = Convert.ToInt32(personaDto.EstadoCivil);

            return true;
        }

        private void AsignarPersona(ref Persona persona, PersonaDto personaDto)
        {
            persona = new Persona
            {
                Nombres = personaDto.Nombres,
                Apellido = personaDto.Apellidos,
                Documento = personaDto.Documento,
                IdDocumento = Convert.ToInt32(personaDto.TipoDocumento),
                FechaNcacimiento = DateOnly.FromDateTime(Convert.ToDateTime(personaDto.FechaNacmiento)),
                Fecha = Convert.ToDateTime(personaDto.Fecha),
                IdEstadoCivil = Convert.ToInt32(personaDto.EstadoCivil),
                ValorGanar = personaDto.ValorGanar
            };
        }

        private List<PersonaDto> AsignarCampos(List<Persona> persona)
        {
            List<PersonaDto> personas = new List<PersonaDto>();

            foreach(var item in persona)
            {
                personas.Add(new PersonaDto
                {
                    Nombres = item.Nombres,
                    Apellidos = item.Apellido,
                    TipoDocumento = item.IdDocumentoNavigation.Documento,
                    Documento = item.Documento,
                    EstadoCivil = item.IdEstadoCivilNavigation?.EstadoCivil1,
                    FechaNacmiento = item.FechaNcacimiento.ToString(),
                    Fecha = item.Fecha.ToString("dd/MMM/yyyy"),
                    ValorGanar = item.ValorGanar
                });
            }
            return personas;
        }
    }
}
