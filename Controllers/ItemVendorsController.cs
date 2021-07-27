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
    [Route("/api/{v:apiVersion}/item-vendors")]
    [Authorize]
    public class ItemVendorsController : ControllerBase
    {
        #region Controller Initializer
        private readonly IMapper mapper;
        private readonly IItemVendorRepo repo;
        private readonly IUnitOfWork uow;
        private readonly ILogger<ItemVendorsController> logger;
        public ItemVendorsController(IMapper mapper, IItemVendorRepo repo, IUnitOfWork uow, ILogger<ItemVendorsController> logger)
        {
            this.logger = logger;
            this.uow = uow;
            this.repo = repo;
            this.mapper = mapper;
        }
        #endregion

        #region public async Task<IActionResult> GetItemVendor(long id)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemVendor(long id)
        {
            ItemVendor result = new ItemVendor();
            try
            {
                result = await repo.GetItemVendor(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on GetItemVendor(long id)");
            }            

            if (result == null)
            {
                ErrorResponseDto error = new ErrorResponseDto();
                error.ErrorCode = "NF001";
                error.ErrorMessage = "The object does not exist.";
                return NotFound(error);
            }

            return Ok(mapper.Map<ItemVendor, ItemVendorDto>(result));
        }
        #endregion

        #region public async Task<IActionResult> GetItemVendors()
        [HttpGet]
        public async Task<IActionResult> GetItemVendors()
        {
            List<ItemVendor> result = new List<ItemVendor>();
            try
            {
                result = await repo.GetItemVendors();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on GetItemVendors()");
            }            
            return Ok(mapper.Map<List<ItemVendor>, List<ItemVendorDto>>(result));
        }
        #endregion

        #region public async Task<IActionResult> CreateItemVendor([FromBody] ItemVendorDto dto)
        [HttpPost]
        public async Task<IActionResult> CreateItemVendor([FromBody] ItemVendorDto dto)
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

            ItemVendor record = new ItemVendor();
            try
            {
                // Mapping and completing record for database
                record = mapper.Map<ItemVendorDto, ItemVendor>(dto);
                record.Status = "A";
                record.CreatedDate = DateTime.Now;
                record.CreatedBy = _email;
                record.UpdatedDate = DateTime.Now;
                record.UpdatedBy = _email;
                repo.CreateItemVendor(record);
                await uow.CompleteAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on CreateItemVendor([FromBody] ItemVendorDto dto)");
            }

            var result = mapper.Map<ItemVendor, ItemVendorDto>(record);
            return CreatedAtAction(nameof(GetItemVendor), new { id = record.Id }, result);
        }
        #endregion

        #region public async Task<IActionResult> UpdateItemVendor(long id, [FromBody] ItemVendorDto dto)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemVendor(long id, [FromBody] ItemVendorDto dto)
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
            ItemVendor record = new ItemVendor();
            try
            {
                record = await repo.GetItemVendor(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on UpdateItemVendor(long id, [FromBody] ItemVendorDto dto)");
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
                mapper.Map<ItemVendorDto, ItemVendor>(dto, record);
                record.Status = "U";
                record.UpdatedDate = DateTime.Now;
                record.UpdatedBy = _email;
                await uow.CompleteAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on UpdateItemVendor(long id, [FromBody] ItemVendorDto dto)");
            }            

            var result = mapper.Map<ItemVendor, ItemVendorDto>(record);
            return Ok(result);
        }
        #endregion

        #region public async Task<IActionResult> DeleteItemVendor(long id)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemVendor(long id)
        {
            // Pick up the identity token information
            var _identity = HttpContext.User.Identity as ClaimsIdentity;
            string _email = _identity.FindFirst("email").Value;

            ItemVendor record = new ItemVendor();
            try
            {
                record = await repo.GetItemVendor(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on DeleteItemVendor(long id)");
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
                logger.LogError(ex, "This error happened on DeleteItemVendor(long id)");
            }

            var result = mapper.Map<ItemVendor, ItemVendorDto>(record);
            return Ok(result);
        }
        #endregion
    }
}