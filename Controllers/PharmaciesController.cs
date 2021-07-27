using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechStart.Core;
using TechStart.Dtos;
using TechStart.Dtos.Helpers;
using TechStart.Models;

namespace dianAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("/api/{v:apiVersion}/pharmacies")]
    [Authorize]
    public class PharmaciesController : ControllerBase
    {
        #region Controller Initializer
        private readonly IMapper mapper;
        private readonly IPharmacyRepo repo;
        private readonly IUnitOfWork uow;
        private readonly ILogger<PharmaciesController> logger;
        public PharmaciesController(IMapper mapper, IPharmacyRepo repo, IUnitOfWork uow, ILogger<PharmaciesController> logger)
        {
            this.logger = logger;
            this.uow = uow;
            this.repo = repo;
            this.mapper = mapper;
        }
        #endregion

        #region public async Task<IActionResult> GetPharmacy(long id)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPharmacy(long id)
        {
            Pharmacy result = new Pharmacy();
            try
            {
                result = await repo.GetPharmacy(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on GetPharmacy(long id)");
            }            

            if (result == null)
            {
                ErrorResponseDto error = new ErrorResponseDto();
                error.ErrorCode = "NF001";
                error.ErrorMessage = "The object does not exist.";
                return NotFound(error);
            }

            return Ok(mapper.Map<Pharmacy, PharmacyDto>(result));
        }
        #endregion

        #region public async Task<IActionResult> GetPharmacys()
        [HttpGet]
        public async Task<IActionResult> GetPharmacies()
        {
            List<Pharmacy> result = new List<Pharmacy>();
            try
            {
                result = await repo.GetPharmacies();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on GetPharmacies()");
            }            
            return Ok(mapper.Map<List<Pharmacy>, List<PharmacyDto>>(result));
        }
        #endregion

        #region public async Task<IActionResult> CreatePharmacy([FromBody] PharmacyDto dto)
        [HttpPost]
        public async Task<IActionResult> CreatePharmacy([FromBody] PharmacyDto dto)
        {
            if (!ModelState.IsValid)
            {
                ErrorResponseDto error = new ErrorResponseDto();
                error.ErrorCode = "IM001";
                error.ErrorMessage = "The information is invalid.";
                return BadRequest(error);
            }

            // Pick up the identity token information
            var _identity = HttpContext.User.Identity as ClaimsIdentity;
            string _email = _identity.FindFirst("email").Value;

            Pharmacy record = new Pharmacy();
            try
            {
                // Mapping and completing record for database
                record = mapper.Map<PharmacyDto, Pharmacy>(dto);
                record.Status = "A";
                record.CreatedDate = DateTime.Now;
                record.CreatedBy = _email;
                record.UpdatedDate = DateTime.Now;
                record.UpdatedBy = _email;
                repo.CreatePharmacy(record);
                await uow.CompleteAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on CreatePharmacy([FromBody] PharmacyDto dto)");
            }

            var result = mapper.Map<Pharmacy, PharmacyDto>(record);
            return CreatedAtAction(nameof(GetPharmacy), new { id = record.Id }, result);
        }
        #endregion

        #region public async Task<IActionResult> UpdatePharmacy(long id, [FromBody] PharmacyDto dto)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePharmacy(long id, [FromBody] PharmacyDto dto)
        {
            if (!ModelState.IsValid)
            {
                ErrorResponseDto error = new ErrorResponseDto();
                error.ErrorCode = "IM001";
                error.ErrorMessage = "The information is invalid.";
                return BadRequest(error);
            }

            // Pick up the identity token information
            var _identity = HttpContext.User.Identity as ClaimsIdentity;
            string _email = _identity.FindFirst("email").Value;

            // Mapping and completing record for database
            Pharmacy record = new Pharmacy();
            try
            {
                record = await repo.GetPharmacy(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on UpdatePharmacy(long id, [FromBody] PharmacyDto dto)");
            }

            // Validators for data
            if (record == null)
            {
                ErrorResponseDto error = new ErrorResponseDto();
                error.ErrorCode = "NF001";
                error.ErrorMessage = "The object does not exist.";
                return NotFound(error);
            }

            try
            {
                mapper.Map<PharmacyDto, Pharmacy>(dto, record);
                record.Status = "U";
                record.UpdatedDate = DateTime.Now;
                record.UpdatedBy = _email;
                await uow.CompleteAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on UpdatePharmacy(long id, [FromBody] PharmacyDto dto)");
            }            

            var result = mapper.Map<Pharmacy, PharmacyDto>(record);
            return Ok(result);
        }
        #endregion

        #region public async Task<IActionResult> DeletePharmacy(long id)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePharmacy(long id)
        {
            // Pick up the identity token information
            var _identity = HttpContext.User.Identity as ClaimsIdentity;
            string _email = _identity.FindFirst("email").Value;

            Pharmacy record = new Pharmacy();
            try
            {
                record = await repo.GetPharmacy(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on DeletePharmacy(long id)");
            }

            // Validators for data
            if (record == null)
            {
                ErrorResponseDto error = new ErrorResponseDto();
                error.ErrorCode = "NF001";
                error.ErrorMessage = "The object does not exist.";
                return NotFound(error);
            }

            try
            {
                record.Status = "D";
                record.UpdatedDate = DateTime.Now;
                record.UpdatedBy = _email;
                await uow.CompleteAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on DeletePharmacy(long id)");
            }

            var result = mapper.Map<Pharmacy, PharmacyDto>(record);
            return Ok(result);
        }
        #endregion
    }
}