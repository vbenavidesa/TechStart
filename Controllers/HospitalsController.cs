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
    [Route("/api/{v:apiVersion}/hospitals")]
    [Authorize]
    public class HospitalsController : ControllerBase
    {
        #region Controller Initializer
        private readonly IMapper mapper;
        private readonly IHospitalRepo repo;
        private readonly IUnitOfWork uow;
        private readonly ILogger<HospitalsController> logger;
        public HospitalsController(IMapper mapper, IHospitalRepo repo, IUnitOfWork uow, ILogger<HospitalsController> logger)
        {
            this.logger = logger;
            this.uow = uow;
            this.repo = repo;
            this.mapper = mapper;
        }
        #endregion

        #region public async Task<IActionResult> GetHospital(long id)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHospital(long id)
        {
            Hospital result = new Hospital();
            try
            {
                result = await repo.GetHospital(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on GetHospital(long id)");
            }            

            if (result == null)
            {
                ErrorResponseDto error = new ErrorResponseDto();
                error.ErrorCode = "NF001";
                error.ErrorMessage = "The object does not exist.";
                return NotFound(error);
            }

            return Ok(mapper.Map<Hospital, HospitalDto>(result));
        }
        #endregion

        #region public async Task<IActionResult> GetHospitals()
        [HttpGet]
        public async Task<IActionResult> GetHospitals()
        {
            List<Hospital> result = new List<Hospital>();
            try
            {
                result = await repo.GetHospitals();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on GetHospitals()");
            }            
            return Ok(mapper.Map<List<Hospital>, List<HospitalDto>>(result));
        }
        #endregion

        #region public async Task<IActionResult> CreateHospital([FromBody] HospitalDto dto)
        [HttpPost]
        public async Task<IActionResult> CreateHospital([FromBody] HospitalDto dto)
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

            Hospital record = new Hospital();
            try
            {
                // Mapping and completing record for database
                record = mapper.Map<HospitalDto, Hospital>(dto);
                record.Status = "A";
                record.CreatedDate = DateTime.Now;
                record.CreatedBy = _email;
                record.UpdatedDate = DateTime.Now;
                record.UpdatedBy = _email;
                repo.CreateHospital(record);
                await uow.CompleteAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on CreateHospital([FromBody] HospitalDto dto)");
            }

            var result = mapper.Map<Hospital, HospitalDto>(record);
            return CreatedAtAction(nameof(GetHospital), new { id = record.Id }, result);
        }
        #endregion

        #region public async Task<IActionResult> UpdateHospital(long id, [FromBody] HospitalDto dto)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHospital(long id, [FromBody] HospitalDto dto)
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
            Hospital record = new Hospital();
            try
            {
                record = await repo.GetHospital(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on UpdateHospital(long id, [FromBody] HospitalDto dto)");
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
                mapper.Map<HospitalDto, Hospital>(dto, record);
                record.Status = "U";
                record.UpdatedDate = DateTime.Now;
                record.UpdatedBy = _email;
                await uow.CompleteAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on UpdateHospital(long id, [FromBody] HospitalDto dto)");
            }            

            var result = mapper.Map<Hospital, HospitalDto>(record);
            return Ok(result);
        }
        #endregion

        #region public async Task<IActionResult> DeleteHospital(long id)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHospital(long id)
        {
            // Pick up the identity token information
            var _identity = HttpContext.User.Identity as ClaimsIdentity;
            string _email = _identity.FindFirst("email").Value;

            Hospital record = new Hospital();
            try
            {
                record = await repo.GetHospital(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on DeleteHospital(long id)");
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
                logger.LogError(ex, "This error happened on DeleteHospital(long id)");
            }

            var result = mapper.Map<Hospital, HospitalDto>(record);
            return Ok(result);
        }
        #endregion
    }
}