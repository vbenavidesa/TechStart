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
    [Route("/api/{v:apiVersion}/pharmacy-inventories")]
    [Authorize]
    public class PharmacyInventoriesController : ControllerBase
    {
        #region Controller Initializer
        private readonly IMapper mapper;
        private readonly IPharmacyInventoryRepo repo;
        private readonly IUnitOfWork uow;
        private readonly ILogger<PharmacyInventoriesController> logger;
        private readonly IItemRepo itemRepo;
        public PharmacyInventoriesController(IMapper mapper, IPharmacyInventoryRepo repo, IItemRepo itemRepo, IUnitOfWork uow, ILogger<PharmacyInventoriesController> logger)
        {
            this.itemRepo = itemRepo;
            this.logger = logger;
            this.uow = uow;
            this.repo = repo;
            this.mapper = mapper;
        }
        #endregion

        #region public async Task<IActionResult> GetPharmacyInventory(long id)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPharmacyInventory(long id)
        {
            PharmacyInventory result = new PharmacyInventory();
            try
            {
                result = await repo.GetPharmacyInventory(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on GetPharmacyInventory(long id)");
            }

            if (result == null)
            {
                ErrorResponseDto error = new ErrorResponseDto();
                error.ErrorCode = "NF001";
                error.ErrorMessage = "The object does not exist.";
                return NotFound(error);
            }

            return Ok(mapper.Map<PharmacyInventory, PharmacyInventoryDto>(result));
        }
        #endregion

        #region public async Task<IActionResult> GetPharmacyInventories()
        [HttpGet]
        public async Task<IActionResult> GetPharmacyInventories()
        {
            List<PharmacyInventory> result = new List<PharmacyInventory>();
            try
            {
                result = await repo.GetPharmacyInventories();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on GetPharmacyInventories()");
            }
            return Ok(mapper.Map<List<PharmacyInventory>, List<PharmacyInventoryDto>>(result));
        }
        #endregion

        #region public async Task<IActionResult> CreatePharmacyInventory([FromBody] PharmacyInventoryDto dto)
        [HttpPost]
        public async Task<IActionResult> CreatePharmacyInventory([FromBody] PharmacyInventoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                ErrorResponseDto error = new ErrorResponseDto();
                error.ErrorCode = "IM001";
                error.ErrorMessage = "The information is invalid.";
                return BadRequest(error);
            }
            else if (dto.QtyOnHand <= 0)
            {
                ErrorResponseDto error = new ErrorResponseDto();
                error.ErrorCode = "DI001";
                error.ErrorMessage = "The Quantity on hand should be higher than 0.";
                return BadRequest(error);
            }

            // Validate the existence of an item
            Item item = new Item();
            try
            {
                item = await itemRepo.GetItem(dto.ItemId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on GetItem(long id)");
            }            

            if (item == null)
            {
                ErrorResponseDto error = new ErrorResponseDto();
                error.ErrorCode = "NF001";
                error.ErrorMessage = "The item you are selecting does not exist.";
                return NotFound(error);
            }

            // Pick up the identity token information
            var _identity = HttpContext.User.Identity as ClaimsIdentity;
            string _email = _identity.FindFirst("email").Value;

            PharmacyInventory record = new PharmacyInventory();
            try
            {
                // Mapping and completing record for database
                record = mapper.Map<PharmacyInventoryDto, PharmacyInventory>(dto);
                record.Status = "A";
                record.CreatedDate = DateTime.Now;
                record.CreatedBy = _email;
                record.UpdatedDate = DateTime.Now;
                record.UpdatedBy = _email;
                repo.CreatePharmacyInventory(record);
                await uow.CompleteAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on CreatePharmacyInventory([FromBody] PharmacyInventoryDto dto)");
            }

            var result = mapper.Map<PharmacyInventory, PharmacyInventoryDto>(record);
            return CreatedAtAction(nameof(GetPharmacyInventory), new { id = record.Id }, result);
        }
        #endregion

        #region public async Task<IActionResult> UpdatePharmacyInventory(long id, [FromBody] PharmacyInventoryDto dto)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePharmacyInventory(long id, [FromBody] PharmacyInventoryDto dto)
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
            PharmacyInventory record = new PharmacyInventory();
            try
            {
                record = await repo.GetPharmacyInventory(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on UpdatePharmacyInventory(long id, [FromBody] PharmacyInventoryDto dto)");
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
                mapper.Map<PharmacyInventoryDto, PharmacyInventory>(dto, record);
                record.Status = "U";
                record.UpdatedDate = DateTime.Now;
                record.UpdatedBy = _email;
                await uow.CompleteAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on UpdatePharmacyInventory(long id, [FromBody] PharmacyInventoryDto dto)");
            }

            var result = mapper.Map<PharmacyInventory, PharmacyInventoryDto>(record);
            return Ok(result);
        }
        #endregion

        #region public async Task<IActionResult> DeletePharmacyInventory(long id)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePharmacyInventory(long id)
        {
            // Pick up the identity token information
            var _identity = HttpContext.User.Identity as ClaimsIdentity;
            string _email = _identity.FindFirst("email").Value;

            PharmacyInventory record = new PharmacyInventory();
            try
            {
                record = await repo.GetPharmacyInventory(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on DeletePharmacyInventory(long id)");
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
                logger.LogError(ex, "This error happened on DeletePharmacyInventory(long id)");
            }

            var result = mapper.Map<PharmacyInventory, PharmacyInventoryDto>(record);
            return Ok(result);
        }
        #endregion
    }
}