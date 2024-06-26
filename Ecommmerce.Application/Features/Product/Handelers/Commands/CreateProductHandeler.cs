﻿using Ecommerce.Application.Models.Email;
using Ecommmerce.Application.DTO.Entities.Product.Validators;
using Ecommmerce.Application.Persistance.Email;
using Ecommmerce.Application.Responses;


namespace Ecommmerce.Application.Features.Categories.Handelers.Commands
{
    public class CreateProductHandeler : IRequestHandler<CreateProductRequest, BaseCommandResponse>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly IEmailSender emailSender;

        public CreateProductHandeler(IProductRepository productRepository, IMapper mapper ,IEmailSender emailSender)
        {
            this.productRepository=productRepository;
            this.mapper=mapper;
            this.emailSender=emailSender;
        }

        public async Task<BaseCommandResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new ProductValidator(productRepository);
            var validationResult = await validator.ValidateAsync(request.ProductDTO, cancellationToken);
            if (validationResult.IsValid == false)
            {
                response.Success=false;
                response.Message="Failed to Create Product";
                response.Errors=validationResult.Errors.Select(x=> x.ErrorMessage).ToList();
                return response;
            }
            var product = mapper.Map<Ecommerce.Domain.Entities.Product>(request.ProductDTO);
            await productRepository.CreateAsync(product);
            response.Id=product.Id;
            response.Success = true;
            response.Message="Created successfully";
            try
            {
                var email = new EmailMessage()
                {
                    To="mido.aaa27@gmail.com",
                    Subject="Check New Products",
                    Body =$"There are new products you may be interested {request.ProductDTO.Name} "
                        
                };
               var result= await emailSender.SendEmailAsync(email);
                if(result != false)
                {
                    response.Message="Created and Email sent successfully";
                }
                else
                {
                    response.Message="Failed to create ";
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }
    }
}
