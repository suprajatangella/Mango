using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
	//[Authorize(Roles = "ADMIN")]
	public class ProductAPIController : ControllerBase
    {

        private readonly AppDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;
        public ProductAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }
        [HttpGet]
        [AllowAnonymous]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Product> objList = _db.products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
		[AllowAnonymous]
		public ResponseDto Get(int id)
        {
            try
            {
                Product product = _db.products.First(c => c.ProductId == id);
                if (product != null)
                {
                    _response.Result = _mapper.Map<ProductDto>(product);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }

        [HttpPost]
        //[Authorize(Roles ="ADMIN")]
        public ResponseDto Post([FromBody] ProductDto productDto)
        {
            try
            {
                Product obj = _mapper.Map<Product>(productDto);
                _db.products.Add(obj);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPut]
        //[Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] ProductDto productDto)
        {
            try
            {
                Product obj = _mapper.Map<Product>(productDto);
                _db.products.Update(obj);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        //[Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Product obj = _db.products.First(c => c.ProductId == id);
                _db.products.Remove(obj);
                _db.SaveChanges();

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}

