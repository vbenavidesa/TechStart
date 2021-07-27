using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("/api/{v:apiVersion}/items")]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        #region Controller Initializer
        private readonly IMapper mapper;
        private readonly IItemRepo repo;
        private readonly IUnitOfWork uow;
        private readonly ILogger<ItemsController> logger;
        public ItemsController(IMapper mapper, IItemRepo repo, IUnitOfWork uow, ILogger<ItemsController> logger)
        {
            this.logger = logger;
            this.uow = uow;
            this.repo = repo;
            this.mapper = mapper;
        }
        #endregion

        #region public async Task<IActionResult> GetItem(long itemNumber)
        [HttpGet("{itemNumber}")]
        public async Task<IActionResult> GetItem(long itemNumber)
        {
            Item result = new Item();
            try
            {
                result = await repo.GetItem(itemNumber);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on GetItem(long id)");
            }            

            if (result == null)
            {
                ErrorResponseDto error = new ErrorResponseDto();
                error.ErrorCode = "NF001";
                error.ErrorMessage = "The object does not exist.";
                return NotFound(error);
            }

            return Ok(mapper.Map<Item, ItemDto>(result));
        }
        #endregion

        #region public async Task<IActionResult> GetItems()
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            List<Item> result = new List<Item>();
            try
            {
                result = await repo.GetItems();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on GetItems()");
            }            
            return Ok(mapper.Map<List<Item>, List<ItemDto>>(result));
        }
        #endregion

        #region public async Task<IActionResult> CreateItem([FromBody] ItemDto dto)
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] ItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                ErrorResponseDto error = new ErrorResponseDto();
                error.ErrorCode = "IM001";
                error.ErrorMessage = "The information is invalid.";
                return BadRequest(error);
            }
            else if (dto.UPC.All(char.IsDigit) == false)
            {
                ErrorResponseDto error = new ErrorResponseDto();
                error.ErrorCode = "IM002";
                error.ErrorMessage = "The UPC should be a 12 number-only code.";
                return BadRequest(error);
            }

            // Pick up the identity token information
            var _identity = HttpContext.User.Identity as ClaimsIdentity;
            string _email = _identity.FindFirst("email").Value;

            Item record = new Item();
            try
            {
                // Mapping and completing record for database
                record = mapper.Map<ItemDto, Item>(dto);
                record.Status = "A";
                record.CreatedDate = DateTime.Now;
                record.CreatedBy = _email;
                record.UpdatedDate = DateTime.Now;
                record.UpdatedBy = _email;
                repo.CreateItem(record);
                await uow.CompleteAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on CreateItem([FromBody] ItemDto dto)");
            }

            var result = mapper.Map<Item, ItemDto>(record);
            return CreatedAtAction(nameof(GetItem), new { itemNumber = record.ItemNumber }, result);
        }
        #endregion

        #region public async Task<IActionResult> UpdateItem(long itemNumber, [FromBody] ItemDto dto)
        [HttpPut("{itemNumber}")]
        public async Task<IActionResult> UpdateItem(long itemNumber, [FromBody] ItemDto dto)
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
            Item record = new Item();
            try
            {
                record = await repo.GetItem(itemNumber);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on UpdateItem(long itemNumber, [FromBody] ItemDto dto)");
            }

            // Validators for data
            if (record == null)
            {
                ErrorResponseDto error = new ErrorResponseDto();
                error.ErrorCode = "NF001";
                error.ErrorMessage = "El objeto que está buscando no existe.";
                return NotFound(error);
            }

            try
            {
                mapper.Map<ItemDto, Item>(dto, record);
                record.Status = "U";
                record.UpdatedDate = DateTime.Now;
                record.UpdatedBy = _email;
                await uow.CompleteAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on UpdateItem(long itemNumber, [FromBody] ItemDto dto)");
            }            

            var result = mapper.Map<Item, ItemDto>(record);
            return Ok(result);
        }
        #endregion

        #region public async Task<IActionResult> DeleteItem(long itemNumber)
        [HttpDelete("{itemNumber}")]
        public async Task<IActionResult> DeleteItem(long itemNumber)
        {
            // Pick up the identity token information
            var _identity = HttpContext.User.Identity as ClaimsIdentity;
            string _email = _identity.FindFirst("email").Value;

            Item record = new Item();
            try
            {
                record = await repo.GetItem(itemNumber);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This error happened on DeleteItem(long id)");
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
                logger.LogError(ex, "This error happened on DeleteItem(long id)");
            }

            var result = mapper.Map<Item, ItemDto>(record);
            return Ok(result);
        }
        #endregion
    }
}