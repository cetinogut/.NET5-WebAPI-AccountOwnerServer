using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountOwnerServer.Controllers
{
    //[Route("api/owner")]
    [ApiController]
    [Route("api/[controller]")]
    public class OwnerController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public OwnerController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllOwners()
        {
            try
            {
                var owners = _repository.Owner.GetAllOwners();
                var ownersResult = _mapper.Map<IEnumerable<OwnerDto>>(owners);
                _logger.LogInformation($"Returned all owners from database.");
                //return Ok(owners); // before DTOs
                return Ok(ownersResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        //[HttpGet]
        //public IActionResult GetOwners([FromQuery] OwnerParameters ownerParameters)
        //{
        //    var owners = _repository.Owner.GetOwners(ownerParameters);
        //    _logger.LogInformation($"Returned {owners.Count()} owners from database.");
        //    return Ok(owners);
        //}

        

        //[HttpGet("{id}")]
        [HttpGet("{id}", Name = "OwnerById")] //With this modification, we are setting the name for the action. This name will come in handy in the action method for creating a new owner.
        public IActionResult GetOwnerById(Guid id)
        {
            try
            {
                var owner = _repository.Owner.GetOwnerById(id);

                if (owner == null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned owner with id: {id}");

                    var ownerResult = _mapper.Map<OwnerDto>(owner);
                    return Ok(ownerResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}/account")]
        public IActionResult GetOwnerWithDetails(Guid id)
        {
            try
            {
                var owner = _repository.Owner.GetOwnerWithDetails(id);

                if (owner == null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned owner with details for id: {id}");

                    var ownerDetailedResult = _mapper.Map<OwnerWithAccountsDto>(owner);
                    return Ok(ownerDetailedResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerWithDetails action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateOwner([FromBody] OwnerForCreationDto owner) //We are not collecting it from the Uri but from the request body. Thus the usage of the [FromBody] attribute. Also, the owner object is a complex type and because of that, we have to use [FromBody].
        {
            try
            {
                if (owner == null)
                {
                    _logger.LogError("Owner object sent from client is null.");
                    return BadRequest("Owner object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid owner object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var ownerEntity = _mapper.Map<Owner>(owner);

                _repository.Owner.CreateOwner(ownerEntity);
                _repository.Save();

                var createdOwner = _mapper.Map<OwnerDto>(ownerEntity);

                return CreatedAtRoute("OwnerById", new { id = createdOwner.OwnerId }, createdOwner); //CreatedAtRoute will return a status code 201, which stands for Created
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOwner(Guid id, [FromBody] OwnerForUpdateDto owner)
        {
            try
            {
                if (owner == null)
                {
                    _logger.LogError("Owner object sent from client is null.");
                    return BadRequest("Owner object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid owner object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var ownerEntity = _repository.Owner.GetOwnerById(id);
                if (ownerEntity == null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(owner, ownerEntity);

                _repository.Owner.UpdateOwner(ownerEntity);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOwner(Guid id)
        {
            try
            {
                var owner = _repository.Owner.GetOwnerById(id);
                if (owner == null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                if (_repository.Account.AccountsByOwner(id).Any())
                {
                    _logger.LogError($"Cannot delete owner with id: {id}. It has related accounts. Delete those accounts first");
                    return BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
                }

                _repository.Owner.DeleteOwner(owner);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }   
}

//The IActionResult interface supports using a variety of methods, which return not only the result but the status codes as well.
//In this situation, the OK method returns all the owners and also the status code 200 which stands for OK
//If an exception occurs, we will return the internal server error with the status code 500.
