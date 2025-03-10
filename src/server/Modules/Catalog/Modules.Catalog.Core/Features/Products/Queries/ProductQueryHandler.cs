﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentPOS.Modules.Catalog.Core.Abstractions;
using FluentPOS.Modules.Catalog.Core.Entities;
using FluentPOS.Modules.Catalog.Core.Exceptions;
using FluentPOS.Shared.Core.Extensions;
using FluentPOS.Shared.Core.Wrapper;
using FluentPOS.Shared.DTOs.Catalogs.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FluentPOS.Modules.Catalog.Core.Features.Products.Queries
{
    internal class ProductQueryHandler :
        IRequestHandler<GetAllPagedProductsQuery, PaginatedResult<GetAllPagedProductsResponse>>,
        IRequestHandler<GetProductByIdQuery, Result<GetProductByIdResponse>>,
        IRequestHandler<GetProductImageQuery, Result<string>>
    {
        private readonly ICatalogDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ProductQueryHandler> _localizer;

        public ProductQueryHandler(ICatalogDbContext context, IMapper mapper, IStringLocalizer<ProductQueryHandler> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedResult<GetAllPagedProductsResponse>> Handle(GetAllPagedProductsQuery query, CancellationToken cancellationToken)
        {
            var queryable = _context.Products.ProjectTo<GetAllPagedProductsResponse>(_mapper.ConfigurationProvider).OrderBy(x => x.Id).AsQueryable();

            if (!string.IsNullOrEmpty(query.SearchString)) queryable = queryable.Where(p => p.Name.Contains(query.SearchString) ||
                p.BarcodeSymbology.Contains(query.SearchString) || p.Detail.Contains(query.SearchString));
            if (query.BrandId != Guid.Empty) queryable = queryable.Where(p => p.BrandId == query.BrandId);
            if (query.CategoryId != Guid.Empty) queryable = queryable.Where(p => p.CategoryId == query.CategoryId);

            var productList = await queryable
                .AsNoTracking()
                .ToPaginatedListAsync(query.PageNumber, query.PageSize);

            if (productList == null) throw new CatalogException(_localizer["Products Not Found!"]);

            var mappedProducts = _mapper.Map<PaginatedResult<GetAllPagedProductsResponse>>(productList);

            return mappedProducts;
        }

        public async Task<Result<GetProductByIdResponse>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await _context.Products.Where(p => p.Id == query.Id).FirstOrDefaultAsync(cancellationToken);
            if (product == null) throw new CatalogException(_localizer["Product Not Found!"]);
            var mappedProduct = _mapper.Map<GetProductByIdResponse>(product);
            return await Result<GetProductByIdResponse>.SuccessAsync(mappedProduct);
        }

        public async Task<Result<string>> Handle(GetProductImageQuery query, CancellationToken cancellationToken)
        {
            var data = await _context.Products.Where(p => p.Id == query.Id).Select(x => x.ImageUrl).FirstOrDefaultAsync(cancellationToken);
            return await Result<string>.SuccessAsync(data: data);
        }
    }
}